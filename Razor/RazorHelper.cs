using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Razor
{
    class RazorHelper
    {
        public enum RazorDirectory
        {
            Root,
            Razor,
            RazorDeclaration,
            StaticWebAssets
        }

        public enum RazorFileExtension
        {
            RazorGenCsharp,
            GenCsharp,
            Txt,
            Cache,
            Csharp,
            Pdb,
            Exe,
            Dll,
            Xml
        }

        public static string GetPathByEnum(RazorDirectory razorDir) => razorDir switch
        {
            RazorDirectory.Root => @"obj\Debug\netstandard2.0\",
            RazorDirectory.Razor => @"obj\Debug\netstandard2.0\Razor",
            RazorDirectory.StaticWebAssets => @"obj\Debug\netstandard2.0\staticwebassets",
            RazorDirectory.RazorDeclaration => @"obj\Debug\netstandard2.0\RazorDeclaration",
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(razorDir)),
        };

        public static string GetExtensionByEnum(RazorFileExtension razorExt) => razorExt switch
        {
            RazorFileExtension.RazorGenCsharp => "razor.g.cs",
            RazorFileExtension.GenCsharp => "g.cs",
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(razorExt)),
        };
    }
}
