using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class SettingsPropertyAttribute : Attribute
    {
    }
}
