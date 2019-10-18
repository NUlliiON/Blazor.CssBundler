using Blazor.CssBundler.Commands.Options;
using Blazor.CssBundler.Logging;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Settings;
using System;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    class CreateSettingsCommand : BaseCommand<CreateSettingsOptions>
    {
        public override void Execute(ILogger logger, CreateSettingsOptions options)
        {
            throw new NotImplementedException();
        }

        public override async Task ExecuteAsync(ILogger logger, CreateSettingsOptions options)
        {
            if (await SettingsManager.SettingsExists(options.SettingsName))
            {
                logger.Print("</warn/> Settings with this name or type already exists");
                return;
            }

            await SettingsManager.CreateAsync(options.SettingsName);
            if (options.SettingsType == SettingsType.Application)
            {
                var settings = new ApplicationSettings() 
                { 
                    Name = options.SettingsName 
                };
                await SettingsManager.SaveAsync(settings);
            }
            else if (options.SettingsType == SettingsType.Component)
            {
                var settings = new ComponentSettings()
                {
                    Name = options.SettingsName
                };
                await SettingsManager.SaveAsync(settings);
            }
            else
            {
                logger.Print($"Settings type with name \"{options.SettingsName}\" not found");
                logger.Print("Available types:");
                foreach (string enumValue in Enum.GetValues(typeof(SettingsType)))
                {
                    logger.Print(" - " + enumValue);
                }
            }
            logger.Print($"Settings with name \"{options.SettingsName}\" and type \"{options.SettingsType.ToString()}\" </checkmark/> created </grinning_face/>");
        }
    }
}
