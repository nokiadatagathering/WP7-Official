using System;
using System.Net;
using System.Linq;
using System.Data.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using NDG.DataAccessModels.Repositories;
using System.Collections.Generic;
using NDG.DataAccessModels;
using NDG.BussinesLogic.Providers;
using NDG.StorageAccess.DataServices;
using NDG.StorageAccess;

namespace NDG.BussinesLogic.Governors
{
    public class ResponseSetGovernor : IResponseSetGovernor
    {

        public void PopulateCategoriesQuestionsWithResponseSetAnswers(System.Collections.Generic.IEnumerable<DataAccessModels.Category> categories, int responseSetID)
        {
            using (var responseSetRepository = new ResponseSetRepository())
            {
                foreach (var category in categories)
                    foreach (var question in category.Question)
                    {
                        var answer = responseSetRepository.GetQuestionAnswerByQuestionAndResponseSet(question.ID, responseSetID);
                        if (answer != null)
                        {
                            question.Data.SetResult(answer.AnswerText);

                        }
                    }
            }

        }

        public ResponseSet CreateNewResponseSetWithAnswers(System.Collections.Generic.IEnumerable<DataAccessModels.Category> categories, int surveyID, string responseSetName)
        {
            var responseSet = new ResponseSet
               {
                   DateSaved = DateTime.Now,
                   IsSubmitted = false,
                   SurveyID = surveyID,
                   UserID = Membership.CurrentUser.ID,
                   SystemID = GenerateUniqueID(),
                   Name = responseSetName,
                   IsCompleted = false,
                   Progress = 0,
               };

            using (var responseSetRepository = new ResponseSetRepository())
            {
                responseSetRepository.AddResponseSetToDB(responseSet);
            }

            return UpdateResponseSetWithAnswers(categories, responseSet.ID);
        }

        public ResponseSet UpdateResponseSetWithAnswers(System.Collections.Generic.IEnumerable<DataAccessModels.Category> categories, int responseSetID)
        {
            ResponseSet responseSet;
            using (var responseSetRepository = new ResponseSetRepository())
            {
                var answersToAdd = new List<Answer>();
                responseSet = responseSetRepository.GetResponseSetForUserByID(responseSetID);

                responseSet.IsCompleted = false;
                responseSet.DateModified = DateTime.Now;
                int incompletedAnswers = 0;
                int disabledQuestions = 0;

                foreach (var category in categories)
                    foreach (var question in category.Question)
                    {

                        var answer = responseSetRepository.GetQuestionAnswerByQuestionAndResponseSet(question.ID, responseSetID);


                        incompletedAnswers++;

                        if (question.Data.Validate())
                        {
                            incompletedAnswers--;
                            if (answer != null)
                                answer.AnswerText = question.Data.GetResult();
                            else
                            {
                                answer = new DataAccessModels.Answer
                                {
                                    AnswerText = question.Data.GetResult(),
                                    QuestionID = question.ID,
                                    ResponseSetID = responseSetID,
                                };
                                answersToAdd.Add(answer);
                            }
                        }
                        if (!question.Data.IsEnabled)
                            disabledQuestions++;
                    }

                responseSet.Progress = Math.Ceiling((1 - ((double)incompletedAnswers / (categories.Sum(c => c.Question.Count) - disabledQuestions))) * 100);
                responseSet.IsCompleted = incompletedAnswers == 0;
                responseSetRepository.AddAnswersToResponseSet(answersToAdd, responseSetID);

            }
            return responseSet;
        }

        private UploadReposneSetCallback _uploadCallback;
        private int _responseSetIdToUpload;
        public void UploadResponseSetToServer(int responseSetID, UploadReposneSetCallback callback)
        {
            _uploadCallback = callback;
            _responseSetIdToUpload = responseSetID;
            new ResponseSetDataService().SendResponseSet(Membership.CurrentUser.Name, Membership.CurrentUser.Password, Membership.CurrentUser.Server.Address, responseSetID, UploadResponseSetCallback);
        }

        private void UploadResponseSetCallback(bool result)
        {
            if (result)
            {
                using (var responseSetRepository = new ResponseSetRepository())
                {
                    responseSetRepository.MarkResponseSetAsSubmitted(_responseSetIdToUpload);
                }
            }
            _uploadCallback.Invoke(result);
        }

        private string GenerateUniqueID()
        {
            Random rnd = new Random();
            long uniqueID = (((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds >> 16) << 16) + (long)((rnd.NextDouble() * 2.0 - 1.0) * long.MaxValue);
            return ((int)uniqueID).ToString("X8");
        }

    }
}
