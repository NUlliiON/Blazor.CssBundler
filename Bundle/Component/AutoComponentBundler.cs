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
using System.Threading.Tasks;
using static Blazor.CssBundler.Razor.RazorHelper;

namespace Blazor.CssBundler.Bundle.Component
{
    class AutoComponentBundler : AutoBundler<ComponentSettings, ComponentBundleInfo>
    {
        private RazorEngine _razorEngine;
        private ComponentFileManager _fileManager;

        public AutoComponentBundler(ComponentSettings settings)
            : base (settings)
        {
            _razorEngine = new RazorEngine();
            _razorEngine.AddProjectDirectory(Settings.ProjectName, Settings.ProjectDirectory);

            _fileManager = new ComponentFileManager()
            {
                TempStylesFilePath = Path.GetTempFileName()
            };
        }

        public override async Task<ComponentBundleInfo> Build()
        {
            OnBundlingStart();
            BuildStopWatch.Reset();
            BuildStopWatch.Start();
            var bundleInfo = new ComponentBundleInfo();
            bundleInfo.PathToBundledAssembly = Settings.AssemblyPath;

            if (_fileManager.TempCssFileExists())
            {
                await _fileManager.ClearTmpCssFileAsync();
            }
            else
            {
                await _fileManager.CreateTmpCssFileAsync();
            }

            try
            {
                
                // get files in project directory filtered by ".razor.css"
                foreach (FileInfo cssFile in new DirectoryInfo(Settings.ProjectDirectory).GetFiles(Settings.CssRazorSearchPattern, SearchOption.AllDirectories))
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
                    bundleInfo.CombinedFilesCount++;
                }
                BundleHelper.AddStylesToAssembly(Settings.AssemblyPath, File.ReadAllText(_fileManager.TempStylesFilePath));
                bundleInfo.Succeed = true;
            }
            catch (IOException ex)
            {
                bundleInfo.Errors.Add(new Error("IOException", ex.Message));
            }
            catch (InvalidCssSyntaxException ex)
            {
                bundleInfo.Errors.Add(new Error("CssSyntax", string.Format("invalid css syntax in file by path: {0};\nLine: {1}, Column: {2}", ex.FilePath, ex.Line, ex.Column)));
            }
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
    }
}
