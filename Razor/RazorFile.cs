using System.IO;
using static Blazor.CssBundler.Razor.RazorHelper;

namespace Blazor.CssBundler.Razor
{
    class RazorFile
    {
        public string FullExtension { get; set; }
        public RazorFileExtension RazorExtension { get; set; }
        public RazorDirectory FileLocation { get; set; }
        public FileInfo SystemFile { get; set; }
    }
}
