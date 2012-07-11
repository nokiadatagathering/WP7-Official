using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Moq;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using NDG.DataAccessModels.Repositories;
using NDG.BussinesLogic.Providers;
using System.Collections.Generic;
using NDG.DataAccessModels;
using NDG.UnitTesting.NDG.ViewModels;

namespace NDG.UnitTesting
{
    public enum ResponseSetType
    {
        None,
        Completed,
        InProgress,
        Submitted
    }

    public static class SurveyMock
    {
        private static Random rand = new Random();

        static SurveyRepository surveyRepository = new SurveyRepository();

        static ResponseSetRepository repository = new ResponseSetRepository();

        private const string SURVEY_NAME = "some name";

        public static void RemoveAllSurveys()
        {            
            var surveys = surveyRepository.GetAllUserSurveys(Membership.CurrentUser.ID);
            foreach (var item in surveys)
            {
                surveyRepository.DeleteSurvey(item.ID);
            }
        }

        public static void GenerateSurveys(int count)
        {
            RemoveAllSurveys();
            List<Survey> surveys = new List<Survey>();
            while (count > 0)
            {
                surveys.Add(GenerateSurvey(count));
                count--;
            }

            surveyRepository.AddNewSurveyCollectionForUser(surveys, Membership.CurrentUser.ID);
        }

        public static Survey GenerateSurvey(int id)
        {
            Survey survey = new Survey();
            survey.Name = SURVEY_NAME;
            survey.DateReceived = DateTime.Now;
            survey.ID = id;
            survey.UserID = Membership.CurrentUser.ID;
            survey.SystemID = LoginViewModelTest.SERVER_PATH;
            return survey;
        }

        public static Survey GenerateAndSaveSurvey(int id)
        {
            var survey = GenerateSurvey(id);
            surveyRepository.AddNewSurveyForUser(survey, Membership.CurrentUser.ID);
            return survey;
        }

        public static ResponseSet GenerateAndSaveResponseSet(ResponseSetType type, Survey survey)
        {
            var responseSet = GenerateResponseSet(type, survey);
            repository.AddResponseSetToDB(responseSet);
            return responseSet;
        }

        public static ResponseSet GenerateResponseSet(ResponseSetType type, Survey survey)
        {
            ResponseSet set = new ResponseSet();
            set.ID = rand.Next(0, 1000);
            set.SurveyID = survey.ID;
            set.SystemID = LoginViewModelTest.SERVER_PATH;
            set.Name = rand.Next(0, 1000).ToString();
            switch (type)
            {
                case ResponseSetType.Completed:
                    set.IsCompleted = true;
                    break;
                case ResponseSetType.InProgress:
                    set.IsCompleted = false;
                    set.IsSubmitted = false;
                    break;
                case ResponseSetType.Submitted:
                    set.IsSubmitted = true;
                    break;
                default:
                    break;
            }

            set.UserID = Membership.CurrentUser.ID;
            return set;
        }

        public static List<ResponseSet> GenerateResponseSets(ResponseSetType type, int count)
        {
            List<ResponseSet> result = new List<ResponseSet>();
            Survey survey = GenerateSurvey(rand.Next(0, 1000));
            surveyRepository.AddNewSurveyForUser(survey, Membership.CurrentUser.ID);
            while (count > 0)
            {
                var set = GenerateResponseSet(type, survey);
                result.Add(set);
                repository.AddResponseSetToDB(set);
                count--;
            }

            return result;
        }

        public static List<ResponseSet> GenerateResponseSets(int count)
        {
            List<ResponseSet> result = new List<ResponseSet>();
            Survey survey = GenerateSurvey(rand.Next(0, 1000));
            surveyRepository.AddNewSurveyForUser(survey, Membership.CurrentUser.ID);
            while (count > 0)
            {
                ResponseSetType type = (ResponseSetType)(count % 4);
                var set = GenerateResponseSet(type, survey);
                result.Add(set);
                repository.AddResponseSetToDB(set);
                count--;
            }

            return result;
        }
    }
}
