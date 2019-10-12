using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Bundle.Helper
{
    class NugetHelper
    {
        /// <summary>
        /// Making full assembly path by assembly name, version and netstandart or return null
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="assemblyVersion"></param>
        /// <param name="netStandard"></param>
        /// <returns>full assembly path</returns>
        public static string MakeAssemblyPath(string assemblyName, string assemblyVersion, string netStandard)
        {
            return @$"C:\Users\{Environment.UserName}\.nuget\packages\{assemblyName.ToLower()}\{assemblyVersion}\lib\{netStandard}\{assemblyName}.dll";
        }
    }
}
