// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 09-15-2013
//
// Last Modified By : CTDragon
// Last Modified On : 09-23-2013
// ***********************************************************************
// <copyright file="GetRequestManager.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class GetRequestManager.
    /// </summary>
    public class GetRequestManager
    {
        /// <summary>
        /// The service
        /// </summary>
        private DriveService service = null;

        /// <summary>
        /// The builder
        /// </summary>
        private DownloadBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestManager"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public GetRequestManager(DriveService service)
        {
            this.builder = new DownloadBuilder();
            this.service = service;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <param name="fileID">The file unique identifier.</param>
        /// <returns>File.</returns>
        public File GetMetadata(string fileID)
        {
            File googleFile = this.service.Files.Get(fileID).Execute();
            return googleFile;
        }

        /// <summary>
        /// Saves the specified file unique identifier.
        /// </summary>
        /// <param name="fileID">The file unique identifier.</param>
        /// <returns>System.String. Full File Path</returns>
        public string Save(string fileID, string fileName)
        {
            return FileIO.SaveFile(Download(fileID), fileName);
        }

        /// <summary>
        /// Saves the specified google file.
        /// </summary>
        /// <param name="googleFile">Any object containing a property for "DownloadUrl".</param>
        /// <returns>System.String. Full File Path</returns>
        public string Save(dynamic googleFile, string fileName)
        {
            return FileIO.SaveFile(Download(googleFile), fileName);
        }

        /// <summary>
        /// Downloads the specified google file.
        /// </summary>
        /// <param name="googleFile">The google file.</param>
        /// <returns>System.IO.Stream.</returns>
        private System.IO.Stream Download(dynamic googleFile)
        {
            if (!String.IsNullOrEmpty(googleFile.DownloadUrl))
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(googleFile.DownloadUrl));
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

        /// <summary>
        /// Downloads the specified file unique identifier.
        /// </summary>
        /// <param name="fileID">The file unique identifier.</param>
        /// <returns>System.IO.Stream.</returns>
        private System.IO.Stream Download(string fileID)
        {
            File file = GetMetadata(fileID);
            return Download(file);
        }

    }
}
