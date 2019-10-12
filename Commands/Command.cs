using Blazor.CssBundler.Bundle.App;
using Blazor.CssBundler.Bundle.Component;
using Blazor.CssBundler.Commands.Options;
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

        public async Task ChangeSettings(ChangeSettingsOptions changeOptions)
        {
            Console.WriteLine("Select settings ");
            if (changeOptions.SettingsName == null)
            {
                var settingsList = await SettingsManager.GetAllSettingsInfo().ToArrayAsync();
                var selection = new VerticalSettingsSelector(settingsList.Select(x => new SettingsSelectionItem(x.name, x.type)).ToArray());
                SettingsSelectionItem item = selection.GetUserSelection();

                if (item.Type == SettingsType.Application)
                {
                    var settings = await SettingsManager.ReadAsync<ApplicationSettings>(item.Name);
                    var settingsChanger = new ApplicationSettingsChanger();
                    settingsChanger.Change(settings);
                }
                else if (item.Type == SettingsType.Component)
                {
                    var settings = await SettingsManager.ReadAsync<ComponentSettings>(item.Name);
                    var settingsChanger = new ComponentSettingsChanger();
                }
            }
        }

        public async Task SettingsList()
        {
            var settingsList = await SettingsManager.GetAllSettingsInfo().ToListAsync();
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

        public async Task AppWatch()
        {
            var settingsList = await SettingsManager.GetAllSettingsInfo().ToArrayAsync();
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