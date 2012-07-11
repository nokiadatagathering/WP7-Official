using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDG.DataAccessModels.Repositories
{
    interface IResponseSetRepository : IRepository
    {
        IEnumerable<ResponseSet> GetAllResponseSetsForUser(int userID);
        IEnumerable<ResponseSet> GetSavedResponseSetsForUser(int userID);
        IEnumerable<ResponseSet> GetSubmittedResponseSetsForUser(int userID);
        IEnumerable<ResponseSet> GetTopSavedResponseSetsForUser(int userID, int topCount);
        IEnumerable<ResponseSet> GetTopSubmittedResponseSetsForUser(int userID, int topCount);
        IEnumerable<ResponseSet> GetSurveyResponseSets(int surveyID);
        ResponseSet GetResponseSetForUserByID(int responseSetID);
        Answer GetQuestionAnswerByQuestionAndResponseSet(int questionID, int responseSetID);
        void AddAnswersToResponseSet(IEnumerable<Answer> answers, int responseSetID);
        void AddResponseSetToDB(ResponseSet responseSet);
        void MarkResponseSetAsSubmitted(int responseSetID);
        bool DeleteResponseSet(int responseSetID);
    }
}
