using CustomBA.ViewModels.Interfaces;

namespace CustomBA.ViewModels
{
    public class ParametersViewModel : IParametersViewModel
    {
        private readonly InstallViewModel _mainViewModel;

        public ParametersViewModel(InstallViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}