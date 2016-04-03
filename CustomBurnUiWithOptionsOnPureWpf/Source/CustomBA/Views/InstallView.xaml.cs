using System.Windows;
using CustomBA.ViewModels;

namespace CustomBA.Views
{
    /// <summary>
    ///     Interaction logic for InstallView.xaml
    /// </summary>
    public partial class InstallView : Window
    {
        public InstallView(InstallViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // This will be called if the user closes the window instead of clicking on the Cancel button. 
            // Our method guides the code to execute our CancelCommand.
            Closed += (sender, e) => viewModel.CancelCommand.Execute(this);
        }
    }
}