using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blazor.CssBundler.Analyzing
{
    class AssemblyAnalyzer : IAnalyzer<AssemblyAnalyzingResult>
    {
        private string[] _offDotNetNamespaces = { "System", "Microsoft", "netstandard" };

        public AssemblyAnalyzingResult Analyze(string assemblyPath)
        {
            var assemblyPathList = new List<string>();
            string[] referencePaths = GetLocalReferences(assemblyPath).ToArray();
            return new AssemblyAnalyzingResult() { AssemblyPaths = referencePaths };
        }

        /// <summary>
        /// Get local referenses
        /// </summary>
        /// <param name="assemblyPath">full assembly path</param>
        /// <returns></returns>
        private List<string> GetLocalReferences(string assemblyPath)
        {
            return GetReferencesRecursively(assemblyPath, _offDotNetNamespaces);
        }

        private List<string> GetReferencesRecursively(string assemblyPath, string[] ignoredNamespaces)
        {
            var assembly = Assembly.LoadFile(assemblyPath);
            string assemblyDir = Path.GetDirectoryName(assemblyPath);
            var assemblyPathList = new List<string>();

            AssemblyName[] references = assembly.GetReferencedAssemblies();
            foreach (var reference in references)
            {
                if (!ignoredNamespaces.Contains(reference.Name.Split('.')[0]))
                {
                    string referenceAssemblyPath = Path.Combine(assemblyDir, $"{reference.Name}.dll");
                    if (File.Exists(referenceAssemblyPath))
                    {
                        assemblyPathList.Add(referenceAssemblyPath);
                        assemblyPathList.AddRange(GetReferencesRecursively(referenceAssemblyPath, ignoredNamespaces));
                    }
                }
            }
            return assemblyPathList;
        }
    }
}