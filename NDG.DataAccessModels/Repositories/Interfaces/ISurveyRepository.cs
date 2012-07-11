using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Generic;


namespace NDG.DataAccessModels.Repositories
{
    public interface ISurveyRepository : IRepository
    {
        IEnumerable<Survey> GetAllUserSurveys(int userID);
        IEnumerable<Survey> GetUserTopSurveys(int topCount,int userID);
        Survey GetSurveyByID(int id);
        Survey GetSurveyByResponseSetID(int responseSetID);
        IEnumerable<ResponseSet> GetSurveyResponseSetsBySurveyID(int surveyID);
        void AddNewSurveyForUser(Survey survey, int userID);
        void AddNewSurveyCollectionForUser(IEnumerable<Survey> surveys,int userID);
        IEnumerable<ResponseSet> GetCompletedResponseSets(int surveyID);
        IEnumerable<ResponseSet> GetInProgressResponseSets(int surveyID);
        IEnumerable<ResponseSet> GetSubmittedResponseSets(int surveyID);
        bool DeleteSurvey(int surveyID);
    }
}
