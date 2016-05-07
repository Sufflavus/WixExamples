using CustomBA.ViewModels.Interfaces;

namespace CustomBA.ViewModels
{
    public class FinishViewModel : IFinishViewModel
    {
        private readonly InstallViewModel _mainViewModel;

        public FinishViewModel(InstallViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}