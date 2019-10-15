using Blazor.CssBundler.Models.Settings;
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

        public SettingsNotFoundException(string name)
            : base()
        {
            Name = name;
        }
    }
}
