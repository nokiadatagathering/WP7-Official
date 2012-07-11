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
using System.IO;
using System.Collections.Generic;
using NDG.DataAccessModels.Repositories;

namespace NDG.StorageAccess.DataServices
{
    public class ResponseSetDataService
    {
        private UploadReposneSetCallback _callback;
        private string _boundary = "----------V2ymHFg03ehbqgZCaKO6jy";
        private int _responseSetID;

        public void SendResponseSet(string userName, string password, string serverUrl, int responseSetID, UploadReposneSetCallback callback)
        {
            _callback = callback;
            _responseSetID = responseSetID;
            var request = WebRequest.Create(string.Concat(serverUrl, ServerMethodAddresses.UploadResponseSet));
            request.ContentType = "multipart/form-data; boundary=" + _boundary;
            request.Credentials = new NetworkCredential(userName, password);

            request.Method = "POST";
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);

        }

        protected void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            WebRequest webRequest = (WebRequest)asynchronousResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(asynchronousResult);


            var formData = new Dictionary<string, string>();
            using (var responseSetRepository = new ResponseSetRepository())
                formData["surveyId"] = responseSetRepository.GetResponseSetForUserByID(_responseSetID).Survey.SystemID;

            Stream postDataStream = MultipartRequestFactory.GetMultipartRequestForResultUpload(formData, _boundary, _responseSetID, formData["surveyId"]);

            postDataStream.Position = 0;

            byte[] buffer = new byte[1024];
            int bytesRead = 0;

            while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postStream.Write(buffer, 0, bytesRead);
            }

            postDataStream.Close();
            postStream.Flush();
            postStream.Close();


            webRequest.BeginGetResponse(new AsyncCallback(SendResponseSetCallback), webRequest);
        }

        private void SendResponseSetCallback(IAsyncResult result)
        {
            try
            {
                var request = (WebRequest)result.AsyncState;
                var response = (WebResponse)request.EndGetResponse(result);
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    _callback.Invoke(true);
                }
            }
            catch (WebException)
            {
                _callback.Invoke(false);
            }

        }


    }
}
