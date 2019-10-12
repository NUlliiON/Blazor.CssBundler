using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Models
{
    public class ValidateResult
    {
        public List<Error> Errors { get; set; }
        public bool Succeed { get; set; }

        public ValidateResult()
        {
            Errors = new List<Error>();
        }
    }
}
