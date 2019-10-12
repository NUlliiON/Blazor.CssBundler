using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Blazor.CssBundler.Analyzing.Models
{
    class Reference
    {
        public string Name { get; set; }
        public string HintPath { get; set; }

        public Reference(XElement referenceEl)
        {
            Name = referenceEl.Attribute("Include")?.Value;
            HintPath = referenceEl.Element("HintPath")?.Value;
        }
    }
}
