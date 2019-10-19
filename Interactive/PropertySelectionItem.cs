using System;
using System.Collections.Generic;
using System.Text;
using Blazor.CssBundler.Models;

namespace Blazor.CssBundler.Interactive
{
    class PropertySelectionItem : SelectionItem
    {
        public string Value { get; set; }
        public int LinePosition { get; set; }
        public int CaretPosition { get; set; }
        public Error[] Errors { get; set; }
        public string OldValue { get; set; }
        public int NameWithPaddingLength { get; set; }

        public PropertySelectionItem(string name, string value, int linePosition, bool selected = false)
            : base(name, selected)
        {
            Value = value;
            LinePosition = linePosition;
            CaretPosition = 0;
        }
    }
}
