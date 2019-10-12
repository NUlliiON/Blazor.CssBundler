using Blazor.CssBundler.Bundle.App;
using Blazor.CssBundler.Bundle.Component;
using Blazor.CssBundler.Commands.Parser;
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
    class Command
    {
        private ExtendedLogger _logger;

        public Command(ExtendedLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Checking array args contains only possible arguments defined in command
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool OnlyPossibleArguments(Argument[] args, ICommand cmd)
        {
            int argsCount = args.Length;
            if (args.Select(x => x.Name).Intersect(cmd.PossibleArguments).Count() == argsCount)
            {
                return true;
            }
            return false;
        }

        public async Task ChangeSettings()
        {
            Console.WriteLine("Select settings ");
            var settingsList = SettingsManager.AllSettingsInfo().ToArray();
            var selection = new VerticalSettingsSelector(settingsList.Select(x => new SettingsSelectionItem(x.name, x.type)).ToArray());
            selection.GetUserSelection();
            SettingsSelectionItem item = selection.GetUserSelection();
            if (item.Name == "Application")
            {
                var settings = await SettingsManager.ReadAsync<ApplicationSettings>(item.Name);
                var settingsChanger = new ApplicationSettingsChanger();
                settingsChanger.Change(settings);
            }
            else if (item.Name == "Component")
            {
                var settings = await SettingsManager.ReadAsync<ComponentSettings>(item.Name);
                var settingsChanger = new ComponentSettingsChanger();
            }


            //Console.WriteLine("Select settings -> ");
            //Console.Write("Application");
            //_projSettings = new AppSettings();

            //bool validSettings = false;
            //while (!validSettings)
            //{
            //    Console.Write("Path to project directory: ");
            //    _projSettings.ProjectDirectory = Console.ReadLine();

            //    Console.Write("Path to global css styles file: ");
            //    _projSettings.GlobalStylesPath = Console.ReadLine();

            //    Console.Write("Path to the directory for the Buildd file: ");
            //    _projSettings.OutputCssDirectory = Console.ReadLine();

            //    var results = SettingsValidator.Validate(_projSettings);
            //    if (results.Count == 0)
            //    {
            //        validSettings = true;
            //    }
            //    else
            //    {
            //        foreach (var error in results)
            //        {
            //            ConsoleExtension.WriteError(error.ErrorMessage);
            //        }
            //        Console.WriteLine();
            //    }
            //}
            //await SettingsManager.SaveAsync(_projSettings);
            //Console.WriteLine("File successfully saved");
        }

        //static async Task RunBuild(/*ManualCssBundler builder*/)
        //{
        //    throw new NotImplementedException();
        //    //ConsoleExtension.WriteInfo("Building...");

        //    //_ = Console.ReadLine();
        //}

        public void SettingsList()
        {
            var settingsList = SettingsManager.AllSettingsInfo().ToList();
            for (int i = 0; i < settingsList.Count; i++)
            {
                _logger.Print("(" + (i + 1) + ") " + settingsList[i].name + " - " + settingsList[i].type);
            }
        }

        public async Task CreateSettings(string name, string type)
        {
            if (SettingsManager.Has(name))
            {
                _logger.Print("</warn/> Settings with this name or type already exists");
                return;
            }

            if (type.ToLower() == "application")
            {
                await SettingsManager.SaveAsync(new ApplicationSettings(), name);
            }
            else if (type.ToLower() == "component")
            {
                await SettingsManager.SaveAsync(new ComponentSettings(), name);
            }
            else
            {
                throw new Exception("Settings type not found");
            }
            _logger.Print($"Settings with name \"{name}\" and type \"{type}\" </checkmark/> created </grinning_face/>");
        }

        public void AppWatch()
        {
            var settingsList = SettingsManager.AllSettingsInfo().ToArray();
            var selection = new VerticalSettingsSelector(settingsList.Select(x => new SettingsSelectionItem(x.name, x.type)).ToArray());
            SettingsSelectionItem selectedItem = selection.GetUserSelection();
        }

        public async Task AppWatch(ApplicationSettings settings)
        {
            if (settings == null)
            {
                throw new InvalidSettingsException();
            }

            var bundler = new AutoApplicationBundler(settings);

            bundler.BundlingStart += () =>
            {
                _logger.Print("</package/> Bundling...");
            };
            bundler.BundlingEnd += (bInfo) =>
            {
                _logger.Print("CSS </checkmark/> bundled");
                _logger.Print("</filefolder/> Output file path: " + bInfo.OutputFilePath);
                _logger.Print("</page_facing_up/> Combined files count: " + bInfo.CombinedFilesCount);
                _logger.Print("</clock/> Bundling time: " + bInfo.BundlingTime);
                Console.WriteLine();
            };
            bundler.BundlingError += (bInfo) =>
            {
                _logger.Print("CSS </crossmark/> builded");
                _logger.Print("</page_facing_up/> Combined files count: " + bInfo.CombinedFilesCount);
                _logger.Print("</clock/> Bundling time: " + bInfo.BundlingTime);
                foreach (Error err in bInfo.Errors)
                {
                    _logger.PrintError(err.Name + ": " + err.Message);
                }
                Console.WriteLine();
            };

            _logger.Print("Watching...");
            await bundler.StartWatchingAsync();

            await Task.Delay(Timeout.Infinite); // Infinity because we need infinity watching
        }

        public async Task ComponentWatch(ComponentSettings settings)
        {
            if (settings == null)
            {
                throw new InvalidSettingsException();
            }

            var bundler = new AutoComponentBundler(settings);

            bundler.BundlingStart += () =>
            {
                _logger.Print("</package/> Bundling...");
            };
            bundler.BundlingEnd += (bInfo) =>
            {
                _logger.Print("CSS </checkmark/> bundled");
                _logger.Print("</filefolder/> Assembly path: " + bInfo.PathToBundledAssembly);
                _logger.Print("</page_facing_up/> Combined files count: " + bInfo.CombinedFilesCount);
                _logger.Print("</clock/> Bundling time: " + bInfo.BundlingTime);
                Console.WriteLine();
            };
            bundler.BundlingError += (bInfo) =>
            {
                _logger.Print("CSS </crossmark/> builded");
                _logger.Print("</page_facing_up/> Combined files count: " + bInfo.CombinedFilesCount);
                _logger.Print("</clock/> Bundling time: " + bInfo.BundlingTime);
                foreach (Error err in bInfo.Errors)
                {
                    _logger.PrintError(err.Name + ": " + err.Message);
                }
                Console.WriteLine();
            };

            _logger.Print("Watching...");
            await bundler.StartWatchingAsync();

            await Task.Delay(Timeout.Infinite); // Infinity because we need infinity watching
        }
    }
}