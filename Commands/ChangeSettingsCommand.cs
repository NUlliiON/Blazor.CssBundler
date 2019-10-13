using Blazor.CssBundler.Commands.Options;
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
            if (options.SettingsName == null)
            {
                var settingsList = await SettingsManager.GetAboutAllSettings().ToArrayAsync();
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
    }
}
