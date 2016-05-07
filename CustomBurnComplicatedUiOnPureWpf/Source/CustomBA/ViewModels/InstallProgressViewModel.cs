using CustomBA.ViewModels.Interfaces;

namespace CustomBA.ViewModels
{
    public class InstallProgressViewModel : IInstallProgressViewModel
    {
        private readonly InstallViewModel _mainViewModel;

        public InstallProgressViewModel(InstallViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}