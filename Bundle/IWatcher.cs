using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Bundle
{
    interface IWatcher
    {
        Task StartWatchingAsync();
        void StopWatching();
    }
}
