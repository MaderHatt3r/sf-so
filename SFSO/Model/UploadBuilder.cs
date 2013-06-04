using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using SFSO.Data;
using Office = Microsoft.Office.Core;
using Google.Apis.Util;

//For build service
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Services;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;

using System.Diagnostics;
using DotNetOpenAuth.OAuth2;
using System.Security.Cryptography;
using System.Reflection;

namespace SFSO.Model
{
    public class UploadBuilder
    {
        GlobalApplicationOptions userOptions;

        public UploadBuilder(GlobalApplicationOptions userOptions)
        {
            this.userOptions = userOptions;
        }

        //Check if there is a googleFileID and create update or upload request respectively
        public Google.Apis.Upload.ResumableUpload<File, File> buildUploadRequest(DriveService service, string googleFileID, System.IO.MemoryStream stream, string fileName)
        {
            File body = this.buildFileBody(service, googleFileID, fileName);
            Google.Apis.Upload.ResumableUpload<File, File> request;
            if (googleFileID.IsNotNullOrEmpty())
            {
                //Create an upload request
                request = service.Files.Update(body, googleFileID, stream, GlobalApplicationOptions.MIME_TYPE);
                ((FilesResource.UpdateMediaUpload)request).NewRevision = this.userOptions.newRevision;
                return request;
            }
            else
            {
                //Create an upload request
                request = service.Files.Insert(body, stream, GlobalApplicationOptions.MIME_TYPE);
                return request;
            }
        }

        public File buildFileBody(DriveService service, string googleFileID, string fileName)
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
            body.MimeType = GlobalApplicationOptions.MIME_TYPE;
            return body;
        }

        public DriveService buildService()
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
