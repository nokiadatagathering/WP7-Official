using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDG.StorageAccess;

namespace NDG.BussinesLogic
{
    public interface ISurveyGovernor
    {
        void GetNewSurveys(GetSurveysCallback callback);
    }
}
