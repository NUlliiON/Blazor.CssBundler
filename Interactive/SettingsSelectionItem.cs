using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class SettingsSelectionItem : SelectionItem
    {
        public string Type { get; set; }

        public SettingsSelectionItem(string name, string type)
            : base(name)
        {
            Type = type;
        }
    }
}
