using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazor.CssBundler.Bundle;
using Blazor.CssBundler.Bundle.App;
using Blazor.CssBundler.Bundle.Component;
using Blazor.CssBundler.Commands.Options;
using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Interactive;
using Blazor.CssBundler.Logging;
using Blazor.CssBundler.Models;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Settings;

namespace Blazor.CssBundler.Commands
{
    class WatchCommand : BaseCommand<WatchOptions>
    {
        private ILogger _logger;

        public override void Execute(ILogger logger, WatchOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override async Task ExecuteAsync(ILogger logger, WatchOptions options)
        {
            _logger = logger;
            BaseSettings settings = null;

            if (options.SettingsName == null)
            {
                var settingsList = await SettingsManager.GetAboutAllSettings().ToArrayAsync();
                if (settingsList.Length == 0)
                {
                    logger.Print("</crossmark/> No settings found");
                    return;
                }

                var selection = new VerticalSettingsSelector(settingsList.Select(x => new SettingsSelectionItem(x.name, x.type)).ToArray());
                SettingsSelectionItem selectedItem = selection.GetUserSelection();

                if (await SettingsManager.SettingsExists(selectedItem.Name))
                {
                    settings = await SettingsManager.ReadAsync<BaseSettings>(selectedItem.Name);
                }
            }
            else
            {
                if (await SettingsManager.SettingsExists(options.SettingsName))
                {
                    settings = await SettingsManager.ReadAsync<BaseSettings>(options.SettingsName);
                }
            }

            if (settings == null)
            {
                throw new SettingsNotFoundException(options.SettingsName);
            }
        
            if (settings.Type == SettingsType.Application)
            {
                await WatchApplication((ApplicationSettings)settings);
            }
            else if (settings.Type == SettingsType.Component)
            {
                await WatchComponent((ComponentSettings)settings);
            }
            else
            {
                logger.Print($"Settings contains type \"{options.SettingsName}\" which not supported");
                logger.Print("Available types:");
                foreach (string enumValue in Enum.GetValues(typeof(SettingsType)))
                {
                    logger.Print(" - " + enumValue);
                }
            }
        }

        private async Task WatchApplication(ApplicationSettings settings)
        {
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

        private async Task WatchComponent(ComponentSettings settings)
        {
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
