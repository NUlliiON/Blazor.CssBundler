using Blazor.CssBundler.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Interactive
{
    interface IInteractiveChanger<T>
    {
        T Change(T settings);
    }
}
