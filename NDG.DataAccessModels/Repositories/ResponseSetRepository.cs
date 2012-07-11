using System;
using System.Net;
using System.Data.Linq;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace NDG.DataAccessModels.Repositories
{
    public class ResponseSetRepository : Repository, IResponseSetRepository
    {

        public System.Collections.Generic.IEnumerable<ResponseSet> GetAllResponseSetsForUser(int userID)
        {
            return _context.ResponseSet.Where(r => r.UserID == userID);
        }

        public ResponseSet GetResponseSetForUserByID(int responseSetID)
        {
            return _context.ResponseSet.FirstOrDefault(r => r.ID == responseSetID);
        }

        public Answer GetQuestionAnswerByQuestionAndResponseSet(int questionID, int responseSetID)
        {
            return _context.Answer.FirstOrDefault(a => a.ResponseSetID == responseSetID && a.QuestionID == questionID);
        }


        public System.Collections.Generic.IEnumerable<ResponseSet> GetSurveyResponseSetsForUser(int surveyID)
        {
            return _context.ResponseSet.Where(r => r.SurveyID == surveyID);
        }

        public System.Collections.Generic.IEnumerable<ResponseSet> GetSurveyResponseSets(int surveyID)
        {
            throw new NotImplementedException();
        }

        public void AddAnswersToResponseSet(IEnumerable<Answer> answers, int responseSetID)
        {
            foreach (var answer in answers)
                answer.ResponseSetID = responseSetID;
            _context.Answer.InsertAllOnSubmit(answers);
            _context.SubmitChanges();
        }


        public void AddResponseSetToDB(ResponseSet responseSet)
        {
            _context.ResponseSet.InsertOnSubmit(responseSet);
            _context.SubmitChanges();
        }


        public IEnumerable<ResponseSet> GetSavedResponseSetsForUser(int userID)
        {
            return _context.ResponseSet.Where(r => r.UserID == userID && !r.IsSubmitted);
        }

        public IEnumerable<ResponseSet> GetSubmittedResponseSetsForUser(int userID)
        {
            return _context.ResponseSet.Where(r => r.UserID == userID && r.IsSubmitted);
        }


        public IEnumerable<ResponseSet> GetTopSavedResponseSetsForUser(int userID, int topCount)
        {
            return _context.ResponseSet.Where(r => r.UserID == userID && !r.IsSubmitted).OrderByDescending(r => r.DateSaved).Take(topCount);
        }

        public IEnumerable<ResponseSet> GetTopSubmittedResponseSetsForUser(int userID, int topCount)
        {
            return _context.ResponseSet.Where(r => r.UserID == userID && r.IsSubmitted).OrderByDescending(r => r.DateSubmitted).Take(topCount);
        }


        public void MarkResponseSetAsSubmitted(int responseSetID)
        {
            var responseSet = _context.ResponseSet.FirstOrDefault(r => r.ID == responseSetID);
            if (responseSet != null)
            {
                responseSet.IsSubmitted = true;
                responseSet.DateSubmitted = DateTime.Now;
                _context.SubmitChanges();
            }
        }


        public bool DeleteResponseSet(int responseSetID)
        {
            try
            {
                var responseSet = this.GetResponseSetForUserByID(responseSetID);
                if (responseSet != null)
                    _context.ResponseSet.DeleteOnSubmit(responseSet);
                _context.SubmitChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
