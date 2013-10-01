// ***********************************************************************
// Assembly         : FirstWordAddIn
// Author           : CTDragon
// Created          : 05-15-2013
//
// Last Modified By : CTDragon
// Last Modified On : 05-15-2013
// ***********************************************************************
// <copyright file="GlobalApplicationOptions.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Holds user option properties and global applicaiton options</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternalLibrary.Data
{
    public enum ConflictResolutionOptions
    {
        PULL, MERGE, CREATE_NEW, FORCE_PUSH
    }

    /// <summary>
    /// Class GlobalApplicationOptions
    /// </summary>
    public static class GlobalApplicationOptions
    {
        /// <summary>
        /// The CLIEN t_ ID
        /// </summary>
        public const string CLIENT_ID = "641263753705.apps.googleusercontent.com";
        /// <summary>
        /// The CLIEN t_ SECRET
        /// </summary>
        public const string CLIENT_SECRET = "RHWZG1O8TtwJF0p0jl8WebYY";
        /// <summary>
        /// The WOR d_ MIM e_ TYPE
        /// </summary>
        public const string WORD_MIME_TYPE = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        /// <summary>
        /// The EXCE l_ MIM e_ TYPE
        /// </summary>
        public const string EXCEL_MIME_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        /// <summary>
        /// The SERVIC e_ PATH
        /// </summary>
        public static string SERVICE_PATH = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\SFSO\\Services\\";
        /// <summary>
        /// The SERVIC e_ FIL e_ NAME
        /// </summary>
        public const string SERVICE_FILE_NAME = "service";
        /// <summary>
        /// The GOOGL e_ FIL e_ I d_ PROPERT y_ NAME
        /// </summary>
        public const string GOOGLE_FILE_ID_PROPERTY_NAME = "GoogleFileID";
        public const string HEAD_REVISION_ID_PROPERTY_NAME = "HeadRevisionID";
        /// <summary>
        /// The TM p_ PATH
        /// </summary>
        public static string TMP_PATH = Environment.GetEnvironmentVariable("TMP") + "\\SFSO\\";
        public static TimeSpan ThreadTaskTimeout = new TimeSpan(1, 0, 0);

        // You should use a more secure way of storing the key here as
        // .NET applications can be disassembled using a reflection tool.
        /// <summary>
        /// The KEY
        /// </summary>
        public const string KEY = "g},zrztf11x9;98";

        /// <summary>
        /// Gets or sets a value indicating whether to save to a new revision in Google Drive or to replace the current head.
        /// </summary>
        /// <value><c>true</c> to save to a new revision; otherwise, <c>false</c>.</value>
        //public static bool newRevision { get; set; }

        private static bool _newRevision = true;
        /// <summary>
        /// Gets or sets a value indicating whether to save to a new revision in Google Drive or to replace the current head.
        /// </summary>
        /// <value><c>true</c> to save to a new revision; otherwise, <c>false</c>.</value>
        public static bool NewRevision
        {
            get { return _newRevision; }
            set { _newRevision = value; }
        }

        private static bool _syncFileNameOnChange = true;
        /// <summary>
        /// Gets or sets a value indicating whether [sync file name on change].
        /// </summary>
        /// <value><c>true</c> if [sync file name on change]; otherwise, <c>false</c>.</value>
        public static bool SyncFileNameOnChange
        {
            get { return _syncFileNameOnChange; }
            set { _syncFileNameOnChange = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [sync file name on change].
        /// </summary>
        /// <value><c>true</c> if [sync file name on change]; otherwise, <c>false</c>.</value>
        //public static bool syncFileNameOnChange { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalApplicationOptions" /> class.
        /// </summary>
        //public GlobalApplicationOptions()
        //{
        //    this.setUserOptions();
        //}

        ///// <summary>
        ///// Sets the user options.
        ///// </summary>
        //private void setUserOptions(){
        //    newRevision = true;
        //}
    }
}
