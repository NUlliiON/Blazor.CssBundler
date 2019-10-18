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
            try
            {
                BaseSettings oldSettings = null;

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
                        oldSettings = await SettingsManager.ReadAsync<BaseSettings>(selectedItem.Name);
                    }
                }
                else
                {
                    if (await SettingsManager.SettingsExists(options.SettingsName))
                    {
                        oldSettings = await SettingsManager.ReadAsync<BaseSettings>(options.SettingsName);
                    }
                }

                if (oldSettings == null)
                {
                    throw new SettingsNotFoundException(options.SettingsName);
                }


                BaseSettings newSettings = null;
                if (oldSettings.Type == SettingsType.Application)
                {
                    var settingsChanger = new ApplicationSettingsChanger();
                    newSettings = settingsChanger.Change((ApplicationSettings)oldSettings);
                }
                else if (oldSettings.Type == SettingsType.Component)
                {
                    var settingsChanger = new ComponentSettingsChanger();
                    newSettings = settingsChanger.Change((ComponentSettings)oldSettings);
                }

                if (oldSettings.Name != newSettings.Name)
                    await SettingsManager.ChangeSettingsNameAsync(oldSettings.Name, newSettings.Name);

                await SettingsManager.SaveAsync(newSettings);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
