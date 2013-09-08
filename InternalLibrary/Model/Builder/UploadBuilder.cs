// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 06-13-2013
//
// Last Modified By : CTDragon
// Last Modified On : 06-13-2013
// ***********************************************************************
// <copyright file="UploadBuilder.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using InternalLibrary.Data;
using Google.Apis.Util;

//For build service
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Services;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;

using System.Diagnostics;
using DotNetOpenAuth.OAuth2;
using System.Security.Cryptography;
using System.Reflection;
using InternalLibrary.IO;

namespace InternalLibrary.Model.Bulilder
{
    /// <summary>
    /// Class UploadBuilder
    /// </summary>
    public class UploadBuilder
    {
        /// <summary>
        /// The user options
        /// </summary>
        private GlobalApplicationOptions userOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadBuilder"/> class.
        /// </summary>
        /// <param name="userOptions">The user options.</param>
        public UploadBuilder(GlobalApplicationOptions userOptions)
        {
            this.userOptions = userOptions;
        }

        //Check if there is a googleFileID and create update or upload request respectively
        /// <summary>
        /// Builds the upload request.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="googleFileID">The google file ID.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Google.Apis.Upload.ResumableUpload{FileFile}.</returns>
        public Google.Apis.Upload.ResumableUpload<File, File> buildUploadRequest(DriveService service, string googleFileID, System.IO.MemoryStream stream, string fileName)
        {
            File body;
            Google.Apis.Upload.ResumableUpload<File, File> request;

            body = this.buildFileBody(service, googleFileID, fileName);
            string mimeType = FileIO.GetMIMEType(fileName);
            if (!googleFileID.IsNullOrEmpty())
            {
                
                //Create an update request
                request = service.Files.Update(body, googleFileID, stream, mimeType);
                ((FilesResource.UpdateMediaUpload)request).NewRevision = this.userOptions.newRevision;
                return request;
            }
            else
            {
                //Create an upload request
                request = service.Files.Insert(body, stream, mimeType);
                return request;
            }
        }

        /// <summary>
        /// Builds the file body.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="googleFileID">The google file ID.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>File.</returns>
        private File buildFileBody(DriveService service, string googleFileID, string fileName)
        {
            File body;
            if (googleFileID.IsNullOrEmpty())
            {
                body = new File();
            }
            else
            {
                body = service.Files.Get(googleFileID).Execute();
            }
            body.Title = fileName;
            body.Description = "A test document";
            body.MimeType = FileIO.GetMIMEType(fileName);
            return body;
        }

        /// <summary>
        /// Builds the TMP file body.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="googleFileID">The google file ID.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>File.</returns>
        private File buildTMPFileBody(DriveService service, string googleFileID, string fileName)
        {
            File body = buildFileBody(service, googleFileID, fileName);
            //body.Labels = new File.LabelsData();
            //body.Labels.Hidden = true;
            return body;
        }

        ///// <summary>
        ///// Builds the service.
        ///// </summary>
        ///// <returns>DriveService.</returns>
        //public DriveService buildService()
        //{
        //    // Register the authenticator and create the service
        //    var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, GlobalApplicationOptions.CLIENT_ID, GlobalApplicationOptions.CLIENT_SECRET);
        //    var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, AuthenticationManager.GetAuthorization);
        //    var service = new DriveService(new BaseClientService.Initializer()
        //    {
        //        Authenticator = auth
        //    });

        //    return service;
        //}

    }
}
