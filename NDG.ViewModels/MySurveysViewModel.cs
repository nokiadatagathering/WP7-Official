// ----------------------------------------------------------------------
// <copyright file="MySurveysViewModel.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using GalaSoft.MvvmLight.Command;
    using NDG.BussinesLogic;
    using NDG.BussinesLogic.Providers;
    using NDG.DataAccessModels;
    using NDG.DataAccessModels.Repositories;
    using NDG.ViewModels.Helpers;
    using NDG.Common;

    /// <summary>
    /// View model for my surveys.
    /// </summary>
    public class MySurveysViewModel : ViewModel
    {
        #region Constants

        private string NETWORK_UNAVAILABLE_TEXT = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).NDG_NETWORK_UNAVAILABLE;

        private const string HOME_PAGE_STRING = "HomePage";

        public const string IS_FOR_SEARCHING = "isSearch";

        public const int TOP_SURVYES_COUNT = 5;

        #endregion Constants

        #region Fields

        private DeferredSearchHelper searchHelper;

        private ObservableCollection<Survey> topSurveys = new ObservableCollection<Survey>();

        private bool isUpdateStarted = false;

        private ISurveyRepository _surveyRepository;

        /// <summary>
        /// Seraching text.
        /// </summary>
        private string searchText = string.Empty;

        /// <summary>
        /// Collection of all surveyses.
        /// </summary>
        private ObservableCollection<Survey> allSurveyses = new ObservableCollection<Survey>();

        /// <summary>
        /// Collection of displayed surveyses at the home page.
        /// </summary>
        private ObservableCollection<Survey> displayedSurveyses = new ObservableCollection<Survey>();

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the MySurveysViewModel class.
        /// </summary>
        public MySurveysViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
            this.NavigationBackCommand = new RelayCommand(this.NavigationBackExecute);
            this.DeleteSurveyCommand = new RelayCommand<Survey>(this.DeleteSurveyExecute);
            this.searchHelper = new DeferredSearchHelper("SearchText", this, this.SearchSurveys);
            _surveyRepository = new SurveyRepository();
        }

        #region Properties       

        public RelayCommand NavigationBackCommand { get; private set; }

        public RelayCommand<Survey> DeleteSurveyCommand { get; private set; }

        /// <summary>
        /// Gets or sets search string.
        /// </summary>
        public string SearchText
        {
            get
            {
                return this.searchText;
            }

            set
            {
                this.searchText = value;
                this.RaisePropertyChanged("SearchText");
            }
        }

        /// <summary>
        /// Gets displayed 
        /// </summary>
        public ObservableCollection<Survey> DisplayedSurveyses
        {
            get
            {
                return this.displayedSurveyses;
            }

            private set
            {
                this.displayedSurveyses = value;
                this.RaisePropertyChanged("DisplayedSurveyses");
            }
        }

        public ObservableCollection<Survey> TopSurveys
        {
            get { return this.topSurveys; }
            set { this.topSurveys = value; this.RaisePropertyChanged("TopSurveys"); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes initialization of view model.
        /// </summary>
        private void InitializeViewModelExecute()
        {
            this.allSurveyses = new ObservableCollection<Survey>();
            this.searchHelper.StartSearch();
            this.SearchText = string.Empty;
            PopulateSurveys();       
        }

        private void PopulateSurveys()
        {
            if (NavigationProvider.CurrentSource.OriginalString.Contains(HOME_PAGE_STRING) && this.allSurveyses == null || this.allSurveyses.Count == 0)
            {
                this.allSurveyses = new ObservableCollection<Survey>(_surveyRepository.GetAllUserSurveys(Membership.CurrentUser.ID));
            }

            var pageParameters = NavigationProvider.GetNavigationParameters();
            if (pageParameters.ContainsKey(IS_FOR_SEARCHING))
            {
                this.DisplayedSurveyses = this.allSurveyses;
                this.SearchSurveys();
            }
            else
            {
                this.TopSurveys = new ObservableCollection<Survey>(this._surveyRepository.GetUserTopSurveys(TOP_SURVYES_COUNT, Membership.CurrentUser.ID));
            }
        }

        private void SearchSurveys()
        {
            if (string.IsNullOrEmpty(this.searchText))
            {
                this.DisplayedSurveyses = this.allSurveyses;
            }
            else
            {
                var findedResults = this.allSurveyses.Where(item => item.Name.StartsWith(this.searchText, System.StringComparison.OrdinalIgnoreCase) || item.Name.Contains(this.searchText));
                this.DisplayedSurveyses = new ObservableCollection<Survey>(findedResults);
            }

            this.BusyCount--;
        }

        internal bool RefreshCanExecute()
        {
            return !this.isUpdateStarted;
        }

        //TODO: implement private field for Governor with proper initialization
        internal void RefreshExecute()
        {
            if (InternetChecker.IsInernetActive)
            {
                ISurveyGovernor sGov = new SurveyGovernor();
                this.BusyCount++;
                this.isUpdateStarted = true;
                Locator.HomeStatic.RefreshCommand.RaiseCanExecuteChanged();
                sGov.GetNewSurveys(SurveysReceived);
            }
            else
            {
                MessageBox.Show(NETWORK_UNAVAILABLE_TEXT);
            }
        }

        private void SurveysReceived(IEnumerable<Survey> res)
        {
            SyncContext.Post((parameter) => { this.BusyCount--; this.isUpdateStarted = false; Locator.HomeStatic.RefreshCommand.RaiseCanExecuteChanged(); }, null);
            if (res!=null && res.Count() > 0)
            {
                this._surveyRepository.AddNewSurveyCollectionForUser(res, Membership.CurrentUser.ID);
                foreach (var item in res)
                {
                    SyncContext.Post((parameter) =>
                    {
                        this.allSurveyses.Add(item);
                        this.displayedSurveyses.Add(item);
                    }, null);
                }

                var getedTop = this._surveyRepository.GetUserTopSurveys(TOP_SURVYES_COUNT, Membership.CurrentUser.ID);

                SyncContext.Post((parameter) => { this.TopSurveys = new ObservableCollection<Survey>(getedTop); }, null);
            }

#if UNIT_TEST
            RaiseTestCompleted("Surveys_refresh");
#endif
        }

        private void NavigationBackExecute()
        {
            //  this._surveyRepository.Dispose();
            this.searchHelper.StopSearch();
            this.DisplayedSurveyses = new ObservableCollection<Survey>();
            this.SearchText = string.Empty;
        }

        private void DeleteSurveyExecute(Survey selectedSurvey)
        {
            if (_surveyRepository.DeleteSurvey(selectedSurvey.ID))
            {
                MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).SURVEY_DELETED);
                this.allSurveyses = new ObservableCollection<Survey>(_surveyRepository.GetAllUserSurveys(Membership.CurrentUser.ID));
                this.TopSurveys = new ObservableCollection<Survey>(_surveyRepository.GetUserTopSurveys(TOP_SURVYES_COUNT, Membership.CurrentUser.ID));
                this.DisplayedSurveyses.Remove(selectedSurvey);
                Locator.SubmittedResponsesStatic.RefresheExecute();
                Locator.SavedResponsesStatic.RefresheExecute();
            }
            else
            {
                MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).ERROR_SURVEY_DELETE);
            }
        }
        #endregion Methods
    }
}
