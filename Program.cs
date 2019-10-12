using Blazor.CssBundler.Commands.Options;
using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Settings;
using CommandLine;
using JsonSubTypes;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.CssBundler
{
    class Program
    {
        static async Task Main(string[] args)
        {            
            if (args.Length < 1)
            {
                return;
            }
            try
            {
                Parser.Default.ParseArguments<WatchOptions, ChangeSettingsOptions>(args)
                    .MapResult(
                    //(ChangeSettingsOptions opts) => RunChangeSettingsAndReturnExitCode(opts),
                    //(WatchOptions opts) => RunWatchAndReturnExitCode(opts),
                    errs => 1);
            }
            catch (InvalidSettingsException)
            {
                //ConsoleExtension.WriteError("Unable to load settings. Use arg --set-settings for set new settings!");
            }
        }
    }
}
