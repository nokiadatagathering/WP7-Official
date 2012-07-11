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
using System.Xml;
using NDG.XFormsParser;
using NDG.DataAccessModels;
using System.Collections;
using System.Collections.Generic;


namespace NDG.StorageAccess
{
    public class SurveyDataService
    {
        private GetSurveysCallback _callback;
        private User _currentUser;
        private IList<Survey> _downloadedSurveysList;
        private int _surveysToDownloadCount;
        private object _lock = new object();

        public void GetNewSurveys(User user, GetSurveysCallback callback)
        {
            _callback = callback;
            _currentUser = user;
            var request = HttpWebRequest.Create(string.Concat(user.Server.Address,ServerMethodAddresses.CheckForNewSurveys));
            request.Credentials = new NetworkCredential(user.Name, user.Password);
            request.BeginGetResponse(GetNewSurveysCallback, request);
        }

        private void GetNewSurveysCallback(IAsyncResult result)
        {
            try
            {
                var request = (WebRequest)result.AsyncState;
                var response = (WebResponse)request.EndGetResponse(result);
                using (var stream = response.GetResponseStream())
                {
                    var surveyXmlParser = new SurveysCollectionParser();
                    var surveysToDownload = surveyXmlParser.GetSurveysDownloadUrlsFromStream(stream).ToList();
                    _surveysToDownloadCount = surveysToDownload.Count;
                    _downloadedSurveysList = new List<Survey>();
                    if (surveysToDownload.Count != 0)
                        foreach (var downloadUrl in surveysToDownload)
                            DownloadSurvey(downloadUrl, _currentUser.Name,_currentUser.Password);
                    else
                        InvokeCallbackWithSurveyListParameter(_downloadedSurveysList);
                }
            }
            catch (WebException)
            {
                InvokeCallbackWithSurveyListParameter(null);
            }
            catch (XmlException)
            {
                InvokeCallbackWithSurveyListParameter(null);
            }
        }


        public void DownloadSurvey(string surveyDownloadUrl, string name, string pass)
        {
            var request = HttpWebRequest.Create(surveyDownloadUrl);
            request.Credentials = new NetworkCredential(name, pass);
            request.BeginGetResponse(DownloadSurveyCallback, request);
        }

        private void DownloadSurveyCallback(IAsyncResult result)
        {
            try
            {
                var request = (WebRequest)result.AsyncState;
                var response = (WebResponse)request.EndGetResponse(result);
                using (var stream = response.GetResponseStream())
                {
                    var surveyContentParser = new SurveyContentParser();
                    var survey = surveyContentParser.GetSurveyFromXFormsStream(stream);
                    lock (_lock)
                    {
                        _downloadedSurveysList.Add(survey);
                        if (_downloadedSurveysList.Count == _surveysToDownloadCount)
                            InvokeCallbackWithSurveyListParameter(_downloadedSurveysList);
                    }

                }
            }
            catch (WebException)
            {
                throw;
            }
            catch (XmlException)
            {
                throw;
            }
        }

        private void InvokeCallbackWithSurveyListParameter(IEnumerable<Survey> param)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _callback.Invoke(param);
            });
        }
    }
}
