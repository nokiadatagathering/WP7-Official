// ----------------------------------------------------------------------
// <copyright file="Locator.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
using NDG.Helpers.Classes;
using NDG.DataAccessModels.Repositories;
using System.IO.IsolatedStorage;
using System.Windows;
namespace NDG.ViewModels
{
    /// <summary>
    /// Provides access to all view models like ot properties (static or not)
    /// </summary>
    public class Locator
    {        
        #region StaticFields

        private static SettingsViewModel settingsStatic;

        private static NavigationViewModel navigationStatic;

        private static SearchViewModel searchStatic;

        private static FilterResultsViewModel filterResultStatic;

        private static FilterResponsesViewModel filterResponsesStatic;

        private static CategoryViewModel categoryStatic;

        private static ResponseSetsViewModel submittedResponsesStatic;

        private static ResponseSetsViewModel savedResponsesStatic;

        /// <summary>
        /// Stores an instance of the login view model.
        /// </summary>
        private static LoginViewModel loginViewStatic;

        /// <summary>
        /// Stores an instance of the my surveys view model.
        /// </summary>
        private static MySurveysViewModel mySurveysStatic;

        /// <summary>
        /// Stores an instance of the home view model.
        /// </summary>
        private static HomeViewModel homeViewStatic;

        /// <summary>
        /// Stores an instance of the servey detail view model.
        /// </summary>
        private static SurveyDetailsViewModel surveyDetailsStatic;

        #endregion StaticFields

        #region StaticProperties

        public static SettingsViewModel SettingsStatic
        {
            get
            {
                if (settingsStatic == null)
                {
                    settingsStatic = new SettingsViewModel();
                }

                return settingsStatic;
            }
        }

        public static NavigationViewModel NavigationStatic
        {
            get
            {
                if (navigationStatic == null)
                {
                    navigationStatic = new NavigationViewModel();
                }

                return navigationStatic;
            }
        }

        public static SearchViewModel SearchStatic
        {
            get
            {
                if (searchStatic == null)
                {
                    searchStatic = new SearchViewModel();
                }

                return searchStatic;
            }
        }

        public static FilterResultsViewModel FilterResultStatic
        {
            get
            {
                if (filterResultStatic == null)
                {
                    filterResultStatic = new FilterResultsViewModel();
                }

                return filterResultStatic;
            }
        }

        public static FilterResponsesViewModel FilterResponsesStatic
        {
            get
            {
                if (filterResponsesStatic == null)
                {
                    filterResponsesStatic = new FilterResponsesViewModel();
                }

                return filterResponsesStatic;
            }
        }

        public static CategoryViewModel CategoryStatic
        {
            get
            {
                if (categoryStatic == null)
                {
                    categoryStatic = new CategoryViewModel();
                }

                return categoryStatic;
            }
        }

        public static ResponseSetsViewModel SubmittedResponsesStatic
        {
            get
            {
                if (submittedResponsesStatic == null)
                {
                    submittedResponsesStatic = new ResponseSetsViewModel(ResponseSetsType.Submitted);
                }

                return submittedResponsesStatic;
            }
        }

        public static ResponseSetsViewModel SavedResponsesStatic
        {
            get
            {
                if (savedResponsesStatic == null)
                {
                    savedResponsesStatic = new ResponseSetsViewModel(ResponseSetsType.Saved);
                }

                return savedResponsesStatic;
            }
        }

        /// <summary>
        /// Gets static instance of the LoginViewModel.
        /// </summary>
        public static LoginViewModel LoginStatic
        {
            get
            {
                if (loginViewStatic == null)
                {
                    loginViewStatic = new LoginViewModel();                    
                }

                return loginViewStatic;
            }
        }

        /// <summary>
        /// Gets static instance of the my surveys view model.
        /// </summary>
        public static MySurveysViewModel MySurveysStatic
        {
            get
            {
                if (mySurveysStatic == null)
                {
                    mySurveysStatic = new MySurveysViewModel();
                }

                return mySurveysStatic;
            }
        }

        /// <summary>
        /// Gets static instance of the home view model.
        /// </summary>
        public static HomeViewModel HomeStatic
        {
            get
            {
                if (homeViewStatic == null)
                {
                    homeViewStatic = new HomeViewModel();
                }

                return homeViewStatic;
            }
        }

        /// <summary>
        /// Gets static instance of the survey details view model.
        /// </summary>
        public static SurveyDetailsViewModel SurveyDetailsStatic
        {
            get
            {
                if (surveyDetailsStatic == null)
                {
                    surveyDetailsStatic = new SurveyDetailsViewModel();
                }

                return surveyDetailsStatic;
            }
        }

        #endregion StaticProperties

        #region Properties

        public SettingsViewModel Settings
        {
            get { return SettingsStatic; }
        }

        public NavigationViewModel Navigation
        {
            get { return NavigationStatic; }
        }

        public SearchViewModel Search
        {
            get { return SearchStatic; }
        }

        public FilterResultsViewModel FilterResult
        {
            get { return FilterResultStatic; }
        }

        public FilterResponsesViewModel FilterResponses
        {
            get { return FilterResponsesStatic; }
        }

        public CategoryViewModel Category
        {
            get { return CategoryStatic; }
        }

        public ResponseSetsViewModel SubmittedResponses
        {
            get { return SubmittedResponsesStatic; }
        }

        public ResponseSetsViewModel SavedResponses
        {
            get { return SavedResponsesStatic; }
        }

        /// <summary>
        /// Gets instance of the loginViewModel.
        /// </summary>
        public LoginViewModel Login
        {
            get
            {
                return LoginStatic;
            }
        }

        /// <summary>
        /// Gets an instance of the MySurveysViewModel.
        /// </summary>
        public MySurveysViewModel MySurveys
        {
            get
            {
                return MySurveysStatic;
            }
        }

        /// <summary>
        /// Gets an indtance of HomeViewModel.
        /// </summary>
        public HomeViewModel Home
        {
            get
            {
                return HomeStatic;
            }
        }

        /// <summary>
        /// Gets an instance of SurveyDetailsViewModel.
        /// </summary>
        public SurveyDetailsViewModel SurveyDetails
        {
            get
            {
                return SurveyDetailsStatic;
            }
        }

        #endregion Properties

        #region ClearsMethods

        /// <summary>
        /// Clears all views.
        /// </summary>
        public static void Cleanup()
        {
            ClearLoginView();
            ClearMySurveys();
            ClearHomeView();
            ClearSurveyDetails();
            ClearSavedResponses();
            ClearFilterResponses();
            ClearSubmittedResponses();
            ClearSearch();
            ClearCategories();
            ClearNavigation();
            ClearSettings();
        }

        private static void ClearSettings()
        {
            settingsStatic = null;
        }

        private static void ClearNavigation()
        {
            navigationStatic = null;
        }

        private static void ClearSearch()
        {
            searchStatic = null;
        }

        private static void ClearFilterResult()
        {
            filterResultStatic = null;
        }

        private static void ClearFilterResponses()
        {
            filterResponsesStatic = null;
        }

        private static void ClearCategories()
        {
            categoryStatic = null;
        }

        private static void ClearSavedResponses()
        {
            savedResponsesStatic = null;
        }

        private static void ClearSubmittedResponses()
        {
            submittedResponsesStatic = null;
        }

        /// <summary>
        /// Clears survey view model.
        /// </summary>
        private static void ClearSurveyDetails()
        {
            surveyDetailsStatic = null;
        }

        /// <summary>
        /// Clears home view model.
        /// </summary>
        private static void ClearHomeView()
        {
            homeViewStatic = null;
        }

        /// <summary>
        /// Clears login view.
        /// </summary>
        private static void ClearLoginView()
        {
            loginViewStatic = null;
        }

        /// <summary>
        /// Clears my surveys view model.
        /// </summary>
        private static void ClearMySurveys()
        {
            mySurveysStatic = null;
        }

        #endregion ClearsMethods
    }
}
