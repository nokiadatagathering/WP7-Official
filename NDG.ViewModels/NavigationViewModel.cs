using System;
using GalaSoft.MvvmLight.Command;
using NDG.Common;
using NDG.ViewModels.Helpers;

namespace NDG.ViewModels
{
    public class NavigationViewModel : ViewModel
    {
        public NavigationViewModel()
        {
            this.NavigateToSurveyDetailsCommand = new RelayCommand<int>(this.NavigateToSurveyDetailsExecute);                        
            this.NavigateToTableOfContentsCommand = new RelayCommand(this.NavigateToTableOfContentsExecute);            
            this.NavigateToResponseSetAnswersCommand = new RelayCommand<int>(this.NavigateToResponseSetAnswersExecute);
            this.NavigateToSaveResponsesCommand = new WaitingCommand(this.NavigateToSaveResponsesExecute, this.NavigateToSaveResponsesCanExecute);
            this.NavigateToQuestionsCommand = new WaitingCommand<int>(this.NavigateToQuestionsExecute, this.NavigateToQuestionsCanExecute);
            this.NavigateToFilterResultCommand = new RelayCommand(this.NavigateToFilterResultExecute, this.NavigateToFilterResultCanExecute);
            this.NavigateToSearchCommand = new RelayCommand(this.NavigateToSearchExecute);
            this.NavigateToAboutCommand = new RelayCommand(this.NavigateToAboutExecute);
            this.NavigateToFilterCommand = new WaitingCommand(this.NavigateToFilterExecute, this.NavigateToFilterCanExecute);
            this.NavigateToMySurveysCommand = new RelayCommand(this.NavigateToMySurveysExecute);
            this.NavigateToSearchResultsCommand = new RelayCommand(this.NavigateToSearchResultsExecute, this.NavigateToSearchResultsCanExecute);
            this.NavigateToSettingsCommand = new RelayCommand(this.NavigateToSettingsExecute);
        }

        #region Commands

        public RelayCommand NavigateToSettingsCommand { get; private set; }
        public RelayCommand<int> NavigateToSurveyDetailsCommand { get; private set; }
        public RelayCommand NavigateToFilterResultCommand { get; private set; }
        public RelayCommand<int> NavigateToResponseSetAnswersCommand { get; private set; }        
        public RelayCommand NavigateToTableOfContentsCommand { get; private set; }
        public WaitingCommand<int> NavigateToQuestionsCommand { get; private set; }
        public WaitingCommand NavigateToSaveResponsesCommand { get; private set; }
        public RelayCommand NavigateToAboutCommand { get; private set; }
        public RelayCommand NavigateToSearchCommand { get; private set; }
        public RelayCommand NavigateToMySurveysCommand { get; private set; }
        public WaitingCommand NavigateToFilterCommand { get; private set; }
        public RelayCommand NavigateToSearchResultsCommand { get; private set; }

        #endregion Commands

        #region Executes

        private void NavigateToSettingsExecute()
        {
            NavigationProvider.Navigate(new Uri(Constants.SETTINGS_PAGE, UriKind.Relative));
        }

        private void NavigateToSearchResultsExecute()
        {
            NavigationProvider.Navigate(new Uri(Constants.SEARCH_RESULTS_PAGE_SOURCE, UriKind.Relative));
            Locator.SearchStatic.BusyCount++;
            Locator.SearchStatic.Search();
        }

        private void NavigateToSurveyDetailsExecute(int surveyId)
        {
            NavigationProvider.Navigate(new Uri(string.Format(Constants.SURVYES_DETAILS_SOURCE, surveyId), UriKind.Relative));
        }

        private void NavigateToFilterExecute()
        {
            NavigationProvider.Navigate(new Uri(Constants.FILTER_PAGE_SOURCE, UriKind.Relative));
        }

        private void NavigateToMySurveysExecute()
        {
            NavigationProvider.Navigate(new System.Uri(Constants.SEARCH_MY_SURVEYS_SOURCE, System.UriKind.Relative));
        }

        private void NavigateToSearchExecute()
        {
            NavigationProvider.Navigate(new Uri(Constants.SEARCH_PAGE_SOURCE, UriKind.Relative));
        }

        private void NavigateToAboutExecute()
        {
            NavigationProvider.Navigate(new Uri(Constants.ABOUT_PAGE_SOURCE, UriKind.Relative));
        }

        private void NavigateToResponseSetAnswersExecute(int responseSetId)
        {
            NavigationProvider.Navigate(new Uri(string.Format(Constants.RESPONSESET_PAGE_SOURCE, responseSetId), UriKind.Relative));
        }

        private void NavigateToTableOfContentsExecute()
        {
            NavigationProvider.Navigate(new Uri(Constants.TABLE_OF_CONTENTS_SOURCE, UriKind.Relative));
        }

        private void NavigateToQuestionsExecute(int surveyId)
        {
            NavigationProvider.Navigate(new Uri(Constants.CATEGORIES_PAGE_SOURCE + surveyId, UriKind.Relative));
        }

        private void NavigateToSaveResponsesExecute()
        {
            if (Locator.CategoryStatic.currentResponsesSetId == 0 || Locator.CategoryStatic.currentResponseSet.IsSubmitted)
            {
                Locator.CategoryStatic.SaveResponseSetCommand.RaiseCanExecuteChanged();
                Locator.CategoryStatic.ResponseSetName = string.Empty;
                NavigationProvider.Navigate(new Uri(Constants.SAVE_RESPONSES_PAGE_SOURCE, UriKind.Relative));
            }
            else
            {
                var responseSet = Locator.CategoryStatic.responseSetGovernor.UpdateResponseSetWithAnswers(Locator.CategoryStatic.Categories, Locator.CategoryStatic.currentResponsesSetId);
                Locator.CategoryStatic.UploadToService();
            }            
        }

        private void NavigateToFilterResultExecute()
        {
            FilterParameters parameters = new FilterParameters();

            switch (Locator.FilterResponsesStatic.CurrentFilter)
            {
                case FilterPages.FilterByDate:
                    parameters.Type = FilterType.ByDate;
                    parameters.Date = new DateParameters()
                    {
                        SelectedDate = Locator.FilterResponsesStatic.SelectedDate,
                        SelectedStartDate = Locator.FilterResponsesStatic.SelectedStartDate,
                        SelectedEndDate = Locator.FilterResponsesStatic.SelectedEndDate,
                        SelectedPeriod = Locator.FilterResponsesStatic.SelectedPeriod
                    };
                    break;
                case FilterPages.FilterByLocation:
                    if (Locator.FilterResponsesStatic.IsFilterByGps)
                    {
                        parameters.Type = FilterType.ByGps;
                    }
                    else
                    {
                        parameters.Type = FilterType.ByAddress;
                        parameters.Address = new AddressParameters()
                        {
                            City = Locator.FilterResponsesStatic.City,
                            State = Locator.FilterResponsesStatic.State,
                            StreetAddress = Locator.FilterResponsesStatic.StreetAddress
                        };
                    }
                    break;
            }

            NavigationProvider.NavigationParameters.Add(Constants.FILTER_KEY, parameters);
            NavigationProvider.Navigate(new Uri(Constants.FILER_RESULT_PAGE_SOURCE, UriKind.Relative));
        }

        #endregion Executes

        #region CanExecutes

        private bool NavigateToFilterCanExecute()
        {
            return !Locator.SavedResponsesStatic.IsBusy && !Locator.SubmittedResponsesStatic.IsBusy;
        }

        private bool NavigateToQuestionsCanExecute(int parameter)
        {
            return !Locator.SurveyDetailsStatic.IsBusy;
        }

        private bool NavigateToFilterResultCanExecute()
        {
            bool canNavigate = Locator.FilterResponsesStatic.CurrentFilter == FilterPages.FilterByDate
                && Locator.FilterResponsesStatic.SelectedPeriod != null
                && !string.IsNullOrEmpty(Locator.FilterResponsesStatic.SelectedPeriod.Value);
            if (!canNavigate)
            {
                if (Locator.FilterResponsesStatic.IsFilterByAddress)
                {
                    canNavigate = !string.IsNullOrWhiteSpace(Locator.FilterResponsesStatic.StreetAddress) && !string.IsNullOrWhiteSpace(Locator.FilterResponsesStatic.City)
                        && !string.IsNullOrWhiteSpace(Locator.FilterResponsesStatic.State);
                }
                else
                {
                    canNavigate = Locator.FilterResponsesStatic.IsFilterByGps;
                }
            }

            return canNavigate;
        }

        private bool NavigateToSearchResultsCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Locator.SearchStatic.SearchString);
        }

        private bool NavigateToSaveResponsesCanExecute()
        {
            return !Locator.CategoryStatic.IsBusy;
        }

        #endregion CanExecutes
    }
}
