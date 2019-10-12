using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Exceptions
{
    class SettingsNotFoundException : Exception
    {
        /// <summary>
        /// Settings name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Settings type
        /// </summary>
        public string Type { get; set; }

        public SettingsNotFoundException(string name, string type)
            : base()
        {
            Name = name;
            Type = type;
        }
    }
}
