using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Commands.Parser
{
    public class Argument
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool HasValue => Value == null ? false : true;

        public Argument(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
