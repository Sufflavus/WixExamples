using System;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace CustomBA.Models
{
    public class BootstrapperApplicationModel
    {
        private IntPtr _hwnd;

        public BootstrapperApplicationModel(BootstrapperApplication bootstrapperApplication)
        {
            BootstrapperApplication =
                bootstrapperApplication;
            _hwnd = IntPtr.Zero;
        }

        public BootstrapperApplication BootstrapperApplication { get; }

        /// <summary>
        ///     We will use this to store the exit status code that the Burn engine returns after the bootstrapper has finished.
        ///     This status code will be passed to the Engine.Quit(model.FinalResult) method at the end of the Run method
        ///     in our CustomBootstrapperApplication class.
        /// </summary>
        public int FinalResult { get; set; }

        public void SetWindowHandle(Window view)
        {
            _hwnd = new WindowInteropHelper(view).Handle;
        }

        /// <summary>
        ///     When a button is clicked on the UI, we call PlanAction, passing in the task that we want to execute.
        ///     Once planning has completed, we invoke ApplyAction.Plan and Apply, along with Detect,
        ///     are among the most important events you'll interact with.
        /// </summary>
        /// <param name="action"></param>
        public void PlanAction(LaunchAction action)
        {
            BootstrapperApplication.Engine.Plan(action);
        }

        public void ApplyAction()
        {
            BootstrapperApplication.Engine.Apply(_hwnd);
        }

        /// <summary>
        ///     a helper for appending messages to the bootstrapper's log, which can be found in the %TEMP% directory
        /// </summary>
        /// <param name="message"></param>
        public void LogMessage(string message)
        {
            BootstrapperApplication.Engine.Log(
                LogLevel.Standard,
                message);
        }

        /// <summary>
        ///     This method will now accept the name of the variable that we want to set and the value to set it to.
        ///     It passes this information to the StringVariables property, which passes it to the bootstrapper.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        public void SetBurnVariable(string variableName, string value)
        {
            BootstrapperApplication.Engine.StringVariables[variableName] = value;
        }
    }
}