using Blazor.CssBundler.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    abstract class BaseCommand<T>
    {
        public abstract void Execute(ILogger logger, T options);
        public abstract Task ExecuteAsync(ILogger logger, T options);
    }
}
