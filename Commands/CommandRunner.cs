using Blazor.CssBundler.Bundle.App;
using Blazor.CssBundler.Bundle.Component;
using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Interactive;
using Blazor.CssBundler.Logging;
using Blazor.CssBundler.Models;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Settings;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    class CommandExecuter
    {
        private ILogger _logger;

        private delegate void ExecuteCommand();

        public CommandExecuter(ILogger logger)
        {
            _logger = logger;
        }

        public int Execute<T, K>(T command, K options) where T : BaseCommand<K>
        {
            try
            {
                command.Execute(_logger, options);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public async Task<int> ExecuteAsync<T, K>(T command, K options) where T : BaseCommand<K>
        {
            try
            {
                await command.ExecuteAsync(_logger, options);
                return 0;
            }
            catch
            {
                return 1;
            }
        }
    }
}