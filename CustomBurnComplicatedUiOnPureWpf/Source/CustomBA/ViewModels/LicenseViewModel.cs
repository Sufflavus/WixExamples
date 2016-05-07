using CustomBA.ViewModels.Interfaces;

namespace CustomBA.ViewModels
{
    public class LicenseViewModel : ILicenseViewModel
    {
        private readonly InstallViewModel _mainViewModel;

        public LicenseViewModel(InstallViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}