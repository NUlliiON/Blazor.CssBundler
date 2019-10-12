using Blazor.CssBundler.Analyzing;
using Blazor.CssBundler.Bundle.FileManager;
using Blazor.CssBundler.Bundle.Helper;
using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Models;
using Blazor.CssBundler.Models.Bundle;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Razor;
using ExCSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Blazor.CssBundler.Razor.RazorHelper;

namespace Blazor.CssBundler.Bundle.App
{
    class AutoApplicationBundler : AutoBundler<ApplicationSettings, ApplicationBundleInfo>
    {
        private RazorEngine _razorEngine;
        private ApplicationFileManager _fileManager;
        private ProjectAnalyzer _projAnalyzer;
        private AssemblyAnalyzer _asmAnalyzer;

        public AutoApplicationBundler(ApplicationSettings settings)
            : base(settings)
        {
            _razorEngine = new RazorEngine();
            _razorEngine.AddProjectDirectory(Settings.ProjectName, Settings.ProjectDirectory);

            _fileManager = new ApplicationFileManager()
            {
                TempStylesFilePath = Path.GetTempFileName()
            };

            _projAnalyzer = new ProjectAnalyzer();
            _asmAnalyzer = new AssemblyAnalyzer();
        }

        public override async Task<ApplicationBundleInfo> Build()
        {
            OnBundlingStart();
            BuildStopWatch.Reset();
            BuildStopWatch.Start();
            var bundleInfo = new ApplicationBundleInfo();
            bundleInfo.OutputFilePath = Path.Combine(Settings.OutputCssDirectory, "bundle.css");

            if (_fileManager.TempCssFileExists())
            {
                await _fileManager.ClearTmpCssFileAsync();
            }
            else
            {
                await _fileManager.CreateTmpCssFileAsync();
            }

            ProjectAnalyzingResult projAnalyzeRes = _projAnalyzer.Analyze(Settings.ProjectFilePath);
            foreach (var pckgRef in projAnalyzeRes.PackageReferences.Where(x => !x.Name.StartsWith("System") && !x.Name.StartsWith("Microsoft")))
            {
                /* Extract styles from assembly */
                string mainAsmPath = NugetHelper.MakeAssemblyPath(pckgRef.Name, pckgRef.Version, "netstandard2.0");
                var mainStylesheet = CssParser.Parse(BundleHelper.GetStylesFromAssembly(mainAsmPath));
                if (BundleHelper.HasIsolatedCss(mainAsmPath))
                {
                    BundleHelper.AppendStylesToFile(_fileManager.TempStylesFilePath, mainStylesheet);
                }

                /* Extract styles from assembly references */
                AssemblyAnalyzingResult asmAnalyzeRes = _asmAnalyzer.Analyze(mainAsmPath);
                foreach (string asmPath in asmAnalyzeRes.AssemblyPaths)
                {
                    if (BundleHelper.HasIsolatedCss(asmPath))
                    {
                        var stylesheet = CssParser.Parse(BundleHelper.GetStylesFromAssembly(asmPath));
                        BundleHelper.AppendStylesToFile(_fileManager.TempStylesFilePath, stylesheet);
                    }
                }
            }
            foreach (var @ref in projAnalyzeRes.References)
            {
                string mainAsmPath = @ref.HintPath;
                if (@ref.HintPath.StartsWith("..\\"))
                {
                    mainAsmPath = Path.Combine(Settings.ProjectDirectory, mainAsmPath);
                }

                /* Extract styles from assembly */
                if (BundleHelper.HasIsolatedCss(mainAsmPath))
                {
                    var mainStylesheet = CssParser.Parse(BundleHelper.GetStylesFromAssembly(mainAsmPath));
                    BundleHelper.AppendStylesToFile(_fileManager.TempStylesFilePath, mainStylesheet);
                }

                /* Extract styles from assembly references */
                AssemblyAnalyzingResult asmAnalyzeRes = _asmAnalyzer.Analyze(mainAsmPath);
                foreach (string asmPath in asmAnalyzeRes.AssemblyPaths)
                {
                    Stylesheet stylesheet = CssParser.Parse(BundleHelper.GetStylesFromAssembly(asmPath));
                    if (BundleHelper.HasIsolatedCss(asmPath))
                    {
                        BundleHelper.AppendStylesToFile(_fileManager.TempStylesFilePath, stylesheet);
                    }
                }
            }

            string pagesDirPath = Path.Combine(Settings.ProjectDirectory, "Pages");
            string sharedDirPath = Path.Combine(Settings.ProjectDirectory, "Shared");
            BuildByDirectory(pagesDirPath, bundleInfo);
            BuildByDirectory(sharedDirPath, bundleInfo);

            File.WriteAllText(bundleInfo.OutputFilePath, File.ReadAllText(_fileManager.TempStylesFilePath)); // generating css bundle
            BuildStopWatch.Stop();
            bundleInfo.BundlingTime = BuildStopWatch.Elapsed;

            if (bundleInfo.Succeed)
            {
                OnBundlingEnd(bundleInfo);
            }
            else
            {
                OnBundlingError(bundleInfo);
            }

            return bundleInfo;
        }

        private void BuildByDependencies(string assemblyPath)
        {

        }

        private void BuildByDirectory(string dirName, ApplicationBundleInfo curBundleInfo)
        {
            try
            {
                foreach (FileInfo cssFile in new DirectoryInfo(dirName).GetFiles(Settings.CssRazorSearchPattern, SearchOption.AllDirectories))
                {
                    var stylesheet = CssParser.Parse(File.ReadAllText(cssFile.FullName));
                    var children = (List<IStylesheetNode>)stylesheet.Children;
                    // if contains zero styles
                    if (children.Count == 0)
                    {
                        continue;
                    }

                    string relativeCssPath = Path.GetRelativePath(Settings.ProjectDirectory, cssFile.FullName);
                    string cssFileName = Path.GetFileNameWithoutExtension(relativeCssPath);

                    RazorDirectory razorDir = RazorDirectory.Razor;
                    RazorFileExtension razorExt = RazorFileExtension.GenCsharp;

                    RazorFile razorFile = _razorEngine.FindFile(Settings.ProjectName, cssFileName, razorDir, razorExt);
                    string @namespace = _razorEngine.GetNamespaceFromFile(Settings.ProjectName, cssFileName, razorDir, razorExt);

                    foreach (IStyleRule rule in children)
                    {
                        if (rule.Type != RuleType.Style)
                        {
                            continue;
                        }

                        if (rule.SelectorText == null)
                        {
                            TextRange range = rule.Owner.StylesheetText.Range;
                            throw new InvalidCssSyntaxException(cssFile.FullName, range.End.Line, range.End.Column);
                        }

                        if (razorFile != null)
                        {
                            rule.SelectorText = BundleHelper.BuildCssClasses(@namespace, cssFileName.Replace(".razor", ""), rule.SelectorText);
                        }
                    }
                    BundleHelper.AppendStylesToFile(_fileManager.TempStylesFilePath, stylesheet);
                    curBundleInfo.CombinedFilesCount++;
                }
                curBundleInfo.Succeed = true;
            }
            catch (IOException ex)
            {
                curBundleInfo.Errors.Add(new Error("IOException", ex.Message));
            }
            catch (InvalidCssSyntaxException ex)
            {
                curBundleInfo.Errors.Add(new Error("CssSyntax", string.Format("invalid css syntax in file by path: {0};\nLine: {1}, Column: {2}", ex.FilePath, ex.Line, ex.Column)));
            }
        }

        protected override void FileWatcherRenamed(object sender, RenamedEventArgs e)
        {
            base.FileWatcherRenamed(sender, e);
        }

        protected override void FileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            base.FileWatcherChanged(sender, e);
        }

        protected override void FileWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            base.FileWatcherDeleted(sender, e);
        }

        protected override void FileWatcherCreated(object sender, FileSystemEventArgs e)
        {
            base.FileWatcherCreated(sender, e);
        }
    }
}
