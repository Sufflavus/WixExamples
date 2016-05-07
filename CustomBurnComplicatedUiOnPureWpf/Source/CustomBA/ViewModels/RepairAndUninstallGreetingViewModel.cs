using CustomBA.ViewModels.Interfaces;

namespace CustomBA.ViewModels
{
    public class RepairAndUninstallGreetingViewModel : IRepairAndUninstallGreetingViewModel
    {
        private readonly InstallViewModel _mainViewModel;

        public RepairAndUninstallGreetingViewModel(InstallViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}