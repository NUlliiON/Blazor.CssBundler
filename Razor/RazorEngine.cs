using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using static Blazor.CssBundler.Razor.RazorHelper;

namespace Blazor.CssBundler.Razor
{
    class RazorEngine
    {
        private Dictionary<string, string> _projectPaths;

        public RazorEngine()
        {
            _projectPaths = new Dictionary<string, string>();
        }

        private string GetProjectPath(string projName)
        {
            if (projName == null)
            {
                throw new ArgumentNullException(projName);
            }

            if (_projectPaths.ContainsKey(projName))
                return _projectPaths[projName];

            return null;
        }

        public void AddProjectDirectory(string projName, string projDir)
        {
            if (projName == null)
            {
                throw new ArgumentNullException("projName");
            }
            if (projDir == null)
            {
                throw new ArgumentNullException("projDir");
            }

            if (_projectPaths.ContainsKey(projName))
                return;

            _projectPaths.Add(projName, projDir);
        }

        public string GetNamespaceFromFile(string projName, string relativeFilePath, RazorDirectory razorDir, RazorFileExtension razorExt)
        {
            string projPath = GetProjectPath(projName);
            string razorDirPath = GetPathByEnum(razorDir);
            string fullFilePath = Path.Combine(projPath, razorDirPath, relativeFilePath);
            fullFilePath = fullFilePath + "." + GetExtensionByEnum(razorExt);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(fullFilePath));
            var root = tree.GetCompilationUnitRoot();
            var @namespace = (NamespaceDeclarationSyntax)root.Members[0];

            return @namespace.Name.ToString();
        }

        /// <summary>
        /// Finding file in directory and return file if found or null if not
        /// </summary>
        /// <param name="projName">project path</param>
        /// <param name="relativeFilePath">relative file path</param>
        /// <param name="razorDir">razor directory</param>
        /// <param name="razorExt">razor file extension</param>
        /// <returns>Found file path</returns>
        public RazorFile FindFile(string projName, string relativeFilePath, RazorDirectory razorDir, RazorFileExtension razorExt)
        {
            string projPath = GetProjectPath(projName);
            string razorDirPath = GetPathByEnum(razorDir);
            string fullFilePath = Path.Combine(projPath, razorDirPath, relativeFilePath);
            string extension = GetExtensionByEnum(razorExt);
            fullFilePath = fullFilePath + "." + extension;

            if (File.Exists(fullFilePath))
            {
                return new RazorFile()
                {
                    SystemFile = new FileInfo(fullFilePath),
                    FileLocation = razorDir,
                    RazorExtension = razorExt,
                    FullExtension = extension
                };
            }
            return null;
        }

        ///// <summary>
        ///// Finding file in directory and return file if found or null if not
        ///// </summary>
        ///// <param name="projName">project path</param>
        ///// <param name="relativeFilePath">relative file path</param>
        ///// <param name="razorDir">razor directory</param>
        ///// <returns>Found file path</returns>
        //public RazorFile FindFile(string projName, string relativeFilePath, RazorDirectory razorDir)
        //{
        //    string projPath = GetProjectPath(projName);
        //    string razorDirPath = GetPathByEnum(razorDir);
        //    string fullFilePath = Path.Combine(projPath, razorDirPath, relativeFilePath);
        //    string extension = null;
        //    if (razorExt != RazorFileExtension.Empty)
        //    {
        //        fullFilePath = fullFilePath + "." + GetExtensionByEnum(razorExt);
        //    }

        //    if (File.Exists(fullFilePath))
        //    {
        //        RazorFile razorFile = new RazorFile()
        //        {
        //            SystemFile = new FileInfo(fullFilePath),
        //            FileLocation = razorDir,
        //        };
        //        if
        //    }
        //    return null;
        //}
    }
}
