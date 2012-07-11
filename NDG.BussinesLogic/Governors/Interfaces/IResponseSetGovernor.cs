using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDG.DataAccessModels;
using NDG.StorageAccess;

namespace NDG.BussinesLogic.Governors
{
    interface IResponseSetGovernor
    {
        void PopulateCategoriesQuestionsWithResponseSetAnswers(IEnumerable<Category> categories, int responseSetID);
        ResponseSet CreateNewResponseSetWithAnswers(IEnumerable<Category> categories, int surveyID, string responseSetName);
        ResponseSet UpdateResponseSetWithAnswers(IEnumerable<Category> categories, int responseSetID);
        void UploadResponseSetToServer(int responseSetID, UploadReposneSetCallback callback);
    }
}
