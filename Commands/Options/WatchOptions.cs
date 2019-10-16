using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.CssBundler.Commands.Options
{
    [Verb("watch", HelpText = "Watching selected project and create new css bundle if need")]
    class WatchOptions
    {
        [Option(longName: "name", shortName: 'n', HelpText = "Settings name")]
        public string SettingsName { get; set; }
    }
}
