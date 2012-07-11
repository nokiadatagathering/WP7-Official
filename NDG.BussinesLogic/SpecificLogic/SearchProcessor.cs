using System;
using System.Linq;
using System.Data.Linq;
using System.Net;
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

namespace NDG.BussinesLogic.SpecificLogic
{
    public class SearchProcessor:IDisposable
    {
        private SurveyRepository _surveyRepository;
        private ResponseSetRepository _responseSetRepository;

        public SearchProcessor()
        {
            _surveyRepository = new SurveyRepository();
            _responseSetRepository = new ResponseSetRepository();
        }

        public IEnumerable<Survey> SearchSurveysByName(string name)
        {
            return _surveyRepository.GetAllUserSurveys(Membership.CurrentUser.ID).Where(s => s.Name.ToLower().Contains(name.ToLower()));
        }

        public IEnumerable<ResponseSet> SearchSavedResponseSetsByName(string name)
        {
            return _responseSetRepository.GetSavedResponseSetsForUser(Membership.CurrentUser.ID).Where(s => s.Name.ToLower().Contains(name.ToLower()));
        }

        public IEnumerable<ResponseSet> SearchSubmittedResponseSetsByName(string name)
        {
            return _responseSetRepository.GetSubmittedResponseSetsForUser(Membership.CurrentUser.ID).Where(s => s.Name.ToLower().Contains(name.ToLower()));
        }

        public void Dispose()
        {
            _surveyRepository.Dispose();
            _responseSetRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
