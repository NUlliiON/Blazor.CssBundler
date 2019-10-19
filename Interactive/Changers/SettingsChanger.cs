using Blazor.CssBundler.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Blazor.CssBundler.Models;

namespace Blazor.CssBundler.Interactive.Changers
{
    class SettingsChanger<T> where T : BaseSettings, new()
    {
        protected int StartLeftPosition;
        protected int StartTopPosition;
        protected int CurrentPositionInProperty;
        protected PropertySelectionItem[] PropertyItems;

        public SettingsChanger()
        {
            StartLeftPosition = Console.CursorLeft;
            StartTopPosition = Console.CursorTop;
        }

        protected virtual PropertySelectionItem[] GetPropertySelectionItems(T settings)
        {
            var propsSelectionItem = new List<PropertySelectionItem>();
            PropertyInfo[] props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                var attr = prop.GetCustomAttribute<SettingsPropertyAttribute>(true);
                if (attr != null)
                {
                    if (propsSelectionItem.Count == 0)
                    {
                        propsSelectionItem.Add(new PropertySelectionItem(prop.Name, prop.GetValue(settings).ToString(), i, true));
                    }
                    else
                    {
                        propsSelectionItem.Add(new PropertySelectionItem(prop.Name, prop.GetValue(settings).ToString(), i));
                    }

                }
            }
            return propsSelectionItem.ToArray();
        }

        /// <summary>
        /// Property selection items to settings properties
        /// </summary>
        /// <returns>settings</returns>
        protected T Map(PropertySelectionItem[] propertyItems)
        {
            T settings = new T();
            PropertyInfo[] props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                var attr = prop.GetCustomAttribute<SettingsPropertyAttribute>(true);
                if (attr != null)
                {
                    prop.SetValue(settings, propertyItems.Where(x => x.Name == prop.Name).FirstOrDefault().Value);
                }
            }
            return settings;
        }

        protected virtual void DrawProperties(params PropertySelectionItem[] propItems)
        {
            foreach (var propItem in propItems)
            {
                Console.SetCursorPosition(0, StartTopPosition + propItem.LinePosition - 1);
                int longestPropName = PropertyItems.Max(x => x.Name.Length);

                int propNameLength = propItem.Name.Length;
                int residue = longestPropName - propNameLength;
                InteractiveHelper.ClearCurrentConsoleLine();
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
                Console.SetCursorPosition(propItem.NameWithPaddingLength + CurrentPositionInProperty, StartTopPosition + propItem.LinePosition - 1);
            }
        }

        public T Change(T settings)
        {
            PropertyItems = GetPropertySelectionItems(settings);
            DrawProperties(PropertyItems);

            bool changingFinished = false;
            while (!changingFinished)
            {
                ConsoleKeyInfo pressedKey = InteractiveHelper.ReadKeyWithoutMoving();
                bool altPressed = (pressedKey.Modifiers & ConsoleModifiers.Alt) != 0;
                bool controlPressed = (pressedKey.Modifiers & ConsoleModifiers.Control) != 0;
                bool shiftPressed = (pressedKey.Modifiers & ConsoleModifiers.Shift) != 0;

                if (pressedKey.Key == ConsoleKey.Enter)
                {
                    changingFinished = true;
                }
                else if (pressedKey.Key == ConsoleKey.UpArrow)
                {
                    MoveUp();
                }
                else if (pressedKey.Key == ConsoleKey.DownArrow)
                {
                    MoveDown();
                }
                else if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    MoveLeft(controlPressed);
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    MoveRight(controlPressed);
                }
                else if (pressedKey.Key == ConsoleKey.Z && controlPressed)
                {

                }
                else if (pressedKey.Key == ConsoleKey.Backspace)
                {
                    EraseChar();
                }
                else
                {
                    WriteChar(pressedKey.KeyChar);
                }
            }

            return Map(PropertyItems);
        }

        //private bool PressedModifier(ConsoleModifiers modifier) => pressedKey.Modifiers & ConsoleModifiers.Control != 0;

        private void MoveLeft(bool controlPressed)
        {
            if (CurrentPositionInProperty > 0)
            {
                PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
                int stepCount = 5;

                if (controlPressed)
                {
                    if (CurrentPositionInProperty >= stepCount)
                    {
                        CurrentPositionInProperty -= stepCount;
                    }
                    else
                    {
                        CurrentPositionInProperty -= CurrentPositionInProperty;
                    }
                }
                else
                {
                    CurrentPositionInProperty--;
                }
                DrawProperties(selectedPropItem);
            }
        }

        private void MoveRight(bool controlPressed)
        {
            PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
            if (CurrentPositionInProperty < selectedPropItem.Value.Length)
            {
                int stepCount = 5;

                if (controlPressed)
                {
                    int maxStepCount = selectedPropItem.Value.Length - CurrentPositionInProperty;
                    if (CurrentPositionInProperty + stepCount <= selectedPropItem.Value.Length)
                    {
                        CurrentPositionInProperty += stepCount;
                    }
                    else
                    {
                        CurrentPositionInProperty += maxStepCount;
                    }
                }
                else
                {
                    CurrentPositionInProperty++;
                }
                DrawProperties(selectedPropItem);
            }
        }

        private void MoveUp()
        {
            for (int i = 0; i < PropertyItems.Length; i++)
            {
                if (i != 0)
                {
                    if (PropertyItems[i].Selected)
                    {
                        PropertyItems[i].Selected = false; // unselect current item
                        PropertyItems[i].CaretPosition = CurrentPositionInProperty; // saving current caret position

                        PropertyItems[i - 1].Selected = true; // select up item
                        CurrentPositionInProperty = PropertyItems[i - 1].CaretPosition; // get caret position

                        DrawProperties(PropertyItems[i]); // redraw unselected item
                        DrawProperties(PropertyItems[i - 1]); // redraw selected item
                        break;
                    }
                }
            }
        }

        private void MoveDown()
        {
            for (int i = 0; i < PropertyItems.Length; i++)
            {
                if (i != PropertyItems.Length - 1)
                {
                    if (PropertyItems[i].Selected)
                    {
                        PropertyItems[i].Selected = false; // unselect current item
                        PropertyItems[i].CaretPosition = CurrentPositionInProperty; // saving current caret position

                        PropertyItems[i + 1].Selected = true; // select down item
                        CurrentPositionInProperty = PropertyItems[i + 1].CaretPosition; // get caret position

                        DrawProperties(PropertyItems[i]); // redraw unselected item
                        DrawProperties(PropertyItems[i + 1]); // redraw selected item
                        break;
                    }
                }
            }
        }

        private void WriteChar(char chr)
        {
            PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
            selectedPropItem.Value = selectedPropItem.Value.Insert(CurrentPositionInProperty, chr.ToString());
            CurrentPositionInProperty++;
            DrawProperties(selectedPropItem);
        }

        private void EraseChar()
        {
            if (CurrentPositionInProperty > 0)
            {
                CurrentPositionInProperty--;
                PropertySelectionItem selectedPropItem = PropertyItems.Where(x => x.Selected).FirstOrDefault();
                selectedPropItem.Value = selectedPropItem.Value.Remove(CurrentPositionInProperty, 1);
                DrawProperties(selectedPropItem);
            }
        }

        private void CopyText()
        {

        }

        private void PasteText()
        {

        }

        private void UndoText()
        {

        }
    }
}
