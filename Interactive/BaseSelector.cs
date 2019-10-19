using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    abstract class BaseSelector<T> where T: SelectionItem
    {
        protected int startLeftPos;
        protected int startTopPos;
        protected T[] selectionItems;

        protected ConsoleKey NextItemKey;
        protected ConsoleKey PrevItemKey;
        protected bool CursorVisible;

        public BaseSelector(T[] selectionItems)
        {
            startLeftPos = Console.CursorLeft;
            startTopPos = Console.CursorTop;
            this.selectionItems = selectionItems;
        }

        protected abstract void DrawSelections();

        public virtual T GetUserSelection()
        {
            DrawSelections();

            bool selected = false;
            while (!selected)
            {
                ConsoleKeyInfo pressedKey = InteractiveHelper.ReadKeyWithoutMoving();
                bool selectionChanged = false;
                if (pressedKey.Key == ConsoleKey.Enter)
                {
                    selected = true;
                }
                if (pressedKey.Key == PrevItemKey)
                {
                    for (int i = 0; i < selectionItems.Length; i++)
                    {
                        if (i != 0)
                        {
                            if (selectionItems[i].Selected)
                            {
                                selectionItems[i].Selected = false; // unselect right item
                                selectionItems[i - 1].Selected = true; // select left item
                                selectionChanged = true;
                                break;
                            }
                        }
                    }
                }
                else if (pressedKey.Key == NextItemKey)
                {
                    for (int i = 0; i < selectionItems.Length; i++)
                    {
                        if (i != selectionItems.Length - 1)
                        {
                            if (selectionItems[i].Selected)
                            {
                                selectionItems[i].Selected = false; // unselect left item
                                selectionItems[i + 1].Selected = true; // select right item
                                selectionChanged = true;
                                break;
                            }
                        }
                    }
                }
                if (selectionChanged)
                {
                    DrawSelections();
                }
            }
            return selectionItems.Where(x => x.Selected).FirstOrDefault();
        }
    }
}
