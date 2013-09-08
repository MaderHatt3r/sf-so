using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Drive.v2;
using InternalLibrary.Controller;
using InternalLibrary.Model.Builder;

namespace InternalLibrary.Model.RequestManagement
{
    public static class ServiceRequestManagement
    {
        private static DriveService _service = ServiceBuilder.BuildService();

        public static DriveService Service
        {
            get { return _service; }
            set { _service = value; }
        }


        public static UploadRequestManager UploadRequestManager { get; set; }
        public static RevisionRequestManager RevisionRequestManager { get; set; }
        
    }
}
