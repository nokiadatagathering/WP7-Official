using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using NDG.DataAccessModels;
using NDG.DataAccessModels.Repositories;
using System.ComponentModel;

namespace NDG.ViewModels
{
    public class SurveyDetailsViewModel : ViewModel
    {
        #region Constants

        /// <summary>
        /// Page parameter that contain selected survey id.
        /// </summary>
        private const string SURVEY_ID = "surveyId";

        #endregion Constants

        #region Fields

        private bool isResponsesVisible = false;

        private bool isNoResponses = false;

        private ObservableCollection<ResponseSet> submittedResponses = new ObservableCollection<ResponseSet>();

        private ObservableCollection<ResponseSet> completedResponses = new ObservableCollection<ResponseSet>();

        private ObservableCollection<ResponseSet> inProgressResponses = new ObservableCollection<ResponseSet>();

        private int currentSurveyId;

        private BackgroundWorker _surveyInfoLoader;

        private SurveyRepository surveyRepository;

        private Survey currentSurvey = new Survey();

        #endregion Fields

        public SurveyDetailsViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
            this.NavigationBackCommand = new RelayCommand(this.NavigationBackExecute);
        }

        #region Properties

        public RelayCommand NavigationBackCommand { get; private set; }

        public ObservableCollection<ResponseSet> SubmittedResponses
        {
            get
            {
                return this.submittedResponses;
            }

            set
            {
                this.submittedResponses = value;
                this.RaisePropertyChanged("SubmittedResponses");
            }
        }

        public ObservableCollection<ResponseSet> CompletedResponses
        {
            get { return this.completedResponses; }
            set { this.completedResponses = value; this.RaisePropertyChanged("CompletedResponses"); }
        }

        public ObservableCollection<ResponseSet> InProgressResponses
        {
            get { return this.inProgressResponses; }
            set { this.inProgressResponses = value; this.RaisePropertyChanged("InProgressResponses"); }
        }

        public bool IsNoResponses
        {
            get { return this.isNoResponses; }
            set { this.isNoResponses = value; this.RaisePropertyChanged("IsNoResponses"); }
        }

        public bool IsResponsesVisible 
        {
            get { return this.isResponsesVisible; } 
            set { this.isResponsesVisible = value; this.RaisePropertyChanged("IsResponsesVisible"); } 
        }

        public bool IsPageLoaded { get; set; }

        public Survey CurrentSurvey
        {
            get
            {
                return this.currentSurvey;
            }

            private set
            {
                this.currentSurvey = value;
                this.RaisePropertyChanged("CurrentSurvey");
            }
        }

        #endregion Properties

        private void InitializeViewModelExecute()
        {
            this.BusyCount++;
            this.IsNoResponses = false;
            this.IsResponsesVisible = false;
            Locator.NavigationStatic.NavigateToQuestionsCommand.RaiseCanExecuteChanged();

            _surveyInfoLoader = new BackgroundWorker();
            this.CurrentSurvey = new Survey();
            this.SubmittedResponses = new ObservableCollection<ResponseSet>();
            this.InProgressResponses = new ObservableCollection<ResponseSet>();
            this.CompletedResponses = new ObservableCollection<ResponseSet>();
            this.surveyRepository = new SurveyRepository();
            var pageParameters = NavigationProvider.GetNavigationParameters();

            if (pageParameters.ContainsKey(SURVEY_ID))
            {
                this.currentSurveyId = int.Parse(pageParameters[SURVEY_ID]);

                this.CurrentSurvey = surveyRepository.GetSurveyByID(this.currentSurveyId);
            }

            _surveyInfoLoader.DoWork += new DoWorkEventHandler(LoadSurveyInformation);
            this.IsNoResponses = this.CurrentSurvey.ResponseSet.Count == 0;
            this.IsResponsesVisible = !this.IsNoResponses;
            _surveyInfoLoader.RunWorkerAsync();
        }

        void LoadSurveyInformation(object sender, DoWorkEventArgs e)
        {
            var q = new ObservableCollection<ResponseSet>(this.surveyRepository.GetSubmittedResponseSets(currentSurveyId));
            var w = new ObservableCollection<ResponseSet>(this.surveyRepository.GetCompletedResponseSets(currentSurveyId));
            var r = new ObservableCollection<ResponseSet>(this.surveyRepository.GetInProgressResponseSets(currentSurveyId));
            SyncContext.Post((s) =>
            {
                this.SubmittedResponses = q;
                this.CompletedResponses = w;
                this.InProgressResponses = r;
                this.IsNoResponses = this.CompletedResponses.Count == 0 && this.SubmittedResponses.Count == 0 && InProgressResponses.Count == 0;
                this.IsResponsesVisible = !this.IsNoResponses;
                this.BusyCount--;
                Locator.NavigationStatic.NavigateToQuestionsCommand.IsExecutedNow = false;
            }, null);
        }

        private void NavigationBackExecute()
        {
            if (_surveyInfoLoader != null && _surveyInfoLoader.IsBusy)
                _surveyInfoLoader.CancelAsync();
            this.surveyRepository.Dispose();
            this.CurrentSurvey = new Survey();
            this.SubmittedResponses = new ObservableCollection<ResponseSet>();
            this.InProgressResponses = new ObservableCollection<ResponseSet>();
            this.CompletedResponses = new ObservableCollection<ResponseSet>();

        }

        internal void Refresh()
        {
            this.BusyCount++;
            this.surveyRepository = new SurveyRepository();
            this.CurrentSurvey = surveyRepository.GetSurveyByID(this.currentSurveyId);
            this.SubmittedResponses = new ObservableCollection<ResponseSet>(this.surveyRepository.GetSubmittedResponseSets(currentSurveyId));
            this.CompletedResponses = new ObservableCollection<ResponseSet>(this.surveyRepository.GetCompletedResponseSets(currentSurveyId));
            this.InProgressResponses = new ObservableCollection<ResponseSet>(this.surveyRepository.GetInProgressResponseSets(currentSurveyId));
            this.IsNoResponses = this.CompletedResponses.Count == 0 && this.SubmittedResponses.Count == 0 && InProgressResponses.Count == 0;
            this.IsResponsesVisible = !this.IsNoResponses;
            this.BusyCount--;
        }
    }
}
