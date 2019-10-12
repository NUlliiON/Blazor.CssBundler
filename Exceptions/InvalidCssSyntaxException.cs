using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Exceptions
{
    class InvalidCssSyntaxException : Exception
    {
        public string FilePath { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public InvalidCssSyntaxException(string filePath, int line, int column)
            : base(message: "invalid css syntax")
        {
            FilePath = filePath;
            Line = line;
            Column = column;
        }
    }
}
