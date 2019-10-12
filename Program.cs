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
        public static int RunChangeSettingsAndReturnExitCode(ChangeSettingsOptions options)
        {
            return 0;
        }
        public static int RunWatchAndReturnExitCode(WatchOptions options)
        {
            return 0;
        }
        public static int RunCloneAndReturnExitCode(ChangeSettingsOptions options)
        {
            return 0;
        }

        static async Task Main(string[] args)
        {            
            //ISettings settings = JsonConvert.DeserializeObject<ISettings>(File.ReadAllText(Path.Combine("settings", "BlazorApp.application.settings.json")));

            int q = Parser.Default.ParseArguments<WatchOptions, ChangeSettingsOptions>(args)
                .MapResult(
                  (ChangeSettingsOptions opts) => RunChangeSettingsAndReturnExitCode(opts),
                  (WatchOptions opts) => RunWatchAndReturnExitCode(opts),
                  errs => 1);

            

            if (args.Length < 1)
            {
                return;
            }
            try
            {
                //ExtendedLogger logger = new ExtendedLogger();
                //CommandRunner cmdRunner = new CommandRunner(logger);

                //ArgumentParser argParser = new ArgumentParser();
                //Argument[] argObjects = argParser.Parse(string.Join("", args)).ToArray();
                //var commands = new List<ICommand>()
                //{
                //    new ApplicationWatchCommand(),
                //    new ComponentWatchCommand()
                //};

                //foreach (ICommand cmd in commands)
                //{
                //    if (cmdRunner.OnlyPossibleArguments(argObjects, cmd))
                //    {
                //        await cmd.StartAsync(argObjects);
                //    }
                //}

                //string[] argNames = argObjects.Select(x => x.Name).ToArray();

                //if (argNames.Contains("watch"))
                //{

                //}


                //foreach (Argument arg in argObjects)
                //{
                //    if (arg.Name == "change-settings")
                //    {

                //        if (arg.HasFirstValue)
                //        {
                //            await cmdRunner.ChangeSettings();
                //        }
                //        else
                //        {

                //        }
                //    }
                //    else if (arg.Name == "settings-list")
                //    {
                //        cmdRunner.SettingsList();
                //    }
                //    else if (arg.Name == "watch-application")
                //    {
                //        if (arg.HasFirstValue && arg.HasSecondValue)
                //        {
                //            var settings = await SettingsManager.ReadAsync<ApplicationSettings>(arg.FirstValue, arg.SecondValue);
                //            await cmdRunner.AppWatch(settings);
                //        }
                //        else
                //        {
                //            cmdRunner.AppWatch();
                //        }
                //    }
                //    else
                //    {
                //        if (args[0] == "watch-app")
                //        {

                //        }
                //        else if (args[0] == "watch-component")
                //        {
                //            //ComponentSettings settings = await SettingsManager.ReadAsync<ComponentSettings>();
                //            //await cmdRunner.ComponentWatch(settings);
                //        }
                //        else if (args[0] == "build-app")
                //        {

                //        }
                //        else if (args[0] == "build-component")
                //        {

                //        }
                //    }
                //}
            }
            catch (InvalidSettingsException)
            {
                //ConsoleExtension.WriteError("Unable to load settings. Use arg --set-settings for set new settings!");
            }
        }
    }
}
