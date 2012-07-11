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
using NDG.Common;
using NDG.BussinesLogic.Providers;

namespace NDG.BussinesLogic.SpecificLogic
{
    public class FilterProcessor : IDisposable
    {
        private ResponseSetRepository _responseSetRepository;

        public FilterProcessor()
        {
            _responseSetRepository = new ResponseSetRepository();
        }
        public IEnumerable<ResponseSet> FilterSavedResponseSet(FilterParameters parameters)
        {
            return FilterResponseSet(parameters, _responseSetRepository.GetSavedResponseSetsForUser(Membership.CurrentUser.ID), c => { return c.DateSaved.Value; });
        }

        public IEnumerable<ResponseSet> FilterSubmittedResponseSet(FilterParameters parameters)
        {
            return FilterResponseSet(parameters, _responseSetRepository.GetSubmittedResponseSetsForUser(Membership.CurrentUser.ID), c => { return c.DateSubmitted.Value; });
        }

        private IEnumerable<ResponseSet> FilterResponseSet(FilterParameters parameters, IEnumerable<ResponseSet> responseSets, Func<ResponseSet, DateTime> criteriaDate)
        {
            var resultCollection = new List<ResponseSet>();
            switch (parameters.Type)
            {
                case FilterType.ByDate:
                    {
                        switch (parameters.Date.SelectedPeriod.Key)
                        {
                            case TimePeriods.After:
                                resultCollection = responseSets.Where(r => criteriaDate(r).Date > parameters.Date.SelectedDate.Date).ToList();
                                break;
                            case TimePeriods.At:
                                resultCollection = responseSets.Where(r => criteriaDate(r).Date == parameters.Date.SelectedDate.Date).ToList();
                                break;
                            case TimePeriods.Before:
                                resultCollection = responseSets.Where(r => criteriaDate(r).Date < parameters.Date.SelectedDate.Date).ToList();
                                break; 
                            case TimePeriods.Between:
                                resultCollection = responseSets.Where(r => criteriaDate(r).Date <= parameters.Date.SelectedEndDate.Date && criteriaDate(r).Date >= parameters.Date.SelectedStartDate.Date).ToList();
                                break;
                        };
                    }
                    break;
            }
            return resultCollection;
        }

        public void Dispose()
        {
            _responseSetRepository.Dispose();
            GC.SuppressFinalize(this);

        }
    }
}
