using CustomBA.ViewModels.Interfaces;

namespace CustomBA.ViewModels
{
    public class InstallGreetingViewModel : IInstallGreetingViewModel
    {
        private readonly InstallViewModel _mainViewModel;

        public InstallGreetingViewModel(InstallViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}