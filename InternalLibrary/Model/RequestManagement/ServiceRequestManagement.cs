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
        /// The _upload request manager
        /// </summary>
        private static UploadRequestManager _uploadRequestManager;
        /// <summary>
        /// Gets or sets the upload request manager.
        /// </summary>
        /// <value>The upload request manager.</value>
        public static UploadRequestManager UploadRequestManager
        {
            get
            {
                if (_uploadRequestManager == null)
                {
                    _uploadRequestManager = new UploadRequestManager(_service);
                }
                return _uploadRequestManager;
            }
            set { _uploadRequestManager = value; }
        }

        /// <summary>
        /// The _revision request manager
        /// </summary>
        private static RevisionRequestManager _revisionRequestManager;
        /// <summary>
        /// Gets or sets the revision request manager.
        /// </summary>
        /// <value>The revision request manager.</value>
        public static RevisionRequestManager RevisionRequestManager
        {
            get
            {
                if (_revisionRequestManager == null)
                {
                    _revisionRequestManager = new RevisionRequestManager(_service);
                }
                return _revisionRequestManager;
            }
            set { _revisionRequestManager = value; }
        }

        /// <summary>
        /// The _get request manager
        /// </summary>
        private static GetRequestManager _getRequestManager;
        /// <summary>
        /// Gets or sets the get request manager.
        /// </summary>
        /// <value>The get request manager.</value>
        public static GetRequestManager GetRequestManager
        {
            get
            {
                if (_getRequestManager == null)
                {
                    _getRequestManager = new GetRequestManager(_service);
                }
                return _getRequestManager;
            }
            set { _getRequestManager = value; }
        }

        
    }
}
