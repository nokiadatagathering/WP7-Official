using System;
using System.Collections.ObjectModel;
using NDG.Helpers.Classes;
using GalaSoft.MvvmLight.Command;
using NDG.DataAccessModels;
using NDG.DataAccessModels.Repositories;
using System.Linq;
using NDG.DataAccessModels.DataModels;
using System.Collections;
using NDG.BussinesLogic.Governors;
using System.Windows;
using Coding4Fun.Phone.Controls;
using NDG.Helpers.Controls;
using System.ComponentModel;
using NDG.ViewModels.Helpers;
using NDG.BussinesLogic.Providers;
using NDG.Common;

namespace NDG.ViewModels
{
    public enum QuestionPageIndexes
    {
        Questions,
        TableOfContents
    }

    public class CategoryViewModel : ViewModel
    {
        #region Constants

        private string UPLOAD_SERVICE_TEXT = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).UPLOAD_CONFIRMATION;
        private string NETWORK_UNAVAILABLE_TEXT = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).NDG_NETWORK_UNAVAILABLE;
        internal const string OPEN_DUPLICATE = "isDuplicate";
        private string SUCCESSFULLY_UPLOADED = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).SUBMITTED_SUCCESS;
        private  string NOT_COMPLETED_RESPONSE_TEXT = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).INCOMPLETE_UPLOAD;
        private  string ERROR_DURING_UPLOADING = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).SEND_ERRORS;
        private const string SURVEY_ID_STRING = "surveyId";
        private const string RESPONSESET_ID_STRING = "responseSetId";

        #endregion Constants

        #region Fields

        private bool isSaveButtonVisible = true;
        private bool isPageEnabled = true;
        System.Collections.Generic.Dictionary<string, string> pageParameters = null;
        private BackgroundWorker categoriesWorker;
        private bool isSaveStarted = false;
        private QuestionPageIndexes currentPageIndex;
        internal int currentResponsesSetId = 0;
        private int allItemsCount = 0;
        private string responseSetName = string.Empty;
        private bool isQuestionsEmpty = false;
        private InnerIndexes selectedIndexes;
        internal ResponseSetGovernor responseSetGovernor = new ResponseSetGovernor();
        CategoryRepository categoryRepository = new CategoryRepository();
        ResponseSetRepository responseSetRepository = new ResponseSetRepository();
        SurveyRepository surveyRepository = new SurveyRepository();
        Survey currentSurvey = new Survey();
        internal ResponseSet currentResponseSet = new ResponseSet();
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        
        #endregion Fields

        public CategoryViewModel()
        {
            this.SaveResponseSetCommand = new RelayCommand(this.SaveResponseSetExecute, this.SaveResponseSetCanExecute);
            this.SetSelectedIndexesCommand = new RelayCommand<object>(this.SetSelectedIndexesExecute);
            this.UploadToServiceCommand = new RelayCommand<ResponseSet>(this.UploadToServiceExecute);
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
            this.NavigationBackCommand = new RelayCommand(this.NavigationBackExecute);
            this.SummarySelectionDelegate = new Func<IList, string>((items) =>
                {
                    string result = string.Empty;
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (i != items.Count - 1)
                        {
                            result += (items[i] as ChoiceTextValuePair).Text + ", ";
                        }
                        else
                        {
                            result += (items[i] as ChoiceTextValuePair).Text;
                        }
                    }

                    return result;
                });
        }

        #region Properties

        public bool IsSaveButtonVisible
        {
            get { return this.isSaveButtonVisible; }
            set { this.isSaveButtonVisible = value; this.RaisePropertyChanged("IsSaveButtonVisible"); }
        }

        public bool IsPageEnabled { get { return this.isPageEnabled; } set { this.isPageEnabled = value; this.RaisePropertyChanged("IsPageEnabled"); } }

        public RelayCommand NavigationBackCommand { get; private set; }

        public RelayCommand SaveResponseSetCommand { get; private set; }

        public RelayCommand<object> SetSelectedIndexesCommand { get; private set; }

        public RelayCommand<ResponseSet> UploadToServiceCommand { get; private set; }

        public int AllItemsCount
        {
            get { return this.allItemsCount; }
            set { this.allItemsCount = value; this.RaisePropertyChanged("AllItemsCount"); }
        }

        public QuestionPageIndexes CurrentPageIndex
        {
            get { return this.currentPageIndex; }
            set { this.currentPageIndex = value; this.RaisePropertyChanged("CurrentPageIndex"); }
        }

        public InnerIndexes SelectedIndexes
        {
            get { return this.selectedIndexes; }
            set { this.selectedIndexes = value; this.RaisePropertyChanged("SelectedIndexes"); }
        }

        public string ResponseSetName
        {
            get
            {
                if (Locator.NavigationStatic.NavigateToSaveResponsesCommand.IsExecutedNow)
                {
                    Locator.NavigationStatic.NavigateToSaveResponsesCommand.IsExecutedNow = false;
                }

                return this.responseSetName;
            }

            set
            {
                this.responseSetName = value;
                this.RaisePropertyChanged("ResponseSetName");
                this.SaveResponseSetCommand.RaiseCanExecuteChanged();
            }
        }

        public Func<IList, string> SummarySelectionDelegate { get; private set; }

        public bool IsQuestionsEmpty
        {
            get { return this.isQuestionsEmpty; }
            set { this.isQuestionsEmpty = value; this.RaisePropertyChanged("IsQuestionsEmpty"); }
        }

        public Survey CurrentSurvey
        {
            get { return this.currentSurvey; }
            private set { this.currentSurvey = value; this.RaisePropertyChanged("CurrentSurvey"); }
        }

        public ObservableCollection<Category> Categories
        {
            get { return this.categories; }
            set { this.categories = value; this.RaisePropertyChanged("Categories"); }
        }

        #endregion Properties

        private void SetSelectedIndexesExecute(object selectedItem)
        {
            if (selectedItem is Category)
            {
                var selectedCategory = selectedItem as Category;
                this.selectedIndexes.MainIndex = this.categories.IndexOf(selectedCategory);
                this.selectedIndexes.InnerIndex = 0;
            }
            else
            {
                var selectedQuestion = selectedItem as Question;
                var selectedCategory = this.categories.SingleOrDefault(item => item.Question.Contains(selectedQuestion));
                this.selectedIndexes.MainIndex = this.categories.IndexOf(selectedCategory);
                this.selectedIndexes.InnerIndex = selectedCategory.Question.IndexOf(selectedQuestion);
            }

            this.SelectedIndexes = this.selectedIndexes;
            this.CurrentPageIndex = QuestionPageIndexes.Questions;
        }

        private void InitializeViewModelExecute()
        {
            this.BusyCount++;
            this.IsSaveButtonVisible = true;
            Locator.NavigationStatic.NavigateToSaveResponsesCommand.RaiseCanExecuteChanged();
            this.categoriesWorker = new BackgroundWorker();
            this.CurrentPageIndex = QuestionPageIndexes.Questions;
            this.CurrentSurvey = new Survey();
            this.currentResponsesSetId = 0;
            this.Categories = new ObservableCollection<Category>();
            this.surveyRepository = new SurveyRepository();
            this.categoryRepository = new CategoryRepository();
            this.responseSetRepository = new ResponseSetRepository();
            pageParameters = NavigationProvider.GetNavigationParameters();
            categoriesWorker.DoWork += this.LoadContent;
            categoriesWorker.RunWorkerAsync();
        }

        private void LoadContent(object sender, DoWorkEventArgs e)
        {          
            if (pageParameters != null && pageParameters.ContainsKey(SURVEY_ID_STRING))
            {
                int surveyId = int.Parse(pageParameters[SURVEY_ID_STRING]);
                this.currentSurvey = this.surveyRepository.GetSurveyByID(surveyId);
                this.categories = new ObservableCollection<Category>(this.currentSurvey.Category);
            }

            if (pageParameters != null && pageParameters.ContainsKey(RESPONSESET_ID_STRING))
            {
                this.currentResponsesSetId = int.Parse(pageParameters[RESPONSESET_ID_STRING]);
                this.currentSurvey = this.surveyRepository.GetSurveyByResponseSetID(currentResponsesSetId);
                this.currentResponseSet = this.responseSetRepository.GetResponseSetForUserByID(this.currentResponsesSetId);
                this.isSaveButtonVisible = !this.currentResponseSet.IsSubmitted;
                this.categories = new ObservableCollection<Category>(this.currentSurvey.Category);                
            }

            if (pageParameters != null && pageParameters.ContainsKey(OPEN_DUPLICATE))
            {
                this.isSaveButtonVisible = bool.Parse(pageParameters[OPEN_DUPLICATE]);
            }

            SyncContext.Post((parameter) =>
            {
                responseSetGovernor.PopulateCategoriesQuestionsWithResponseSetAnswers(this.categories, currentResponsesSetId);

                var questionsCollection = categories.SelectMany(c => c.Question);
                this.AllItemsCount = questionsCollection.Count();

                foreach (var question in questionsCollection)
                {
                    question.UpdateDependentQuestionsData();
                }

                this.IsSaveButtonVisible = this.isSaveButtonVisible;
                this.CurrentSurvey = this.currentSurvey;
                this.Categories = this.categories;
                this.BusyCount--;
                Locator.NavigationStatic.NavigateToSaveResponsesCommand.RaiseCanExecuteChanged();
            }, null);
        }

        private void SaveResponseSetExecute()
        {
            this.IsPageEnabled = false;
            this.isSaveStarted = true;
            this.SaveResponseSetCommand.RaiseCanExecuteChanged();
            this.currentResponseSet = this.responseSetGovernor.CreateNewResponseSetWithAnswers(Categories, CurrentSurvey.ID, this.ResponseSetName);
            this.currentResponsesSetId = this.currentResponseSet.ID;
            NavigationProvider.RootFrame.RemoveBackEntry();
            this.UploadToService();
        }

        internal void UploadToService()
        {            
            this.BusyCount++;
            Locator.NavigationStatic.NavigateToSaveResponsesCommand.IsExecutedNow = false;
            ConfirmationBox box = new ConfirmationBox();

            string message = string.Empty;
            if (this.currentResponseSet.IsCompleted)
            {
                message = UPLOAD_SERVICE_TEXT;
            }
            else
            {
                message = NOT_COMPLETED_RESPONSE_TEXT;
            }


            box.Message = message;
            box.DialogCompleted += this.OnDialogCompleted;
            box.Show();
        }

        private void OnDialogCompleted(object sender, ConfirmationResulEventArgs e)
        {
            NavigationProvider.RootFrame.DisableCurrentPage();
            if (e.DialogResult == PopUpResult.Ok)
            {
                if (InternetChecker.IsInernetActive)
                {
                    this.BusyCount++;
                    this.IsPageEnabled = false;
                    this.responseSetGovernor.UploadResponseSetToServer(this.currentResponsesSetId, this.OnResponsesUploaded);
                }
                else
                {
                    MessageBox.Show(NETWORK_UNAVAILABLE_TEXT);
                    NavigateFromQuestions();
                    this.IsPageEnabled = true;
                }
            }

            if (e.DialogResult == PopUpResult.NoResponse)
            {
                NavigateFromQuestions();
                this.IsPageEnabled = true;
            }
            else if (e.DialogResult == PopUpResult.UserDismissed)
            {
                this.IsPageEnabled = true;
            }
                        
            this.isSaveStarted = false;
            this.BusyCount--;
        }

        private void OnResponsesUploaded(bool result)
        {
            SyncContext.Post((parameter) => { this.BusyCount--; NavigationProvider.RootFrame.EnableCurrenPage(); }, null);
            string message = string.Empty;
            if (result)
            {
                message = SUCCESSFULLY_UPLOADED;
            }
            else
            {
                message = ERROR_DURING_UPLOADING;
            }

            SyncContext.Post((parameter) =>
            {
                this.IsPageEnabled = true;
                var messageBoxResult = MessageBox.Show(message);
                if (messageBoxResult != MessageBoxResult.None)
                {
                    NavigateFromQuestions();
                }
            },
            null);
        }

        private void NavigateFromQuestions()
        {
            string navigationSource = string.Format(Constants.SURVYES_DETAILS_SOURCE, this.currentSurvey.ID);
            NavigationProvider.NavigateAndRemoveBackEntries(new Uri(navigationSource, UriKind.Relative));
        }

        private bool SaveResponseSetCanExecute()
        {
            return !string.IsNullOrEmpty(this.responseSetName) && !string.IsNullOrEmpty(this.responseSetName.Trim())
                && !isSaveStarted;
        }

        private void UploadToServiceExecute(ResponseSet selectedResponseSet)
        {
            if (InternetChecker.IsInernetActive)
            {
                if (!selectedResponseSet.IsCompleted)
                {
                    var messageBoxResult = MessageBox.Show(NOT_COMPLETED_RESPONSE_TEXT, string.Empty, MessageBoxButton.OKCancel);
                    if (messageBoxResult == MessageBoxResult.OK)
                    {
                        this.BusyCount++;
                        if (selectedResponseSet.ID == this.currentResponsesSetId)
                        {
                            this.IsPageEnabled = false;
                        }

                        this.responseSetGovernor.UploadResponseSetToServer(selectedResponseSet.ID, this.OnUploadToServiceCompleted);
                    }
                }
                else
                {
                    this.BusyCount++;
                    if (selectedResponseSet.ID == this.currentResponsesSetId)
                    {
                        this.IsPageEnabled = false;
                    }

                    this.responseSetGovernor.UploadResponseSetToServer(selectedResponseSet.ID, this.OnUploadToServiceCompleted);
                }
            }
            else
            {

                MessageBox.Show(NETWORK_UNAVAILABLE_TEXT);
                this.IsPageEnabled = true;
                Locator.SavedResponsesStatic.RefresheExecute();
                Locator.SubmittedResponsesStatic.RefresheExecute();
                if (NavigationProvider.CurrentSource.OriginalString.Contains(Constants.SURVEY_DETAILS_PAGE))
                {
                    Locator.SurveyDetailsStatic.Refresh();
                }
            }

            Locator.NavigationStatic.NavigateToQuestionsCommand.RaiseCanExecuteChanged();
        }

        private void OnUploadToServiceCompleted(bool result)
        {
            SyncContext.Post((parameter) => { this.BusyCount--; }, null);
            string message = string.Empty;
            if (result)
            {
                message = SUCCESSFULLY_UPLOADED;
            }
            else
            {
                message = ERROR_DURING_UPLOADING;
            }

            SyncContext.Post((parameter) =>
            {
                MessageBox.Show(message);
            }, null);

            SyncContext.Post((parameter) =>
            {
                this.IsPageEnabled = true;
                Locator.SavedResponsesStatic.RefresheExecute();
                Locator.SubmittedResponsesStatic.RefresheExecute();
                if (NavigationProvider.CurrentSource.OriginalString.Contains(Constants.SURVEY_DETAILS_PAGE))
                {
                    Locator.SurveyDetailsStatic.Refresh();
                }

                Locator.NavigationStatic.NavigateToQuestionsCommand.RaiseCanExecuteChanged();
            }, null);
        }

        private void NavigationBackExecute()
        {
            this.ResponseSetName = string.Empty;
            this.SaveResponseSetCommand.RaiseCanExecuteChanged();
            this.surveyRepository.Dispose();
            this.categoryRepository.Dispose();
            this.responseSetRepository.Dispose();
            this.Categories = new ObservableCollection<Category>();
            this.CurrentSurvey = new Survey();
            this.ResponseSetName = string.Empty;
        }
    }
}
