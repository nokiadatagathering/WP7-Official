using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using NDG.Common;

namespace NDG.ViewModels
{
    public enum FilterPages
    {
        FilterByDate = 0,

        FilterByLocation = 1
    }

    public class FilterResponsesViewModel : ViewModel
    {
        #region Fields

        private TimePeriodKeyValuePair selectedPeriod;

        private ObservableCollection<TimePeriodKeyValuePair> periods = new ObservableCollection<TimePeriodKeyValuePair>();

        private DateTime selectedDate;

        private DateTime selectedStartDate;

        private DateTime selectedEndDate;

        private bool isBetweenPeriodSelected = false;

        private bool isFilterByAddress = false;

        private bool isFilterByGps = false;

        private string streetAddress = string.Empty;

        private string city = string.Empty;

        private string state = string.Empty;

        private FilterPages currentFilter;

        #endregion Fields

        public FilterResponsesViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
        }

        #region Properties

        public TimePeriodKeyValuePair SelectedPeriod
        {
            get { return this.selectedPeriod; }
            set
            {
                this.selectedPeriod = value;
                this.RaisePropertyChanged("SelectedPeriod");
                IsBetweenPeriodSelected = (value.Key == TimePeriods.Between);
                Locator.NavigationStatic.NavigateToFilterResultCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<TimePeriodKeyValuePair> Periods
        {
            get { return this.periods; }
            set { this.periods = value; this.RaisePropertyChanged("Periods"); }
        }

        public DateTime SelectedDate
        {
            get { return this.selectedDate; }
            set { this.selectedDate = value; this.RaisePropertyChanged("SelectedDate"); }
        }

        public DateTime SelectedStartDate
        {
            get { return this.selectedStartDate; }
            set 
            {
                this.selectedStartDate = value;
                if (this.SelectedEndDate < this.SelectedStartDate)
                {
                    this.SelectedEndDate = this.SelectedStartDate;
                }

                this.RaisePropertyChanged("SelectedStartDate"); 
            }
        }

        public DateTime SelectedEndDate
        {
            get { return this.selectedEndDate; }
            set { this.selectedEndDate = value; this.RaisePropertyChanged("SelectedEndDate"); }
        }

        public string StreetAddress
        {
            get { return this.streetAddress; }
            set { this.streetAddress = value; this.RaisePropertyChanged("StreetAddress"); Locator.NavigationStatic.NavigateToFilterResultCommand.RaiseCanExecuteChanged(); }
        }

        public string City
        {
            get { return this.city; }
            set { this.city = value; this.RaisePropertyChanged("City"); Locator.NavigationStatic.NavigateToFilterResultCommand.RaiseCanExecuteChanged(); }
        }

        public string State
        {
            get { return this.state; }
            set { this.state = value; this.RaisePropertyChanged("State"); Locator.NavigationStatic.NavigateToFilterResultCommand.RaiseCanExecuteChanged(); }
        }

        public FilterPages CurrentFilter
        {
            get { return this.currentFilter; }
            set { this.currentFilter = value; this.RaisePropertyChanged("CurrentFilter"); Locator.NavigationStatic.NavigateToFilterResultCommand.RaiseCanExecuteChanged(); }
        }

        public bool IsFilterByAddress
        {
            get { return this.isFilterByAddress; }
            set { this.isFilterByAddress = value; this.RaisePropertyChanged("IsFilterByAddress"); Locator.NavigationStatic.NavigateToFilterResultCommand.RaiseCanExecuteChanged(); }
        }

        public bool IsBetweenPeriodSelected
        {
            get { return this.isBetweenPeriodSelected; }
            set { this.isBetweenPeriodSelected = value; this.RaisePropertyChanged("IsBetweenPeriodSelected"); }
        }

        public bool IsFilterByGps
        {
            get { return this.isFilterByGps; }
            set { this.isFilterByGps = value; this.RaisePropertyChanged("IsFilterByGps"); Locator.NavigationStatic.NavigateToFilterResultCommand.RaiseCanExecuteChanged(); }
        }

        #endregion Properties

        private void InitializeViewModelExecute()
        {
            this.CurrentFilter = FilterPages.FilterByDate;
            if (this.Periods == null || this.Periods.Count == 0)
            {
                this.Periods = new ObservableCollection<TimePeriodKeyValuePair>();
                this.Periods.Add(new TimePeriodKeyValuePair(TimePeriods.After, "after"));
                this.Periods.Add(new TimePeriodKeyValuePair(TimePeriods.At, "at"));
                this.Periods.Add(new TimePeriodKeyValuePair(TimePeriods.Before, "before"));
                this.Periods.Add(new TimePeriodKeyValuePair(TimePeriods.Between, "between"));
            }

            this.SelectedPeriod = this.Periods[3];
            this.SelectedDate = this.SelectedStartDate = this.SelectedEndDate = DateTime.Now;
            this.IsFilterByGps = false;
            this.StreetAddress = this.City = this.State = string.Empty;
            this.IsFilterByAddress = false;
            Locator.NavigationStatic.NavigateToFilterCommand.IsExecutedNow = false;
        }
    }
}
