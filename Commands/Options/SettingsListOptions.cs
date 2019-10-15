using CommandLine;

namespace Blazor.CssBundler.Commands.Options
{
    [Verb("settings-list", HelpText = "Display settings list")]
    class SettingsListOptions
    {
        [Option(SetName = "type", Default = SettingsDisplayType.All, HelpText = "Settings display type")]
        public SettingsDisplayType Type { get; set; }
    }
}
