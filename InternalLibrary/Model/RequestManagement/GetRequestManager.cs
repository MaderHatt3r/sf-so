using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using InternalLibrary.IO;
using InternalLibrary.Model.Builder;

namespace InternalLibrary.Model.RequestManagement
{
    public class GetRequestManager
    {
        /// <summary>
        /// The service
        /// </summary>
        private DriveService service = null;

        private DownloadBuilder builder;

        public GetRequestManager(DriveService service)
        {
            this.builder = new DownloadBuilder();
            this.service = service;
        }

        public File GetMetadata(string fileID)
        {
            File googleFile = this.service.Files.Get(fileID).Execute();
            return googleFile;
        }

        public string Save(string fileID)
        {
            return FileIO.SaveFile(Download(fileID));
        }

        private System.IO.Stream Download(string fileID)
        {
            File file = GetMetadata(fileID);
            if (!String.IsNullOrEmpty(file.DownloadUrl))
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(file.DownloadUrl));
                    service.Authenticator.ApplyAuthenticationToRequest(request);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return response.GetResponseStream();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("An error occurred: " + response.StatusDescription);
                        return null;
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("An error occurred: " + e.Message);
                    return null;
                }
            }
            else
            {
                // The file doesn't have any content stored on Drive.
                return null;
            }

        }
    }
}
