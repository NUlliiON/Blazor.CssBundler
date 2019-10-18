using System;
using System.Collections.Generic;
using System.Text;
using Blazor.CssBundler.Models;

namespace Blazor.CssBundler.Interactive
{
    class PropertySelectionItem : SelectionItem
    {
        public string Value { get; set; }
        public int Position { get; set; }
        public Error[] Errors { get; set; }
        public int NameWithPaddingLength { get; set; }

        public PropertySelectionItem(string name, string value, int position, bool selected = false)
            : base(name, selected)
        {
            Value = value;
            Position = position;
        }
    }
}
