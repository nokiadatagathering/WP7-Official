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
using NDG.LanguageParser;
using System.Xml;
using NDG.Helpers.Classes;

namespace NDG.StorageAccess.DataServices
{
    public class LanguageDataService
    {
        private GetLanguageListCallback _getLanguagesListCallback;
        private DownloadLanguageCallback _downloadLanguageCallback;


        public void GetAllLanguages(string serverUrl, GetLanguageListCallback callback)
        {
            try
            {
                _getLanguagesListCallback = callback;

                var request = HttpWebRequest.Create(string.Concat(serverUrl, ServerMethodAddresses.GetLanguageList));

                request.BeginGetResponse(GetAllLanguagesCallback, request);
            }
            catch (Exception ex)
            {
                InvokeCallbackWithLanguageListParameter(null);
            }
        }

        private void GetAllLanguagesCallback(IAsyncResult result)
        {
            try
            {
                var request = (WebRequest)result.AsyncState;
                var response = (WebResponse)request.EndGetResponse(result);
                using (var stream = response.GetResponseStream())
                {
                    var _downloadedLanguagesList = LanguageListParser.GetLanguagesList(stream);
                    InvokeCallbackWithLanguageListParameter(_downloadedLanguagesList);
                }
            }
            catch (WebException)
            {
                InvokeCallbackWithLanguageListParameter(null);
            }
            catch (XmlException)
            {
                InvokeCallbackWithLanguageListParameter(null);
            }
        }

        private void InvokeCallbackWithLanguageListParameter(IEnumerable<Language> param)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _getLanguagesListCallback.Invoke(param);
            });
        }

        public void DownloadLanguage(string serverUrl, Language language, DownloadLanguageCallback callback)
        {
            _downloadLanguageCallback = callback;

            var request = HttpWebRequest.Create(string.Concat(serverUrl, ServerMethodAddresses.DownloadLanguage, language.Culture));

            request.BeginGetResponse(DownloadLanguageCallback, request);
        }

        private void DownloadLanguageCallback(IAsyncResult result)
        {
            try
            {
                var request = (WebRequest)result.AsyncState;
                var response = (WebResponse)request.EndGetResponse(result);
                using (var stream = response.GetResponseStream())
                {
                    var _downloadedLanguageStrings = JavaMessagesParser.GetLanguageStrings(stream);
                    InvokeCallbackWithLanguageStringsParameter(_downloadedLanguageStrings);
                }
            }
            catch (WebException)
            {
                InvokeCallbackWithLanguageStringsParameter(null);
            }
            catch (XmlException)
            {
                InvokeCallbackWithLanguageStringsParameter(null);
            }
        }

        private void InvokeCallbackWithLanguageStringsParameter(SerializableDictionary<string,string> param)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _downloadLanguageCallback.Invoke(param);
            });
        }
    }
}
