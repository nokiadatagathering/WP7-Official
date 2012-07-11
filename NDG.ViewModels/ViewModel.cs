// ----------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using NDG.ViewModels.Helpers;
    using System.Threading;
    using NDG.Helpers.Classes;
    using System.Windows;
    using System;

    public class TestCompletedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    /// <summary>
    /// Base view model, that contains common logic.
    /// </summary>
    public class ViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// Synchronization context of user interface thread. It's need for update binding properties in callbacks.
        /// </summary>
        private static SynchronizationContext syncContext = SynchronizationContextProvider.UIThreadSyncContext;

        /// <summary>
        /// Provider for navigation in application.
        /// </summary>
        private static NavigationProvider navigationProvider = new NavigationProvider();

        /// <summary>
        /// Count of current requests.
        /// </summary>
        private int busyCount = 0;

        #endregion Fields

        public event EventHandler<TestCompletedEventArgs> TestCompleted;

        public ViewModel()
        {
#if !DEBUG && UNIT_TEST
            throw new InvalidOperationException("Cannot run application in RELEASE mode during UNIT_TEST mode included.");
#endif
        }

        #region Properties

        /// <summary>
        /// Gets synchronization context of user interface thread. It's need for update binding properties in callbacks.
        /// </summary>
        public static SynchronizationContext SyncContext
        {
            get
            {
                return syncContext;
            }
        }

        /// <summary>
        /// Gets navigation provider.
        /// </summary>
        public static NavigationProvider NavigationProvider
        {
            get
            {
                return navigationProvider;
            }
        }

        /// <summary>
        /// Gets or sets command for initialization view model.
        /// </summary>
        public ICommand InitializeViewModelCommand { get; protected set; }

        /// <summary>
        /// Gets or sets count of current requests.
        /// </summary>
        public int BusyCount
        {
            get
            {
                return this.busyCount;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                var changeValue = value - this.busyCount;
                this.busyCount = value;
                if (this != Locator.HomeStatic)
                {
                    Locator.HomeStatic.BusyCount += changeValue;
                }

                this.RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Gets a value indicating whether is there executed requests.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.BusyCount > 0;
            }
        }

        #endregion Properties

        protected void RaiseTestCompleted(string message)
        {
            var handler = this.TestCompleted;
            if (handler != null)
            {
                handler(this, new TestCompletedEventArgs { Message = message });
            }
        }
    }
}
