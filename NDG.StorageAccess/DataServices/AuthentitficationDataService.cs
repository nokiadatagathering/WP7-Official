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
using NDG.DataAccessModels;
using System.IO;
using NDG.Common;

namespace NDG.StorageAccess
{
    public class AuthentitficationDataService
    {
        private AuthentificateUserCallback _callback;
        public void AuthentificateUser(string userName, string password, string serverUrl, AuthentificateUserCallback callback)
        {
            _callback = callback;
            var request = HttpWebRequest.Create(string.Concat(serverUrl, ServerMethodAddresses.AuthentificateUser));
            request.Credentials = new NetworkCredential(userName, password);
            var result = request.BeginGetResponse(AuthentificateUserCallback, request);
        }

        private void AuthentificateUserCallback(IAsyncResult result)
        {

            try
            {
                var request = (WebRequest)result.AsyncState;
                var response = (WebResponse)request.EndGetResponse(result);
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    if (reader.ReadToEnd().Equals("OK"))
                        _callback.Invoke(AuthentificationCode.LoginSuccessed);
                    else
                        _callback.Invoke(AuthentificationCode.InvalidCredentials);
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException)
                {
                    var response = ((HttpWebResponse)((WebException)ex).Response);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        _callback.Invoke(AuthentificationCode.InvalidCredentials);
                    else
                        _callback.Invoke(AuthentificationCode.ServerNotFound);
                }
                else if (ex is ArgumentException)
                {
                    _callback.Invoke(AuthentificationCode.ServerNotFound);
                }
            }

        }
    }
}
