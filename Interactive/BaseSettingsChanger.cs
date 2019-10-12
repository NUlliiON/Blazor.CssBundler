using Blazor.CssBundler.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    abstract class BaseSettingsChanger<T> where T: BaseSettings
    {
        protected int StartLeftPosition;
        protected int StartTopPosition;
        protected int CurrentPositionInProperty;
        protected PropertySelectionItem[] PropertyItems;

        public BaseSettingsChanger()
        {
            StartLeftPosition = Console.CursorLeft;
            StartTopPosition = Console.CursorTop;
        }

        protected abstract PropertySelectionItem[] GetPropertySelectionItems(T settings);

        protected virtual void DrawProperties(params PropertySelectionItem[] propItems)
        {
            foreach (var propItem in propItems)
            {
                Console.SetCursorPosition(0, StartTopPosition + propItem.Position - 1);
                int longestPropName = PropertyItems.Max(x => x.Name.Length);

                int propNameLength = propItem.Name.Length;
                int residue = longestPropName - propNameLength;
                ClearCurrentConsoleLine();
                if (propItem.Selected)
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    string text = propItem.Name + new string(' ', residue) + " > ";
                    propItem.NameWithPaddingLength = text.Length;
                    Console.Write(text);
                    Console.ResetColor();
                    Console.Write(propItem.Value + "\n");
                }
                else
                {
                    Console.ResetColor();
                    Console.Write(propItem.Name + new string(' ', residue) + " > " + propItem.Value + "\n");
                }
                Console.SetCursorPosition(propItem.NameWithPaddingLength + CurrentPositionInProperty, StartTopPosition + propItem.Position - 1);
            }
        }

        public T Change(T settings)
        {
            PropertyItems = GetPropertySelectionItems(settings);
            DrawProperties(PropertyItems);

            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            while (cki != null && cki.Key != ConsoleKey.Enter)
            {
                cki = ReadKeyWithoutMoving();
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    CurrentPositionInProperty = 0;
                    for (int i = 0; i < PropertyItems.Length; i++)
                    {
                        if (i != 0)
                        {
                            if (PropertyItems[i].Selected)
                            {
                                PropertyItems[i].Selected = false; // unselect current item
                                PropertyItems[i - 1].Selected = true; // select up item
                                DrawProperties(PropertyItems[i]); // redraw unselected item
                                DrawProperties(PropertyItems[i - 1]); // redraw selected item
                                break;
                            }
                        }
                    }
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    CurrentPositionInProperty = 0;
                    for (int i = 0; i < PropertyItems.Length; i++)
                    {
                        if (i != PropertyItems.Length - 1)
                        {
                            if (PropertyItems[i].Selected)
                            {
                                PropertyItems[i].Selected = false; // unselect current item
                                PropertyItems[i + 1].Selected = true; // select down item
                                DrawProperties(PropertyItems[i]); // redraw unselected item
                                DrawProperties(PropertyItems[i + 1]); // redraw selected item
                                break;
                            }
                        }
                    }
                }
                else if (cki.Key == ConsoleKey.LeftArrow)
                {
                    if (CurrentPositionInProperty > 0)
                    {
                        PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
                        int stepCount = 5;
                        bool controlPressed = (cki.Modifiers & ConsoleModifiers.Control) != 0;

                        if (controlPressed)
                        {
                            if (CurrentPositionInProperty >= stepCount)
                            {
                                CurrentPositionInProperty -= stepCount;
                            }
                        }
                        else
                        {
                            CurrentPositionInProperty--;
                        }
                        DrawProperties(selectedPropItem);
                    }
                }
                else if (cki.Key == ConsoleKey.RightArrow)
                {
                    PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
                    if (CurrentPositionInProperty < selectedPropItem.Value.Length)
                    {
                        int stepCount = 5;
                        bool controlPressed = (cki.Modifiers & ConsoleModifiers.Control) != 0;

                        if (controlPressed)
                        {
                            if (CurrentPositionInProperty + stepCount <= selectedPropItem.Value.Length)
                            {
                                CurrentPositionInProperty += stepCount;
                            }
                        }
                        else
                        {
                            CurrentPositionInProperty++;
                        }
                        DrawProperties(selectedPropItem);
                    }
                }
                else if (cki.Key == ConsoleKey.Z && (cki.Modifiers & ConsoleModifiers.Control) != 0)
                {

                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    if (CurrentPositionInProperty > 0)
                    {
                        CurrentPositionInProperty--;
                        PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
                        selectedPropItem.Value = selectedPropItem.Value.Remove(CurrentPositionInProperty, 1);
                        DrawProperties(selectedPropItem);
                    }
                }
                else
                {
                    PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
                    selectedPropItem.Value = selectedPropItem.Value.Insert(CurrentPositionInProperty, cki.KeyChar.ToString());
                    CurrentPositionInProperty++;
                    DrawProperties(selectedPropItem);
                }
            }

            return Map(PropertyItems);
        }

        /// <summary>
        /// Property selection items to settings properties
        /// </summary>
        /// <returns>settings</returns>
        protected abstract T Map(PropertySelectionItem[] propertyItems);

        protected void ClearCurrentConsoleLine()
        {
            int curLeft = Console.CursorLeft;
            int curTop = Console.CursorTop;
            Console.SetCursorPosition(0, curTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(curLeft, curTop);
        }

        protected ConsoleKeyInfo ReadKeyWithoutMoving()
        {
            int curLeft = Console.CursorLeft;
            int curTop = Console.CursorTop;

            ConsoleKeyInfo cki = Console.ReadKey();
            Console.SetCursorPosition(curLeft, curTop);
            return cki;
        }
    }
}
