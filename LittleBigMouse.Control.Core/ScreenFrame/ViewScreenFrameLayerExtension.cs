using System.Windows.Controls;
using HLab.Mvvm.Extensions;
using LittleBigMouse.Control.Core.ScreenFrame;
using LittleBigMouse.Plugins;

namespace LittleBigMouse.Control.Core
{
    public static class ViewScreenFrameLayerExtension
    {
        public static ScreenFrameView GetFrame<T>(this T layer)
            where T : UserControl, IViewScreenFrameLayer
            => layer.FindVisualParent<ScreenFrameView>();

    }
}