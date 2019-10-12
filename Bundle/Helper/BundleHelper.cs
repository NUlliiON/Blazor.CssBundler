using dnlib.DotNet;
using dnlib.DotNet.Resources;
using ExCSS;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Blazor.CssBundler.Bundle.Helper
{
    class BundleHelper
    {
        private const string ComponentStartId = "bz-component";
        private static string[] _strs = { "\\", "\\\\", "/", "//", "." };
        private static readonly string _classInSelectorPattern = "\\.[A-Za-z0-9-]+";

        /// <summary>
        /// Replacing all class selectors to new class selectors based in namespace and type
        /// </summary>
        /// <param name="csharpNamespace"></param>
        /// <param name="csharpType"></param>
        /// <param name="selectorText"></param>
        /// <returns></returns>
        public static string BuildCssClasses(string csharpNamespace, string csharpType, string selectorText)
        {
            Regex reg = new Regex(_classInSelectorPattern);
            MatchCollection matches = reg.Matches(selectorText); // match all classes in selector

            foreach (Match matchedClass in matches)
            {
                selectorText = selectorText.Replace(matchedClass.Value, BuildCssClass(csharpNamespace, csharpType, matchedClass.Value));
            }
            return selectorText;
        }

        /// <summary>
        /// Replace class selector to new class selector based in namespace and type
        /// </summary>
        /// <param name="csharpNamespace"></param>
        /// <param name="csharpType"></param>
        /// <param name="classSelector"></param>
        /// <returns></returns>
        private static string BuildCssClass(string csharpNamespace, string csharpType, string classSelector)
        {
            return "." + ComponentStartId + "-" + ReplaceString(csharpNamespace + "-" + csharpType + classSelector, _strs, "-");
        }

        public static void AppendStylesToFile(string filePath, Stylesheet stylesheet)
        {
            File.AppendAllText(filePath, Environment.NewLine + stylesheet.ToCss());
        }

        public static void AddStylesToAssembly(string assemblyPath, string stylesheet)
        {
            string determinerId = "__IsolatedStyleSheet";
            byte[] stylesheetData = Encoding.Default.GetBytes(stylesheet);

            var module = ModuleDefMD.Load(File.ReadAllBytes(assemblyPath));
            module.Resources.Add(new EmbeddedResource(determinerId, Encoding.Default.GetBytes(stylesheet)));
            module.Write(assemblyPath); // rewrite file
        }

        public static string GetStylesFromAssembly(string assemblyPath)
        {
            string determinedId = "__IsolatedStyleSheet";

            var module = ModuleDefMD.Load(File.ReadAllBytes(assemblyPath));
            if (module.HasResources)
            {
                foreach (Resource res in module.Resources)
                {
                    if (res.Name == determinedId)
                    {
                        string q = res.ToString();
                        return "";
                    }
                }
            }
            return null;
        }

        public static bool HasIsolatedCss(string assemblyPath)
        {
            string determinedId = "__IsolatedStyleSheet";

            var module = ModuleDefMD.Load(File.ReadAllBytes(assemblyPath));
            if (module.HasResources)
            {
                foreach (Resource res in module.Resources)
                {
                    if (res.Name == determinedId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Replace strings to new string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="strs"></param>
        /// <param name="newStr"></param>
        /// <returns></returns>
        private static string ReplaceString(string text, string[] strs, string newStr)
        {
            Array.ForEach(strs, str =>
            {
                text = text.Replace(str, newStr);
            });
            return text;
        }
    }
}
