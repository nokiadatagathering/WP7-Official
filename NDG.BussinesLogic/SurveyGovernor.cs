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

using System.Collections.Generic;
using NDG.DataAccessModels;
using NDG.StorageAccess;

namespace NDG.BussinesLogic
{
    public class SurveyGovernor:ISurveyGovernor
    {
        public void GetNewSurveys(GetSurveysCallback callback)
        {
            var dataService = new SurveyDataService();
            dataService.GetNewSurveys("admin", "ndg", "http://192.168.13.47:9000/ReceiveSurvey", callback);
        }
    }
}
