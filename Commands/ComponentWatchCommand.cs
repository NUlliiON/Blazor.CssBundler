using Blazor.CssBundler.Bundle.Component;
using Blazor.CssBundler.Commands.Parser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Commands
{
    class ComponentWatchCommand : ICommand
    {
        private AutoComponentBundler _bundler;

        public string[] PossibleArguments { get; private set; }

        public ComponentWatchCommand()
        {
            PossibleArguments = new string[]
            {
                "watch"
            };
        }

        public void Start(Argument[] args)
        {
            //_bundler = new AutoComponentBundler();

            Console.WriteLine("ApplicationWatchCommand started");
        }

        public async Task StartAsync(Argument[] args)
        {
            await Task.Run(() => Start(args));
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
