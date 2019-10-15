using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Commands
{
    public enum SettingsDisplayType
    {
        /// <summary>
        /// Display all settings
        /// </summary>
        All = 0,
        /// <summary>
        /// Display only application type
        /// </summary>
        Application,
        /// <summary>
        /// Display only component type
        /// </summary>
        Component,
    }
}
