using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Exceptions
{
    class SettingsAlreadyExistsException : Exception
    {
        /// <summary>
        /// Settings name
        /// </summary>
        public string SettingsName { get; set; }

        public SettingsAlreadyExistsException(string settingsName)
            : base()
        {
            SettingsName = settingsName;
        }
    }
}
