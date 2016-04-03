using System.Windows.Threading;
using CustomBA.Models;
using CustomBA.ViewModels;
using CustomBA.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace CustomBA
{
    /// <summary>
    /// This identifies the class in our assembly that extends the BootstrapperApplication class. 
    /// Burn looks for this class and automatically calls its Run method.
    /// That will be our jumping-on point into the Burn process. 
    /// BootstrapperApplicationAttribute should be added in the Properties\AssemblyInfo.cs file.
    /// </summary>
    public class CustomBootstrapperApplication : BootstrapperApplication
    {
        /// <summary>
        /// A Dispatcher object provides a means for sending messages between the UI thread and any backend threads.
        /// It provides a handy Invoke method that we can use to update the state of our UI controls.
        /// Without it, the UI thread would ignore our attempts to interact with it from another thread.s
        /// </summary>
        public static Dispatcher Dispatcher { get; set; }

        /// <summary>
        /// This is our UI's primary entry point.It will be called by the Burn engine.
        /// </summary>
        protected override void Run()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;

            // BootstrapperApplicationModel wraps the calls to the Burn engine
            var model = new BootstrapperApplicationModel(this);
            // InstallViewModel process commands triggered by the view and pass data to the model
            var viewModel = new InstallViewModel(model);
            var view = new InstallView(viewModel);

            // This goes against the grain of our MVVM design, but is a necessary evil dictated by the BootstrapperCore library. 
            // This method will get a handle to the WPF window, which is needed by the Burn engine when performing the install or uninstall.
            model.SetWindowHandle(view);

            // This gives Burn the go-ahead to check if our bundle is already installed. 
            // That way, when our window is shown, we'll know whether we need to present an Install button or an Uninstall button.
            this.Engine.Detect();

            // display the WPF window
            view.Show();

            // Dispatcher.Run() halts execution of this method at that line until the Dispatcher is shut down. 
            // In the meantime, the Dispatcher object will loop in place, waiting for messages 
            // and providing methods for communicating with the UI thread—which is the current thread. 
            // Calling Dispatcher.Run() prevents our own Run method from exiting prematurely, which would terminate our process
            Dispatcher.Run();

            // When we decide to shut down the window, we'll call InvokeShutdown on the Dispatcher object, 
            // causing our Run method to continue its execution. At that point, Engine.Quit will be called 
            // with whatever status code we've collected at that point and wind down the bootstrapping process.
            this.Engine.Quit(model.FinalResult);
        }
    }
}