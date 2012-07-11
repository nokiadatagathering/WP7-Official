using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using NDG.BussinesLogic.SpecificLogic;
using NDG.DataAccessModels;
using NDG.DataAccessModels.Repositories;

namespace NDG.ViewModels
{
    public class SearchViewModel : ViewModel
    {
        #region Fields

        private SurveyRepository surveyRepository = new SurveyRepository();

        private SearchProcessor _searchProcessor;

        private ResponseSetRepository responsesRepository = new ResponseSetRepository();

        private ObservableCollection<Survey> surveys = new ObservableCollection<Survey>();

        private ObservableCollection<ResponseSet> savedResponses = new ObservableCollection<ResponseSet>();

        private ObservableCollection<ResponseSet> submittedResponses = new ObservableCollection<ResponseSet>();

        private string searchString = string.Empty;

        #endregion Fields

        public SearchViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);            
        }

        #region Properties

        public ObservableCollection<Survey> Surveys
        {
            get { return this.surveys; }
            set { this.surveys = value; this.RaisePropertyChanged("Surveys"); }
        }

        public ObservableCollection<ResponseSet> SavedResponses
        {
            get { return this.savedResponses; }
            set { this.savedResponses = value; this.RaisePropertyChanged("SavedResponses"); }
        }

        public ObservableCollection<ResponseSet> SubmittedResponses
        {
            get { return this.submittedResponses; }
            set { this.submittedResponses = value; this.RaisePropertyChanged("SubmittedResponses"); }
        }

        public string SearchString
        {
            get { return this.searchString; }
            set { this.searchString = value; this.RaisePropertyChanged("SearchString"); Locator.NavigationStatic.NavigateToSearchResultsCommand.RaiseCanExecuteChanged(); }
        }

        #endregion Properties

        #region Methods

        private void InitializeViewModelExecute()
        {
            this._searchProcessor = new SearchProcessor();
            this.SearchString = string.Empty;
            this.SubmittedResponses = new ObservableCollection<ResponseSet>();
            this.SavedResponses = new ObservableCollection<ResponseSet>();
            this.Surveys = new ObservableCollection<Survey>();
            this.surveyRepository = new SurveyRepository();
            this.responsesRepository = new ResponseSetRepository();
        }       

        internal void Search()
        {
            this.Surveys = new ObservableCollection<Survey>(_searchProcessor.SearchSurveysByName(this.searchString));
            this.SavedResponses = new ObservableCollection<ResponseSet>(_searchProcessor.SearchSavedResponseSetsByName(this.searchString));
            this.SubmittedResponses = new ObservableCollection<ResponseSet>(_searchProcessor.SearchSubmittedResponseSetsByName(this.searchString));
            this.BusyCount--;
        }

        #endregion Methods
    }
}
