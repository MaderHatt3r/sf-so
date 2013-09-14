// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 09-07-2013
//
// Last Modified By : CTDragon
// Last Modified On : 09-07-2013
// ***********************************************************************
// <copyright file="ServiceRequestManagement.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Drive.v2;
using InternalLibrary.Controller;
using InternalLibrary.Model.Builder;

namespace InternalLibrary.Model.RequestManagement
{
    /// <summary>
    /// Class ServiceRequestManagement.
    /// </summary>
    public static class ServiceRequestManagement
    {
        /// <summary>
        /// The _service
        /// </summary>
        private static DriveService _service = ServiceBuilder.BuildService();

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>The service.</value>
        public static DriveService Service
        {
            get { return _service; }
            set { _service = value; }
        }


        /// <summary>
        /// Gets or sets the upload request manager.
        /// </summary>
        /// <value>The upload request manager.</value>
        public static UploadRequestManager UploadRequestManager { get; set; }
        /// <summary>
        /// Gets or sets the revision request manager.
        /// </summary>
        /// <value>The revision request manager.</value>
        public static RevisionRequestManager RevisionRequestManager { get; set; }
        
    }
}
