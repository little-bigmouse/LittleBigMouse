/*
  LittleBigMouse.Screen.Config
  Copyright (c) 2017 Mathieu GRENET.  All right reserved.

  This file is part of LittleBigMouse.Screen.Config.

    LittleBigMouse.Screen.Config is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LittleBigMouse.Screen.Config is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MouseControl.  If not, see <http://www.gnu.org/licenses/>.

	  mailto:mathieu@mgth.fr
	  http://www.mgth.fr
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBigMouse.ScreenConfig
{
    public interface ILittleBigMouseClientService : ILittleBigMouseService
    {
        event Action<bool> StateChanged;
    }

    //    public class LittleBigMouseClient : DuplexClientBase<ILittleBigMouseService>, ILittleBigMouseService
    public class LittleBigMouseClient //: DuplexClientBase<ILittleBigMouseService>
    {


        public static Uri Address => new Uri("net.pipe://localhost/littlebigmouse");
        public LittleBigMouseClient(LittleBigMouseClientService service)
        {
            Service = service;
        }

        public ILittleBigMouseService Service { get; }
    }

    public class LittleBigMouseCallback : ILittleBigMouseCallback
    {
        //private readonly LittleBigMouseClientService _service;

        //public LittleBigMouseCallback(LittleBigMouseClientService service)
        //{
        //    _service = service;
        //}

        public void OnStateChange(bool state)
        {
//            _service.OnStateChange(state);
        }
    }


    public interface ILittleBigMouseCallback
    {
        void OnStateChange(bool state);
    }

    public interface ILittleBigMouseService
    {
        Task<bool> LoadConfig();
        Task<bool> Quit();
        Task<bool> Start();
        Task<bool> Stop();
        Task<bool> LoadAtStartup(bool state=true);
        Task CommandLine(IList<string> args);
        Task<bool> Running();
        Task Update();
    }

}
