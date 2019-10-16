using Blazor.CssBundler.Models.Settings;
using CommandLine;

namespace Blazor.CssBundler.Commands.Options
{
    [Verb("settings-list", HelpText = "Display settings list")]
    class SettingsListOptions
    {
        [Option(longName: "type", shortName: 't', HelpText = "Settings type")]
        public SettingsType? SettingsType { get; set; }
    }
}
