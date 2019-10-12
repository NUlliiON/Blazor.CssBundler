using Blazor.CssBundler.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class SettingsSelectionItem : SelectionItem
    {
        public SettingsType Type { get; set; }

        public SettingsSelectionItem(string name, SettingsType type)
            : base(name)
        {
            Type = type;
        }
    }
}
