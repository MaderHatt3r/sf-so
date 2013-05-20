using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using SFSO.Data;
using Office = Microsoft.Office.Core;
using Google.Apis.Util;


namespace SFSO.Model
{
    class RequestBuilder
    {
        GlobalApplicationOptions userOptions;

        public RequestBuilder(ref GlobalApplicationOptions userOptions)
        {
            this.userOptions = userOptions;
        }

        private Google.Apis.Upload.ResumableUpload<File, File> buildRequest(File body, string googleFileID, System.IO.MemoryStream stream)
        {
            DriveService service = null;
            Google.Apis.Upload.ResumableUpload<File, File> request;
            if (googleFileID.IsNotNullOrEmpty())
            {
                //Create an upload request and initiate it
                request = service.Files.Update(body, googleFileID, stream, GlobalApplicationOptions.MIME_TYPE);
                ((FilesResource.UpdateMediaUpload)request).NewRevision = this.userOptions.newRevision;
                return request;
            }
            else
            {
                //Create an upload request and initiate it
                request = service.Files.Insert(body, stream, GlobalApplicationOptions.MIME_TYPE);
                return request;
            }
        }

    }
}
