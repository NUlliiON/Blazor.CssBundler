using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Exceptions
{
    class InvalidSettingsException : Exception
    {
        public InvalidSettingsException()
            : base("invalid settings")
        {

        }
    }
}
