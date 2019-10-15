using Blazor.CssBundler.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    abstract class BaseCommand<TOptions>
    {
        public abstract void Execute(ILogger logger, TOptions options);
        public abstract Task ExecuteAsync(ILogger logger, TOptions options);
    }
}
