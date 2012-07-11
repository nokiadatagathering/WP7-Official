using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;

namespace NDG.ViewModels.Helpers
{
    public class DeferredSearchHelper
    {
        #region Fields

        bool canInvokeCallback = false;

        private DispatcherTimer timer = new DispatcherTimer();

        private string searchStringProperty = string.Empty;

        private ViewModel refferedViewModel;

        private Action callback;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the DeferredSearchHelper class.
        /// </summary>
        /// <param name="propertySearch">Property of view model that represent search string.</param>
        /// <param name="refferedViewModel">Refered view model.</param>
        /// <param name="callback">Callback that invokes for notificate about start search.</param>
        public DeferredSearchHelper(string propertySearch, ViewModel refferedViewModel,  Action callback)
        {
            if (string.IsNullOrEmpty(propertySearch) || refferedViewModel == null || callback == null)
            {
                throw new ArgumentException("PropertySearch must be not string empty, all method parameters can't have null value.");
            }

            this.refferedViewModel = refferedViewModel;
            this.searchStringProperty = propertySearch;
            this.callback = callback;
            this.timer.Interval = new TimeSpan(0, 0, 1);            
        }

        public void StartSearch()
        {
            this.timer.Tick += this.OnTimerTick;
            refferedViewModel.PropertyChanged += this.OnPropertyChanged;
            this.timer.Start();
        }

        public void StopSearch()
        {
            this.timer.Stop();
            this.timer.Tick -= this.OnTimerTick;
            this.refferedViewModel.PropertyChanged -= this.OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.searchStringProperty)
            {
                this.canInvokeCallback = true;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (canInvokeCallback)
            {
                this.refferedViewModel.BusyCount++;
                this.callback.Invoke();
                this.canInvokeCallback = false;
            }
        }
    }
}
