using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Commands.Options
{
    [Verb("change-settings", HelpText = "Change settings")]
    class ChangeSettingsOptions
    {
        [Option(SetName = "name", HelpText = "Settings name")]
        public string SettingsName { get; set; }
    }
}
