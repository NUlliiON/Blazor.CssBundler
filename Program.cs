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
        static int Sum(int a, int b) => a + b;

        static async Task Main(string[] args)
        {
            ILogger logger = new ExtendedLogger();
            CommandExecuter cmd = new CommandExecuter(logger);

            Parser.Default.ParseArguments<ChangeSettingsOptions>(args)
                .MapResult(
                (ChangeSettingsOptions opts) => cmd.Execute(new ChangeSettingsCommand(), opts),
                errs => 1);
        }
    }
}
