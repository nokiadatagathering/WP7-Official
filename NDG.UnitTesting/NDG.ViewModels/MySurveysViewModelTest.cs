using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDG.ViewModels;
using NDG.DataAccessModels.Repositories;
using System.Collections.Generic;
using NDG.DataAccessModels;
using NDG.BussinesLogic.Providers;
using Microsoft.Silverlight.Testing;
using System;
using System.Windows.Navigation;
using NDG.UnitTesting;

namespace NDG.UnitTesting.NDG.ViewModels
{
    [TestClass]
    public class MySurveysViewModelTest : TestBase
    {
        #region Constants

        private const int SURVEYS_COUNT = 100;        

        #endregion Constants

        [TestMethod]
        public void CreationMySurveys()
        {
            MySurveysViewModel mySurveys = new MySurveysViewModel();
            Assert.AreEqual(string.Empty, mySurveys.SearchText);
            Assert.AreEqual(0, mySurveys.TopSurveys.Count, mySurveys.DisplayedSurveyses.Count);
            Assert.IsFalse(mySurveys.IsBusy);
            Assert.IsTrue(mySurveys.DeleteSurveyCommand.CanExecute(null));
        }

        [TestMethod]
        public void InitializationTest()
        {
            MySurveysViewModel mySurvey = new MySurveysViewModel();
            SurveyMock.RemoveAllSurveys();
            SurveyMock.GenerateSurveys(SURVEYS_COUNT);
            mySurvey.InitializeViewModelCommand.Execute(null);
            Assert.AreEqual(MySurveysViewModel.TOP_SURVYES_COUNT, mySurvey.TopSurveys.Count);
            Assert.AreEqual(0, mySurvey.DisplayedSurveyses.Count);
            Assert.AreEqual(string.Empty, mySurvey.SearchText);
        }

        [TestMethod]
        public void InitializationOnSearchTest()
        {
            AddPageParameters(MySurveysViewModel.IS_FOR_SEARCHING + "=true");
            MySurveysViewModel mySurvey = new MySurveysViewModel();
            SurveyMock.RemoveAllSurveys();
            SurveyMock.GenerateSurveys(SURVEYS_COUNT);
            mySurvey.InitializeViewModelCommand.Execute(null);

            Assert.AreEqual(SURVEYS_COUNT, mySurvey.DisplayedSurveyses.Count);
            Assert.AreEqual(0, mySurvey.TopSurveys.Count);

            ClearPageParameters();    
        }
    }
}
