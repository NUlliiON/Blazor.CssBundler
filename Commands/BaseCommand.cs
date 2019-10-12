using Blazor.CssBundler.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    abstract class BaseCommand<T>
    {
        protected ILogger Logger;

        public BaseCommand(ILogger logger)
        {
            Logger = logger;
        }

        public abstract void Execute(T options);
        public abstract Task ExecuteAsync(T options);
    }
}
