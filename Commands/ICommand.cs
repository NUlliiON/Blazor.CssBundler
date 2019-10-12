using Blazor.CssBundler.Commands.Parser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    interface ICommand
    {
        string[] PossibleArguments { get; }

        void Start(Argument[] args);
        
        Task StartAsync(Argument[] args);
        
        void Stop();
        
        Task StopAsync();
    }
}
