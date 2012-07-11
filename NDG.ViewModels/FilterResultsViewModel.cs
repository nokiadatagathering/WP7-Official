using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using NDG.BussinesLogic.SpecificLogic;
using NDG.Common;
using NDG.DataAccessModels;
using NDG.ViewModels.Helpers;

namespace NDG.ViewModels
{
    public class FilterResultsViewModel : ViewModel
    {   
        #region Fields

        private ObservableCollection<ResponseSet> filteredSavedResults = new ObservableCollection<ResponseSet>();

        private ObservableCollection<ResponseSet> filteredSubmittedResults = new ObservableCollection<ResponseSet>();

        private DeferredSearchHelper savedSearchHelper;

        private FilterProcessor _filterProcessor;

        private DeferredSearchHelper submittedSearchHelper;

        private string searchInSavedString = string.Empty;

        private string searchInSubmittedString = string.Empty;

        private FilterParameters currentParameters = new FilterParameters();

        private ObservableCollection<ResponseSet> submittedResponses = new ObservableCollection<ResponseSet>();

        private ObservableCollection<ResponseSet> savedResponses = new ObservableCollection<ResponseSet>();

        #endregion Fields

        public FilterResultsViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
            this.savedSearchHelper = new DeferredSearchHelper("SearchInSavedString", this, this.SearchInSaved);
            this.submittedSearchHelper = new DeferredSearchHelper("SearchInSubmittedString", this, this.SearchInSubmitted);
            this.NavigateBackCommand = new RelayCommand(this.NavigateBackExecute);
        }

        #region Properties

        public RelayCommand NavigateBackCommand { get; private set; }

        public string SearchInSavedString
        {
            get { return this.searchInSavedString; }
            set { this.searchInSavedString = value; this.RaisePropertyChanged("SearchInSavedString"); }
        }

        public string SearchInSubmittedString
        {
            get { return this.searchInSubmittedString; }
            set { this.searchInSubmittedString = value; this.RaisePropertyChanged("SearchInSubmittedString"); }
        }

        public ObservableCollection<ResponseSet> SubmittedResponses
        {
            get { return this.submittedResponses; }
            set { this.submittedResponses = value; this.RaisePropertyChanged("SubmittedResponses"); }
        }

        public ObservableCollection<ResponseSet> SavedResponses
        {
            get { return this.savedResponses; }
            set { this.savedResponses = value; this.RaisePropertyChanged("SavedResponses"); }
        }        

        #endregion Properties

        private void InitializeViewModelExecute()
        {
            this._filterProcessor = new FilterProcessor();
            this.savedSearchHelper.StartSearch();
            this.submittedSearchHelper.StartSearch();
            this.currentParameters = new FilterParameters();
            this.SearchInSavedString = string.Empty;
            this.SearchInSubmittedString = string.Empty;
           
            if (NavigationProvider.NavigationParameters.ContainsKey(Constants.FILTER_KEY))
            {
                this.currentParameters = (FilterParameters)NavigationProvider.NavigationParameters[Constants.FILTER_KEY];
                NavigationProvider.NavigationParameters.Remove(Constants.FILTER_KEY);
                this.SavedResponses = new ObservableCollection<ResponseSet>(_filterProcessor.FilterSavedResponseSet(currentParameters));
                this.filteredSavedResults = this.SavedResponses;
                this.SubmittedResponses = new ObservableCollection<ResponseSet>(_filterProcessor.FilterSubmittedResponseSet(currentParameters));
                this.filteredSubmittedResults = this.SubmittedResponses;
            }
        }

        private void NavigateBackExecute()
        {
            this.savedSearchHelper.StopSearch();
            this.submittedSearchHelper.StopSearch();
            this.SubmittedResponses = new ObservableCollection<ResponseSet>();
            this.SavedResponses = new ObservableCollection<ResponseSet>();
            this.SearchInSavedString = string.Empty;
            this.SearchInSubmittedString = string.Empty;
        }

        private void SearchInSubmitted()
        {
            if (!string.IsNullOrWhiteSpace(this.SearchInSubmittedString))
            {
                var findedResults = this.filteredSubmittedResults.Where(item => item.Name.Contains(this.searchInSubmittedString.ToLower()));
                this.SubmittedResponses = new ObservableCollection<ResponseSet>(findedResults);
            }
            else
            {
                this.SubmittedResponses = this.filteredSubmittedResults;
            }

            this.BusyCount--;
        }

        private void SearchInSaved()
        {
            if (!string.IsNullOrWhiteSpace(this.SearchInSavedString))
            {
                var findedResults = this.filteredSavedResults.Where(item => item.Name.Contains(this.searchInSavedString.ToLower()));
                this.SavedResponses = new ObservableCollection<ResponseSet>(findedResults);
            }
            else
            {
                this.SavedResponses = this.filteredSavedResults;
            }

            this.BusyCount--;
        }
    }
}
