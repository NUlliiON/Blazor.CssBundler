using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Blazor.CssBundler.Analyzing.Models
{
    class PackageReference
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public PackageReference(XElement packageReferenceEl)
        {
            this.Name = packageReferenceEl.Attribute("Include").Value;
            this.Version = packageReferenceEl.Attribute("Version").Value;
        }
    }
}
