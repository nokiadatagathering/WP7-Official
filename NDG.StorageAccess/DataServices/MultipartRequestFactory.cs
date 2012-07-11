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
using System.Text;
using NDG.ModelsParser;

namespace NDG.StorageAccess.DataServices
{
    public class MultipartRequestFactory
    {

        public static Stream GetMultipartRequestForResultUpload(Dictionary<string, string> formData, string boundary,int responseSetID,string surveySystemID)
        {
            Stream postDataStream = new System.IO.MemoryStream();

            //adding form data
            string formDataHeaderTemplate = "--" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine + Environment.NewLine + "{1}"
            + Environment.NewLine + "--" + boundary + Environment.NewLine;

            foreach (string key in formData.Keys)
            {
                byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(formDataHeaderTemplate,
                key, formData[key]));
                postDataStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            //adding file data

            string fileHeaderTemplate =
            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
            Environment.NewLine + "Content-Type: application/xhtml+xml" + Environment.NewLine + Environment.NewLine;

            byte[] fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(fileHeaderTemplate,
            "filename", string.Format("r_{0}.xml", surveySystemID)));

            postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

            byte[] buffer = Encoding.UTF8.GetBytes(new ResponseSetParser().ParseResponseSetToRequestXml(responseSetID));

            postDataStream.Write(buffer, 0, buffer.Length);

            byte[] endBoundaryBytes = System.Text.Encoding.UTF8.GetBytes(Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);

            return postDataStream;
        }
    }
}
