using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    class InteractiveHelper
    {
        public static ConsoleKeyInfo ReadKeyWithoutMoving()
        {
            int curLeft = Console.CursorLeft;
            int curTop = Console.CursorTop;

            ConsoleKeyInfo cki = Console.ReadKey();
            Console.SetCursorPosition(curLeft, curTop);
            return cki;
        }

        public static void ClearCurrentConsoleLine()
        {
            int curLeft = Console.CursorLeft;
            int curTop = Console.CursorTop;

            Console.SetCursorPosition(0, curTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(curLeft, curTop);
        }
    }
}
