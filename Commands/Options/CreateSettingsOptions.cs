using Blazor.CssBundler.Models.Settings;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Commands.Options
{
    [Verb("create-settings", HelpText = "Create new settings")]
    class CreateSettingsOptions
    {
        [Option(longName: "name", shortName: 'n', HelpText = "Settings name")]
        public string SettingsName { get; set; }

        [Option(longName: "type", shortName: 't', HelpText = "Settings type")]
        public SettingsType SettingsType { get; set; }
    }
}
