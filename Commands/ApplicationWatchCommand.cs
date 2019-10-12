using Blazor.CssBundler.Bundle.App;
using Blazor.CssBundler.Commands.Parser;
using Blazor.CssBundler.Exceptions;
using Blazor.CssBundler.Interactive;
using Blazor.CssBundler.Models.Settings;
using Blazor.CssBundler.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    class ApplicationWatchCommand : ICommand
    {
        private AutoApplicationBundler _bundler;

        public string[] PossibleArguments { get; private set; }

        public ApplicationWatchCommand()
        {
            PossibleArguments = new string[]
            {
                "watch"
            };
        }

        public void Start(Argument[] args)
        {

        }

        public async Task StartAsync(Argument[] args)
        {
            bool watchArg = args.Select(x => x.Name).Contains("watch");
            string name = args.Where(x => x.Name == "name").FirstOrDefault()?.Value;
            string type = args.Where(x => x.Name == "type").FirstOrDefault()?.Value;
            if (!watchArg)
            {
                var settingsList = SettingsManager.AllSettingsInfo().ToArray();
                if (settingsList.Length < 1)
                {
                    Console.WriteLine("Settings not found! For start watching application you need settings, create them!");
                    return;
                }
                var selection = new VerticalSettingsSelector(settingsList.Select(x => new SettingsSelectionItem(x.name, x.type)).ToArray());
                SettingsSelectionItem selectedItem = selection.GetUserSelection();
                name = selectedItem.Name;
                type = selectedItem.Type;
            }

            if (name == null && type == null)
            {
                
            }

            if (name != null && type == "application")
            {
                var settings = await SettingsManager.ReadAsync<ApplicationSettings>(name);

            }
            else
            {
                throw new SettingsNotFoundException(name, type);
            }
        }

        public void Stop()
        {
            Console.WriteLine("ApplicationWatchCommand stopped");
        }

        public async Task StopAsync()
        {
            await Task.Run(() => Stop());
        }
    }
}
