using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Logging
{
    interface ILogger
    {
        void Print(string text);
        void PrintError(string text);
        void PrintSuccess(string text);
        void PrintWarn(string text);
    }
}
