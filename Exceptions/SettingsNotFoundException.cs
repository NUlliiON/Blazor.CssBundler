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
        public string SettingsName { get; set; }

        public SettingsNotFoundException(string settingsName)
            : base()
        {
            SettingsName = settingsName;
        }
    }
}
