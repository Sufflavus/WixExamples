using CustomBA.ViewModels.Interfaces;

namespace CustomBA.ViewModels
{
    public class UninstallProgressViewModel : IUninstallProgressViewModel
    {
        private readonly InstallViewModel _mainViewModel;

        public UninstallProgressViewModel(InstallViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}