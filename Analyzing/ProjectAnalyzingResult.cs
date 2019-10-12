using Blazor.CssBundler.Analyzing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Analyzing
{
    class ProjectAnalyzingResult
    {
        public PackageReference[] PackageReferences { get; set; }
        public Reference[] References { get; set; }
    }
}
