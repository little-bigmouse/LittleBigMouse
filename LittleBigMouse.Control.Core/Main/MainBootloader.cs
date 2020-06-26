using System;
using System.Windows;
using HLab.Core.Annotations;
using HLab.DependencyInjection.Annotations;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;

namespace LittleBigMouse.Control.Core.Main
{
    public class MainBootloader : IBootloader
    {
        [Import]
        public MainBootloader(MainService mainService, IMvvmService mvvmService)
        {
            _mainService = mainService;
            _mvvmService = mvvmService;
        }

        private readonly MainService _mainService;
        private readonly IMvvmService _mvvmService;

        [Import] private Func<ScreenConfig.ScreenConfig,MultiScreensViewModel> _getViewModel;

        public void Load(IBootContext bootstrapper)
        {
            var viewModel = _mainService.MainViewModel;

            viewModel.Config = _mainService.Config;

            viewModel.Presenter = _getViewModel(_mainService.Config);

            var view = (Window)_mvvmService.MainContext.GetView<ViewModeDefault>(viewModel, typeof(IViewClassDefault));

            view.Show();
        }

    }
}