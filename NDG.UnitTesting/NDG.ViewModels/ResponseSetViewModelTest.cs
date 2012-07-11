using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDG.ViewModels;
using Microsoft.Silverlight.Testing;
using NDG.BussinesLogic.Providers;

namespace NDG.UnitTesting.NDG.ViewModels
{
    [TestClass]
    public class ResponseSetViewModelTest : TestBase
    {
        private const int RESPONSE_SETS_COUNT = 100;
        private const string RESPONSE_DELETED = "RESPONSE_DELETED";
        private const int SURVEY_ID = 345;

        [TestMethod]
        public void CreationTest()
        {
            ResponseSetsViewModel savedViewModel = new ResponseSetsViewModel(ResponseSetsType.Saved);
            Assert.IsFalse(savedViewModel.IsBusy);
            Assert.IsTrue(savedViewModel.DeleteResponseSetCommand.CanExecute(null));
            Assert.IsTrue(savedViewModel.InitializeViewModelCommand.CanExecute(null));
            Assert.AreEqual(string.Empty, savedViewModel.SearchString);
        }

        [TestMethod]
        [Asynchronous]
        public void InitializationSavedTest()
        {
            SurveyMock.RemoveAllSurveys();
            SurveyMock.GenerateResponseSets(RESPONSE_SETS_COUNT);
            DataAccessModels.Repositories.ResponseSetRepository respository = new DataAccessModels.Repositories.ResponseSetRepository();
            var items = respository.GetAllResponseSetsForUser(Membership.CurrentUser.ID);
            var responseSets = new System.Collections.ObjectModel.ObservableCollection<DataAccessModels.ResponseSet>(items);
            this.InitializationResponseSetViewModel(ResponseSetsType.Saved, "TopResponses");
        }

        [TestMethod]
        [Asynchronous]
        public void InitializationSubmittedTest()
        {
            SurveyMock.RemoveAllSurveys();
            SurveyMock.GenerateResponseSets(RESPONSE_SETS_COUNT);
            this.InitializationResponseSetViewModel(ResponseSetsType.Submitted, "TopResponses");
        }

        [TestMethod]
        public void DeleteTest()
        {
            bool isTestCompleted = true;
            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Saved, ResponseSetType.Completed);
            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Saved, ResponseSetType.InProgress);
            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Saved, ResponseSetType.None);
            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Saved, ResponseSetType.Submitted);

            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Submitted, ResponseSetType.Completed);
            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Submitted, ResponseSetType.InProgress);
            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Submitted, ResponseSetType.None);
            isTestCompleted &= this.DeleteResponseSet(ResponseSetsType.Submitted, ResponseSetType.Submitted);
            Assert.IsTrue(isTestCompleted);
        }

        [Asynchronous]
        private void InitializationResponseSetViewModel(ResponseSetsType type, string propertyName)
        {
            bool isTestCompleteCalled = false;
            ResponseSetsViewModel responseSet = new ResponseSetsViewModel(type);
            responseSet.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propertyName)
                {
                    Assert.AreEqual(ResponseSetsViewModel.TOP_COUNT, responseSet.TopResponses.Count);                    
                    if (type == ResponseSetsType.Submitted)
                    {
                        var submitted = responseSet.DisplayedResponses.Where(item => item.IsSubmitted == true);
                        Assert.AreEqual(responseSet.DisplayedResponses.Count, submitted.Count());
                    }
                    else
                    {
                        Assert.AreEqual(RESPONSE_SETS_COUNT - RESPONSE_SETS_COUNT / 4, responseSet.DisplayedResponses.Count);
                    }

                    CallTestComplete(ref isTestCompleteCalled);
                }
            };

            responseSet.InitializeViewModelCommand.Execute(null);
            EndOnWaitingResponse();
        }

        private bool DeleteResponseSet(ResponseSetsType viewModelType, ResponseSetType responseType)
        {
            SurveyMock.RemoveAllSurveys();
            bool isCompleted = false;
            var survey = SurveyMock.GenerateAndSaveSurvey(SURVEY_ID);
            var responseSet = SurveyMock.GenerateAndSaveResponseSet(responseType, survey);
            ResponseSetsViewModel viewModel = new ResponseSetsViewModel(viewModelType);
            viewModel.TestCompleted += (sender, args) =>
            {
                if (args.Message == RESPONSE_DELETED)
                {
                    Assert.AreEqual(0, viewModel.DisplayedResponses.Count);
                    Assert.AreEqual(0, viewModel.TopResponses.Count);
                    isCompleted = true;
                }
            };

            viewModel.DeleteResponseSetCommand.Execute(responseSet);
            return isCompleted;
        }
    }
}
