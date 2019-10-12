using Blazor.CssBundler.Analyzing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Blazor.CssBundler.Analyzing
{
    class ProjectAnalyzer : IAnalyzer<ProjectAnalyzingResult>
    {
        public ProjectAnalyzingResult Analyze(string projectPath)
        {
            try
            {
                XDocument doc = XDocument.Load(projectPath);

                return new ProjectAnalyzingResult()
                {
                    PackageReferences = ParsePackageReferences(doc).ToArray(),
                    References = ParseReferences(doc).ToArray()
                };
            }
            catch (XmlException)
            {
                return null;
            }
        }

        private List<PackageReference> ParsePackageReferences(XDocument doc)
        {
            var packageReferencesList = new List<PackageReference>();
            foreach (var itemGroup in doc.Descendants("ItemGroup"))
            {
                foreach (XElement packageReferenceEl in itemGroup.Descendants("PackageReference"))
                {
                    packageReferencesList.Add(new PackageReference(packageReferenceEl));
                }
            }
            return packageReferencesList;
        }

        private List<Reference> ParseReferences(XDocument doc)
        {
            var referencesList = new List<Reference>();
            foreach (var itemGroup in doc.Descendants("ItemGroup"))
            {
                foreach (XElement referenceEl in itemGroup.Descendants("Reference"))
                {
                    referencesList.Add(new Reference(referenceEl));
                }
            }
            return referencesList;
        }
    }
}
