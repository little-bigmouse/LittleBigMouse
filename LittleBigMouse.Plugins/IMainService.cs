using System;
using System.Collections.Generic;
using System.Text;
using HLab.Mvvm.Annotations;

namespace LittleBigMouse.Plugins
{
    public interface IMainService
    {
        void AddButton(string iconPath, string toolTip, Action activate, Action deactivate);
        void SetViewMode(Type viewMode);
        void SetViewMode<T>() where T:ViewMode;
    }
}
