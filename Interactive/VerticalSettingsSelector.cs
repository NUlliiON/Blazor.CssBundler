using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class VerticalSettingsSelector : BaseSelector<SettingsSelectionItem>
    {
        public VerticalSettingsSelector(SettingsSelectionItem[] selectionItems)
            : base(selectionItems)
        {
            selectionItems[0].Selected = true;
            NextItemKey = ConsoleKey.DownArrow;
            PrevItemKey = ConsoleKey.UpArrow;
        }

        protected override void DrawSelections()
        {
            Console.SetCursorPosition(startLeftPos, startTopPos);
            for (int i = 0; i < selectionItems.Length; i++)
            {
                SettingsSelectionItem item = selectionItems[i];
                string itemValue = selectionItems[i].Type;
                if (selectionItems[i].Selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine(" " + item.Name + " - " + item.Type + " ");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(" " + item.Name + " - " + item.Type + " ");
                    Console.ResetColor();
                }
            }
        }
    }
}
