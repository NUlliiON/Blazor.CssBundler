using Blazor.CssBundler.Commands;
using Blazor.CssBundler.Commands.Options;
using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Extensions;
using Blazor.CssBundler.Logging;
using CommandLine;
using System;
using System.Threading.Tasks;

namespace Blazor.CssBundler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILogger logger = new ExtendedLogger();
            CommandExecuter cmd = new CommandExecuter(logger);

            var parser = new Parser(config => config.CaseInsensitiveEnumValues = true);
            await parser.ParseArguments<SettingsListOptions, CreateSettingsOptions, ChangeSettingsOptions, WatchOptions>(args)
                .MapResult(
                async (SettingsListOptions options) => await cmd.ExecuteAsync(new SettingsListCommand(), options),
                async (CreateSettingsOptions options) => await cmd.ExecuteAsync(new CreateSettingsCommand(), options),
                async (ChangeSettingsOptions options) => await cmd.ExecuteAsync(new ChangeSettingsCommand(), options),
                async (WatchOptions options) => await cmd.ExecuteAsync(new WatchCommand(), options),
                // TODO: async (BuildOptions options) => await cmd.ExecuteAsync(new BuildCommand(), options),
                async errs => 1);
        }
    }
}
