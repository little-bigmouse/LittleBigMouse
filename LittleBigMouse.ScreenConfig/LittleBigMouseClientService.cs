using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HLab.DependencyInjection.Annotations;
using HLab.Remote;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace LittleBigMouse.ScreenConfig
{

     [Export(typeof(ILittleBigMouseClientService)),Singleton]
    public class LittleBigMouseClientService : RemoteClient, ILittleBigMouseClientService
    {
        public event Action<string> StateChanged;

        public void OnStateChange(string state)
        {
            StateChanged?.Invoke(state);
        }

        private readonly PipeServer _server;

        public LittleBigMouseClientService():base("lbm.daemon")
        {
            _server = new PipeServer{PipeName = "lbm.control", Command =  CommandLine};
            _server.Run();
            SendAsync<bool>("register lbm.control");
        }


        private async Task<string> CommandLine(string command)
        {
            switch (command)
            {
                
            }
            return "";
        }

        public Task<bool> LoadConfig() => SendAsync<bool>();
        public Task<bool> Start() => SendAsync<bool>();

        public Task<bool> Stop() => SendAsync<bool>();

        public Task Update() => SendAsync();
        public Task<bool> Quit() => SendAsync<bool>();

        public Task<bool> LoadAtStartup(bool state = true) => SendAsync<bool>();

        public Task CommandLine(IList<string> args) => SendAsync();

        public Task<bool> Running() => SendAsync<bool>();

        protected override bool StartServer()
        {
            var args = "";
            var module = Process.GetCurrentProcess().MainModule;

            var filename = module?.FileName;
            if (filename == null) return false;
                
            filename = filename.Replace(".Control.Loader", ".Daemon").Replace(".vshost", "");
            try
            {
                var process = Process.Start(filename, args);
                if (process?.Responding ?? false)
                {
                    SendAsync<bool>("register=lbm.control");
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}