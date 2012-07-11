using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDG.ViewModels;
using Microsoft.Silverlight.Testing;
using NDG.BussinesLogic.Providers;

namespace NDG.UnitTesting.NDG.ViewModels
{
    [TestClass]
    public class HomeViewModelTest : TestBase
    {
        #region Constants

        private const string SURVEY_TEST_COMLETE = "Surveys_refresh";

        private const string SAVED_TEST_COMPLETE = "Save_refresh";

        private const string SUBMITTED_TEST_COMPLETE = "Submitted_refresh";

        #endregion Constants

        [TestMethod]
        public void CreationHomeTest()
        {
            HomeViewModel homeView = new HomeViewModel();
            Assert.AreEqual(homeView.CurrentPageIndex, HomePageIndexes.MySurveys);
            Assert.IsFalse(homeView.IsBusy);
        }

        [TestMethod]
        public void RefreshTest()
        {
            //this.TryRefresh(SURVEY_TEST_COMLETE, HomePageIndexes.MySurveys, Locator.MySurveysStatic);
            this.TryRefresh(SAVED_TEST_COMPLETE, HomePageIndexes.SavedResponses, Locator.SavedResponsesStatic);
            this.TryRefresh(SUBMITTED_TEST_COMPLETE, HomePageIndexes.SubmittedResponses, Locator.SubmittedResponsesStatic);
        }

        [Asynchronous]
        private void TryRefresh(string expectedMessage, HomePageIndexes currentIndex, ViewModel currentViewModel)
        {
            HomeViewModel home = new HomeViewModel();
            bool isTestCompleteCalled = false;
            currentViewModel.TestCompleted += (sender, args) =>
                {
                    Assert.AreEqual(args.Message, expectedMessage);
                    if (currentIndex == HomePageIndexes.MySurveys)
                    {
                        CallTestComplete(ref isTestCompleteCalled);
                    }
                };
            
            home.CurrentPageIndex = currentIndex;
            home.RefreshCommand.Execute(null);
            if (currentIndex == HomePageIndexes.MySurveys)
            {
                EndOnWaitingResponse();
            }
        }
    }
}
