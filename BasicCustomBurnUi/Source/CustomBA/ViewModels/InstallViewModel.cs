﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using CustomBA.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace CustomBA.ViewModels
{
    public class InstallViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     This will track which phase of the bootstrapping process we are in so that we can enable and disable buttons as
        ///     appropriate.
        ///     It will also allow us to track whether the user has canceled the install.
        ///     If they have, and we're already installing—otherwise known as Applying—we will know to not immediately shut down
        ///     the process,
        ///     but rather send a flag to the bootstrapper so that it can roll back any installed packages.
        ///     If, on the other hand, we have not begun the Apply phase, it is safe to shut down the bootstrapper immediately.
        /// </summary>
        public enum InstallState
        {
            Initializing,
            Present,
            NotPresent,
            Applying,
            Cancelled
        }

        private readonly BootstrapperApplicationModel _model;
        private int _cacheProgress;
        private int _executeProgress;

        private string _message;
        private int _progress;
        private InstallState _state;
        private string _username;

        public InstallViewModel(BootstrapperApplicationModel model)
        {
            _model = model;

            State = InstallState.Initializing;

            WireUpEventHandlers();

            InstallCommand = new DelegateCommand(() =>
                _model.PlanAction(LaunchAction.Install),
                // an anonymous method to invoke when the command is executed
                () => State == InstallState.NotPresent);
            // anonymous method that returns a Boolean value that signifies whether the command, and all UI controls that are bound to it, should be enabled.

            UninstallCommand = new DelegateCommand(() =>
                _model.PlanAction(LaunchAction.Uninstall),
                () => State == InstallState.Present);

            CancelCommand = new DelegateCommand(() =>
            {
                _model.LogMessage("Cancelling...");
                if (State == InstallState.Applying)
                {
                    State = InstallState.Cancelled;
                }
                else
                {
                    CustomBootstrapperApplication.Dispatcher
                        .InvokeShutdown();
                }
            }, () => State != InstallState.Cancelled);

            // If you'd like to show a progress bar during the installation, you can handle two events: CacheAcquireProgress and ExecuteProgress. 
            // The former will give you a percentage completed for caching the packages. The latter will give you a percentage for packages executed. 
            // To get a total progress percentage, we add them both together and divide by two—if we didn't divide by two we'd end up 
            // with a final result of 200 since both events count up to 100.
            _model.BootstrapperApplication.CacheAcquireProgress +=
                (sender, args) =>
                {
                    _cacheProgress = args.OverallPercentage;
                    Progress =
                        (_cacheProgress + _executeProgress)/2;
                };
            _model.BootstrapperApplication.ExecuteProgress +=
                (sender, args) =>
                {
                    _executeProgress = args.OverallPercentage;
                    Progress =
                        (_cacheProgress + _executeProgress)/2;
                };
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;

                // set value to variable "Username" inside Bundle of our Bootstrapper
                _model.SetBurnVariable("Username", _username);
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Progress)));
            }
        }

        public ICommand InstallCommand { get; }
        public ICommand UninstallCommand { get; }
        public ICommand CancelCommand { get; }

        /// <summary>
        ///     text that we want to display on the WPF window
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        /// <summary>
        ///     state is used to hold the current status of the installation
        /// </summary>
        public InstallState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    Message = _state.ToString();
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(State)));
                    Refresh();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     This method is called when the Detect method that we called in the CustomBootstrapperApplication class' Run method
        ///     completes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DetectPackageComplete(
            object sender,
            DetectPackageCompleteEventArgs e)
        {
            // First, check the DetectPackageCompleteEventArgs object to see if the package that was detected was an installer called MyInstaller.msi. 
            // If it finds a match, check whether that package is currently installed on the end user's computer via the PackageState enumeration. 
            // If it is Present, we set the State property to InstallState.Present, otherwise it is set to InstallState.NotPresent.
            if (e.PackageId.Equals(
                "SimpleSetup.msi", StringComparison.Ordinal))
            {
                State = e.State == PackageState.Present ? InstallState.Present : InstallState.NotPresent;
            }
            // The Present/NotPresent value will be used by the InstallCommand and UninstallComand properties 
            // to enable or disable the UI controls that are bound to them.
        }

        /// <summary>
        ///     after we've called PlanAction to kick off the installation or uninstallation and planning has completed, we then
        ///     want to trigger ApplyAction.
        ///     This hand off is performed in the PlanComplete event handler.Additionally, if the user has canceled the install and
        ///     at this stage we haven't applied any changes to the computer—we are free to simply call InvokeShutdown on the
        ///     Dispatcher object,
        ///     essentially stopping before we've even begun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlanComplete(
            object sender, PlanCompleteEventArgs e)
        {
            if (State == InstallState.Cancelled)
            {
                CustomBootstrapperApplication.Dispatcher
                    .InvokeShutdown();
                return;
            }
            _model.ApplyAction();
        }

        protected void ApplyBegin(
            object sender, ApplyBeginEventArgs e)
        {
            State = InstallState.Applying;
        }

        /// <summary>
        ///     triggered before each package in the chain is installed or uninstalled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExecutePackageBegin(
            object sender, ExecutePackageBeginEventArgs e)
        {
            if (State == InstallState.Cancelled)
            {
                e.Result = Result.Cancel;
            }
        }

        /// <summary>
        ///     triggered after each package in the chain is installed or uninstalled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExecutePackageComplete(
            object sender, ExecutePackageCompleteEventArgs e)
        {
            // If we find that the bootstrapper has been canceled, we set the Result property on EventArgs to Result.Cancel. 
            // This will inform Burn that it should roll back any packages that have been installed up to that point. 
            // This is a graceful way of handling a cancelation, rather than shutting down the process outright.
            if (State == InstallState.Cancelled)
            {
                e.Result = Result.Cancel;
            }
        }

        /// <summary>
        ///     It's called when the installation is fully complete and the planned action has been applied
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyComplete(
            object sender, ApplyCompleteEventArgs e)
        {
            _model.FinalResult = e.Status;
            CustomBootstrapperApplication.Dispatcher
                .InvokeShutdown();
        }

        private void Refresh()
        {
            // The Refresh method calls RaiseCanExecuteChanged on each of the command properties. 
            // This allows the UI to update the enabled/disabled status of buttons that are bound to these commands. 
            // Notice that we've nested these calls inside Dispatcher.Invoke. We must do this because it's likely that the Refresh method
            // will be called by a background thread that won't have access to the UI thread. The Dispatcher ferries messages between the two.
            CustomBootstrapperApplication.Dispatcher.Invoke(
                () =>
                {
                    ((DelegateCommand) InstallCommand)
                        .RaiseCanExecuteChanged();
                    ((DelegateCommand) UninstallCommand)
                        .RaiseCanExecuteChanged();
                    ((DelegateCommand) CancelCommand)
                        .RaiseCanExecuteChanged();
                });
        }

        /// <summary>
        ///     associates event handlers with the events that are fired by the bootstrapper
        /// </summary>
        private void WireUpEventHandlers()
        {
            _model.BootstrapperApplication.DetectPackageComplete
                += DetectPackageComplete;
            _model.BootstrapperApplication.PlanComplete += PlanComplete;
            _model.BootstrapperApplication.ApplyComplete += ApplyComplete;
            _model.BootstrapperApplication.ApplyBegin += ApplyBegin;
            _model.BootstrapperApplication.ExecutePackageBegin +=
                ExecutePackageBegin;
            _model.BootstrapperApplication.ExecutePackageComplete
                += ExecutePackageComplete;
        }
    }
}