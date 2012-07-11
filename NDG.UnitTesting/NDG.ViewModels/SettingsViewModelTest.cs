using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDG.BussinesLogic.Providers.Settings;
using NDG.DataAccessModels.Repositories;
using NDG.ViewModels;
using NDG.DataAccessModels;
using System;

namespace NDG.UnitTesting.NDG.ViewModels
{
    [TestClass]
    public class SettingsViewModelTest : TestBase
    {
        private const string REFRESH_ENDED = "REFRESHED";

        SettingsRepository settingsRespository = new SettingsRepository();

        [TestMethod]
        public void CreationTest()
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            Assert.AreEqual(0, settingsViewModel.BusyCount);
            Assert.IsFalse(settingsViewModel.IsBusy);
            Assert.IsTrue(settingsViewModel.InitializeViewModelCommand.CanExecute(null));
        }

        [TestMethod]
        [Asynchronous]
        public void InitializationSettingsTest()
        {
            bool isTestCompleteCalled = false;
            SettingsViewModel settings = new SettingsViewModel();
            settings.TestCompleted += (sender, args) =>
            {
                if (args.Message == REFRESH_ENDED)
                {
                    var defaultSettings = settingsRespository.GetDefaultSettings();
                    Assert.AreEqual(defaultSettings.PhotoResolutionID, settings.SelectedResolution.ID);
                    Assert.AreEqual(defaultSettings.LanguageID, settings.CurrentLanguage.ID);
                    Assert.AreEqual(defaultSettings.IsGpsEnabled, settings.IsGpsEnabled);
                    Assert.AreEqual(defaultSettings.CheckForNewSurveys, settings.IsCheckForNewSurveysOnStart);
                    CallTestComplete(ref isTestCompleteCalled);
                }
            };

            settings.InitializeViewModelCommand.Execute(null);
            EndOnWaitingResponse();
        }

        [TestMethod]
        [Asynchronous]
        public void ChangeSettingsTest()
        {
            bool isTestCompleted = false;
            bool wasInitialization = false;
            SettingsViewModel settingsVm = new SettingsViewModel();
            settingsVm.TestCompleted += (sender, args) =>
            {
                if (args.Message == REFRESH_ENDED)
                {
                    if (!wasInitialization)
                    {
                        var defaultSettings = settingsRespository.GetDefaultSettings();
                        Assert.AreEqual(defaultSettings.PhotoResolutionID, settingsVm.SelectedResolution.ID);
                        Assert.AreEqual(defaultSettings.LanguageID, settingsVm.CurrentLanguage.ID);
                        Assert.AreEqual(defaultSettings.IsGpsEnabled, settingsVm.IsGpsEnabled);
                        Assert.AreEqual(defaultSettings.CheckForNewSurveys, settingsVm.IsCheckForNewSurveysOnStart);

                        settingsVm.IsGpsEnabled = false;
                        settingsVm.IsCheckForNewSurveysOnStart = false;
                        settingsVm.SelectedResolution = settingsVm.Resolutions[1];
                        settingsVm.CurrentLanguage = settingsVm.Languages[0];
                        wasInitialization = true;

                        settingsVm.SaveSettingsCommand.Execute(null);
                    }
                    else
                    {
                        var currentSettings = settingsRespository.GetCurrentSettings();
                        Assert.AreEqual(settingsVm.IsCheckForNewSurveysOnStart, currentSettings.CheckForNewSurveys);
                        Assert.AreEqual(settingsVm.IsGpsEnabled, currentSettings.IsGpsEnabled);
                        Assert.AreEqual(settingsVm.CurrentLanguage.ID, currentSettings.LanguageID);
                        Assert.AreEqual(settingsVm.SelectedResolution.ID, currentSettings.PhotoResolutionID);
                        Assert.AreEqual(false, currentSettings.IsDefault);

                        settingsRespository.ResetCurrentSettings();

                        CallTestComplete(ref isTestCompleted);
                    }
                }
            };

            settingsVm.InitializeViewModelCommand.Execute(null);
            EndOnWaitingResponse();
        }
    }
}
