using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class SelectionItem
    {
        public string Name { get; set; }
        public bool Selected { get; set; }

        public SelectionItem(string name, bool selected = false)
        {
            Name = name;
            Selected = selected;
        }
    }
}
