using Blazor.CssBundler.Commands.Options;
using Blazor.CssBundler.Logging;
using Blazor.CssBundler.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    class SettingsListCommand : BaseCommand<SettingsListOptions>
    {
        public override void Execute(ILogger logger, SettingsListOptions options)
        {
            throw new NotImplementedException();
        }

        public override async Task ExecuteAsync(ILogger logger, SettingsListOptions options)
        {
            var settingsList = await SettingsManager.GetAboutAllSettings()
                .OrderByDescending(x => x.type)
                .ToListAsync();

            if (options.Type == SettingsDisplayType.All)
            {
                for (int i = 0; i < settingsList.Count; i++)
                {
                    logger.Print("(" + (i + 1) + ") " + settingsList[i].name + " - " + settingsList[i].type);
                }
            }
            else
            {
                for (int i = 0; i < settingsList.Count; i++)
                {
                    logger.Print("(" + (i + 1) + ") " + settingsList[i].name);
                }
            }
        }
    }
}
