using System;
using System.Net;
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
using NDG.DataAccessModels;
using NDG.StorageAccess;
using NDG.BussinesLogic.Providers;
using NDG.DataAccessModels.Repositories;

namespace NDG.BussinesLogic
{
    public class SurveyGovernor:ISurveyGovernor
    {
        private GetSurveysCallback _callback;

        public void GetNewSurveys(GetSurveysCallback callback)
        {
            var dataService = new SurveyDataService();
            _callback = callback;
            dataService.GetNewSurveys(Membership.CurrentUser, GetSurveysCallback);
        }

        public void GetSurveysCallback(IEnumerable<Survey> surveys)
        {
            if (surveys != null)
            {
                using (var surveyRepository = new SurveyRepository())
                {
                    var existingSurveys = surveyRepository.GetAllUserSurveys(Membership.CurrentUser.ID).Select(s=>s.SystemID).ToList();
                    surveys = surveys.Where(s => !existingSurveys.Contains(s.SystemID));
                }
            }
            _callback.Invoke(surveys);
        }
    }
}
