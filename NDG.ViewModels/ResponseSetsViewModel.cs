using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using NDG.BussinesLogic.Providers;
using NDG.DataAccessModels;
using NDG.DataAccessModels.Repositories;
using NDG.ViewModels.Helpers;
using System.Windows;
using System.ComponentModel;

namespace NDG.ViewModels
{
    public enum ResponseSetsType
    {
        Saved,
        Submitted
    }

    public class ResponseSetsViewModel : ViewModel
    {
        #region Constants

        public const int TOP_COUNT = 4;

        #endregion Constants

        #region Fields

        private BackgroundWorker responseSetsWorker = new BackgroundWorker();
        private ResponseSetsType currentType;
        private DeferredSearchHelper searchHelper;
        private ResponseSetRepository responseRepository = new ResponseSetRepository();
        private ObservableCollection<ResponseSet> allResponses = new ObservableCollection<ResponseSet>();
        private ObservableCollection<ResponseSet> displayedResponses = new ObservableCollection<ResponseSet>();
        private ObservableCollection<ResponseSet> topResponses = new ObservableCollection<ResponseSet>();
        private string searchString = string.Empty;

        #endregion Fields

        public ResponseSetsViewModel(ResponseSetsType type)
        {
            this.currentType = type;
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
            this.searchHelper = new DeferredSearchHelper("SearchString", this, this.Search);
            this.NavigateBackCommand = new RelayCommand(this.NavigateBackExecute);
            this.DeleteResponseSetCommand = new RelayCommand<ResponseSet>(this.DeleteResponseSetExecute);
            this.OpenDuplicateCommand = new RelayCommand<int>(this.OpenDuplicateExecute);
        }

        #region Commands

        public RelayCommand<int> OpenDuplicateCommand { get; private set; }
        public RelayCommand NavigateBackCommand { get; private set; }
        public RelayCommand<ResponseSet> DeleteResponseSetCommand { get; private set; }

        #endregion Commands

        #region Properties

        public ObservableCollection<ResponseSet> DisplayedResponses
        {
            get { return this.displayedResponses; }
            set { this.displayedResponses = value; this.RaisePropertyChanged("DisplayedResponses"); }
        }

        public ObservableCollection<ResponseSet> TopResponses
        {
            get { return this.topResponses; }
            set { this.topResponses = value; this.RaisePropertyChanged("TopResponses"); }
        }

        public string SearchString
        {
            get { return this.searchString; }
            set { this.searchString = value; this.RaisePropertyChanged("SearchString"); }
        }

        #endregion Properties

        #region Methods

        internal void RefresheExecute()
        {
            this.BusyCount++;
            switch (this.currentType)
            {
                case ResponseSetsType.Saved:
                    this.TopResponses = new ObservableCollection<ResponseSet>(this.responseRepository.GetTopSavedResponseSetsForUser(Membership.CurrentUser.ID, TOP_COUNT));
                    this.displayedResponses = new ObservableCollection<ResponseSet>(this.responseRepository.GetSavedResponseSetsForUser(Membership.CurrentUser.ID));
#if UNIT_TEST
                    RaiseTestCompleted("Save_refresh");
#endif
                    break;
                case ResponseSetsType.Submitted:
                    this.TopResponses = new ObservableCollection<ResponseSet>(this.responseRepository.GetTopSubmittedResponseSetsForUser(Membership.CurrentUser.ID, TOP_COUNT));
                    this.displayedResponses = new ObservableCollection<ResponseSet>(this.responseRepository.GetSubmittedResponseSetsForUser(Membership.CurrentUser.ID));
#if UNIT_TEST
                    RaiseTestCompleted("Submitted_refresh");
#endif
                    break;
            }

            this.Sort();
            this.BusyCount--;
        }

        private void InitializeViewModelExecute()
        {
            this.BusyCount++;
            Locator.NavigationStatic.NavigateToFilterCommand.RaiseCanExecuteChanged();
            this.responseSetsWorker = new BackgroundWorker();
            this.searchHelper.StartSearch();
            this.responseRepository = new ResponseSetRepository();
            this.SearchString = string.Empty;
            this.allResponses = new ObservableCollection<ResponseSet>();
            this.responseSetsWorker.DoWork += new DoWorkEventHandler(this.LoadResponseSets);
            this.responseSetsWorker.RunWorkerAsync();
        }

        private void Sort()
        {
            this.allResponses = new ObservableCollection<ResponseSet>(this.allResponses.Reverse());
            this.displayedResponses = new ObservableCollection<ResponseSet>(this.displayedResponses.Reverse());
            this.DisplayedResponses = this.displayedResponses;
        }

        private void LoadResponseSets(object sender, DoWorkEventArgs e)
        {
            IEnumerable<ResponseSet> all = null;
            IEnumerable<ResponseSet> getedItems = null;
            switch (this.currentType)
            {
                case ResponseSetsType.Saved:
                    all = this.responseRepository.GetSavedResponseSetsForUser(Membership.CurrentUser.ID);
                    getedItems = this.responseRepository.GetTopSavedResponseSetsForUser(Membership.CurrentUser.ID, TOP_COUNT);
                    break;
                case ResponseSetsType.Submitted:
                    all = this.responseRepository.GetSubmittedResponseSetsForUser(Membership.CurrentUser.ID);
                    getedItems = this.responseRepository.GetTopSubmittedResponseSetsForUser(Membership.CurrentUser.ID, TOP_COUNT);
                    break;
            }

            this.allResponses = new ObservableCollection<ResponseSet>(all);            
            this.displayedResponses = this.allResponses;
            SyncContext.Post((parameter) =>
            {                
                this.Sort();
                this.TopResponses = new ObservableCollection<ResponseSet>(getedItems);
                this.BusyCount--;
                Locator.NavigationStatic.NavigateToFilterCommand.RaiseCanExecuteChanged();
            }, null);
        }

        private void NavigateBackExecute()
        {
            this.searchHelper.StopSearch();
            this.DisplayedResponses = new ObservableCollection<ResponseSet>();
            this.SearchString = string.Empty;
        }

        private void DeleteResponseSetExecute(ResponseSet selectedResponseSet)
        {
            this.BusyCount++;
            bool isDeleted = this.responseRepository.DeleteResponseSet(selectedResponseSet.ID);
            if (isDeleted)
            {
                IEnumerable<ResponseSet> top = null;
                IEnumerable<ResponseSet> all = null;
                switch (this.currentType)
                {
                    case ResponseSetsType.Saved:
                        top = this.responseRepository.GetTopSavedResponseSetsForUser(Membership.CurrentUser.ID, TOP_COUNT);
                        all = this.responseRepository.GetSavedResponseSetsForUser(Membership.CurrentUser.ID);
                        break;
                    case ResponseSetsType.Submitted:
                        top = this.responseRepository.GetTopSubmittedResponseSetsForUser(Membership.CurrentUser.ID, TOP_COUNT);
                        all = this.responseRepository.GetSubmittedResponseSetsForUser(Membership.CurrentUser.ID);
                        break;
                }

                if (Locator.SurveyDetailsStatic.CurrentSurvey != null && Locator.SurveyDetailsStatic.CurrentSurvey.ID != 0 && Locator.SurveyDetailsStatic.CurrentSurvey.ID == selectedResponseSet.SurveyID)
                {
                    Locator.SurveyDetailsStatic.Refresh();
                }

                this.TopResponses = new ObservableCollection<ResponseSet>(top);
                this.allResponses = new ObservableCollection<ResponseSet>(all);
                this.DisplayedResponses = this.allResponses;
#if !UNIT_TEST
                MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).RESPOSE_DELETED);
#else
                RaiseTestCompleted("RESPONSE_DELETED");
#endif
            }
            else
            {
                MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).ERROR_RESPONSE_DELETE);
            }

            Locator.NavigationStatic.NavigateToQuestionsCommand.RaiseCanExecuteChanged();
            this.BusyCount--;
        }

        private void OpenDuplicateExecute(int responseSetId)
        {
            string navigationString = string.Format(Constants.RESPONSESET_PAGE_SOURCE, responseSetId);
            navigationString += "&" + CategoryViewModel.OPEN_DUPLICATE + "=true";
            NavigationProvider.Navigate(new System.Uri(navigationString, System.UriKind.Relative));
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.searchString))
            {
                this.DisplayedResponses = this.allResponses;
            }
            else
            {
                var findedResult = this.allResponses.Where(item => item.Name.StartsWith(this.searchString) || item.Name.Contains(this.searchString));
                this.DisplayedResponses = new ObservableCollection<ResponseSet>(findedResult);
            }

            this.BusyCount--;
        }

        #endregion Methods
    }
}
