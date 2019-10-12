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
        private string[] _offNetNamespaces = { "System", "Microsoft", "netstandard" };

        public AssemblyAnalyzingResult Analyze(string assemblyPath)
        {
            var asmPathsList = new List<string>();
            string[] refsPaths = GetLocalReferences(assemblyPath).ToArray();
            return new AssemblyAnalyzingResult() { AssemblyPaths = refsPaths };
        }

        /// <summary>
        /// Get local referenses
        /// </summary>
        /// <param name="assemblyPath">full assembly path</param>
        /// <returns></returns>
        private List<string> GetLocalReferences(string assemblyPath)
        {
            return GetReferencesRecursively(assemblyPath, _offNetNamespaces);
        }

        private List<string> GetReferencesRecursively(string assemblyPath, string[] ignoredNamespaces)
        {
            var asmPathsList = new List<string>();
            string asmDir = Path.GetDirectoryName(assemblyPath);
            var asm = Assembly.LoadFile(assemblyPath);
            var refs = asm.GetReferencedAssemblies();
            foreach (var @ref in refs)
            {
                if (!ignoredNamespaces.Contains(@ref.Name.Split('.')[0]))
                {
                    string refAsmPath = Path.Combine(asmDir, $"{@ref.Name}.dll");
                    if (File.Exists(refAsmPath))
                    {
                        asmPathsList.Add(refAsmPath);
                        asmPathsList.AddRange(GetReferencesRecursively(refAsmPath, ignoredNamespaces));
                    }
                }
            }
            return asmPathsList;
        }
    }
}