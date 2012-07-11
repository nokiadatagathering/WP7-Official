using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Collections.Generic;


namespace NDG.DataAccessModels.Repositories
{
    public class SurveyRepository : Repository, ISurveyRepository
    {

        public Survey GetSurveyByID(int id)
        {
            return _context.Survey.FirstOrDefault(s => s.ID == id);
            
        }

        public IEnumerable<Survey> GetAllUserSurveys(int userID)
        {
            return _context.Survey.Where(s => s.UserID == userID);
        }

        public IEnumerable<Survey> GetUserTopSurveys(int topCount, int userID)
        {
            return _context.Survey.Where(s => s.UserID == userID).OrderByDescending(s => s.DateReceived).Take(topCount);
        }

        public IEnumerable<ResponseSet> GetSurveyResponseSetsBySurveyID(int surveyID)
        {
            return _context.ResponseSet.Where(r => r.SurveyID == surveyID);
        }

        public void AddNewSurveyForUser(Survey survey, int userID)
        {
            survey.UserID = userID;
            _context.Survey.InsertOnSubmit(survey);
            _context.SubmitChanges();
        }

        public void AddNewSurveyCollectionForUser(IEnumerable<Survey> surveys, int userID)
        {
            foreach (var s in surveys)
            {
                s.UserID = userID;
            }
            _context.Survey.InsertAllOnSubmit(surveys);
            _context.SubmitChanges();
        }


        public Survey GetSurveyByResponseSetID(int responseSetID)
        {
            return _context.ResponseSet.First(s => s.ID == responseSetID).Survey;
        }


        public IEnumerable<ResponseSet> GetCompletedResponseSets(int surveyID)
        {
            return _context.Survey.First(s => s.ID == surveyID).ResponseSet.Where(r => r.IsCompleted && !r.IsSubmitted);
        }

        public IEnumerable<ResponseSet> GetInProgressResponseSets(int surveyID)
        {
            return _context.Survey.First(s => s.ID == surveyID).ResponseSet.Where(r => !r.IsCompleted && !r.IsSubmitted);

        }

        public IEnumerable<ResponseSet> GetSubmittedResponseSets(int surveyID)
        {
            return _context.Survey.First(s => s.ID == surveyID).ResponseSet.Where(r => r.IsSubmitted);
        }


        public bool DeleteSurvey(int surveyID)
        {
            try
            {
                var survey = this.GetSurveyByID(surveyID);
                if (survey != null)
                {
                    _context.ResponseSet.DeleteAllOnSubmit(GetSurveyResponseSetsBySurveyID(surveyID));
                    _context.SubmitChanges();
                    _context.Survey.DeleteOnSubmit(survey);
                    _context.SubmitChanges();
                    _context.DependentQuestions.DeleteAllOnSubmit(survey.Category.SelectMany(c => c.Question).Select(q => q.DependentQuestion).Where(d => d != null));
                    _context.SubmitChanges();
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
