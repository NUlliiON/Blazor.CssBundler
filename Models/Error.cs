using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Models
{
    public struct Error
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public Error(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
