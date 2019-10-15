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
        [Option(SetName = "name", HelpText = "Settings name", Required = true)]
        public string SettingsName { get; set; }

        [Option(SetName = "type", HelpText = "Settings type", Required = true)]
        public SettingsType SettingsType { get; set; }
    }
}
