using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HLab.DependencyInjection.Annotations;
using HLab.Remote;
using JKang.IpcServiceFramework.Client;
using LittleBigMouse.ScreenConfigs;
using Microsoft.Extensions.DependencyInjection;

namespace LittleBigMouse.ScreenConfig
{

     [Export(typeof(ILittleBigMouseClientService)),Singleton]
    public class LittleBigMouseClientService : RemoteClient, ILittleBigMouseClientService
    {
        public event Action<bool> StateChanged;

        public void OnStateChange(bool state)
        {
            StateChanged?.Invoke(state);
        }


        private void RefreshClient()
        {
            //_client.Abort();
            //_client = new LittleBigMouseClient(this);
            //_client.InnerDuplexChannel.Faulted += (a, e) => RefreshClient();
            //_client.InnerDuplexChannel.Closing += (a, e) => OnStateChange(false);
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
            var p = Process.GetCurrentProcess();
            string filename = p.MainModule.FileName.Replace(".Control.Loader", ".Daemon").Replace(".vshost", "");
            var process = Process.Start(filename, args);
            return process.Responding;
        }

        public void LauchServer(string args = "")
        {
            var p = Process.GetCurrentProcess();
            string filename = p.MainModule.FileName.Replace("_Control", "_Daemon").Replace(".vshost", "");
            var process = Process.Start(filename, args);
        }

    }

}