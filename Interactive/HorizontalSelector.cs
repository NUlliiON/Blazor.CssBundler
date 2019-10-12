using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class HorizontalSelector<T> : BaseSelector<T> 
        where T: SettingsSelectionItem
    {
        public HorizontalSelector(T[] selectionItems)
            : base(selectionItems)
        {
            selectionItems[0].Selected = true;
            NextItemKey = ConsoleKey.RightArrow;
            PrevItemKey = ConsoleKey.LeftArrow;
        }

        protected override void DrawSelections()
        {
            Console.SetCursorPosition(startLeftPos, startTopPos);
            for (int i = 0; i < selectionItems.Length; i++)
            {
                T item = selectionItems[i];
                if (item.Selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write(" " + item.Name + " ");
                    Console.ResetColor();
                    Console.Write(" ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write(" " + item.Name + " ");
                    Console.ResetColor();
                    Console.Write(" ");
                }
            }
        }
    }
}
