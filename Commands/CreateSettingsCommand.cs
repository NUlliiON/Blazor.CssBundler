using Blazor.CssBundler.Commands.Options;
using Blazor.CssBundler.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    class CreateSettingsCommand : BaseCommand<CreateSettingsOptions>
    {
        public override void Execute(ILogger logger, CreateSettingsOptions options)
        {
            throw new NotImplementedException();
        }

        public override Task ExecuteAsync(ILogger logger, CreateSettingsOptions options)
        {
            
        }
    }
}
