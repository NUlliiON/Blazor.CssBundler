using Blazor.CssBundler.Commands.Options;
using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Interactive;
using Blazor.CssBundler.Logging;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    class ChangeSettingsCommand : BaseCommand<ChangeSettingsOptions>
    {
        public override void Execute(ILogger logger, ChangeSettingsOptions options)
        {
            throw new NotImplementedException();
        }

        public override async Task ExecuteAsync(ILogger logger, ChangeSettingsOptions options)
        {
            Console.WriteLine("Select settings ");
            BaseSettings settings = null;
            if (options.SettingsName == null)
            {
                var settingsList = await SettingsManager.GetAboutAllSettings().ToArrayAsync();
                var selection = new VerticalSettingsSelector(settingsList.Select(x => new SettingsSelectionItem(x.name, x.type)).ToArray());
                SettingsSelectionItem item = selection.GetUserSelection();

                if (SettingsManager.SettingsExists(item.Name))
                {
                    settings = await SettingsManager.ReadSettingsAsync<BaseSettings>(item.Name);
                }
            }
            else
            {
                if (SettingsManager.SettingsExists(options.SettingsName))
                {
                    settings = await SettingsManager.ReadSettingsAsync<BaseSettings>(options.SettingsName);
                }
            }

            if (settings == null)
            {
                throw new SettingsNotFoundException(options.SettingsName);
            }

            if (settings.Type == SettingsType.Application)
            {
                var settingsChanger = new ApplicationSettingsChanger();
                settingsChanger.Change((ApplicationSettings)settings);
            }
            else if (settings.Type == SettingsType.Component)
            {
                var settingsChanger = new ComponentSettingsChanger();
                settingsChanger.Change((ComponentSettings)settings);
            }
        }
    }
}
