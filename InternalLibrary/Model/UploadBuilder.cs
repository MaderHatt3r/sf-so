using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using SFSO.Data;
using Google.Apis.Util;

//For build service
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Services;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;

using System.Diagnostics;
using DotNetOpenAuth.OAuth2;
using System.Security.Cryptography;
using System.Reflection;
using SFSO.IO;

namespace SFSO.Model
{
    internal class UploadBuilder
    {
        private GlobalApplicationOptions userOptions;

        internal UploadBuilder(GlobalApplicationOptions userOptions)
        {
            this.userOptions = userOptions;
        }

        //Check if there is a googleFileID and create update or upload request respectively
        internal Google.Apis.Upload.ResumableUpload<File, File> buildUploadRequest(DriveService service, string googleFileID, System.IO.MemoryStream stream, string fileName)
        {
            File body;
            Google.Apis.Upload.ResumableUpload<File, File> request;

            body = this.buildFileBody(service, googleFileID, fileName);
            string mimeType = FileIO.GetMIMEType(fileName);
            if (googleFileID.IsNotNullOrEmpty())
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

        private File buildFileBody(DriveService service, string googleFileID, string fileName)
        {
            File body;
            if (googleFileID.IsNullOrEmpty())
            {
                body = new File();
            }
            else
            {
                body = service.Files.Get(googleFileID).Fetch();
            }
            body.Title = fileName;
            body.Description = "A test document";
            body.MimeType = FileIO.GetMIMEType(fileName);
            return body;
        }

        private File buildTMPFileBody(DriveService service, string googleFileID, string fileName)
        {
            File body = buildFileBody(service, googleFileID, fileName);
            //body.Labels = new File.LabelsData();
            //body.Labels.Hidden = true;
            return body;
        }

        internal DriveService buildService()
        {
            // Register the authenticator and create the service
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, GlobalApplicationOptions.CLIENT_ID, GlobalApplicationOptions.CLIENT_SECRET);
            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, AuthenticationManager.GetAuthorization);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                Authenticator = auth
            });

            return service;
        }

    }
}
