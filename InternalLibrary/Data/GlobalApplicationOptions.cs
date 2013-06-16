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
    public class GlobalApplicationOptions
    {
        public const string CLIENT_ID = "641263753705.apps.googleusercontent.com";
        public const string CLIENT_SECRET = "RHWZG1O8TtwJF0p0jl8WebYY";
        public const string WORD_MIME_TYPE = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string EXCEL_MIME_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string SERVICE_PATH = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\SFSO\\Services\\";
        public const string SERVICE_FILE_NAME = "service";
        public const string GOOGLE_FILE_ID_PROPERTY_NAME = "GoogleFileID";
        public static string TMP_PATH = Environment.GetEnvironmentVariable("TMP") + "\\SFSO\\";

        // You should use a more secure way of storing the key here as
        // .NET applications can be disassembled using a reflection tool.
        public const string KEY = "g},zrztf11x9;98";

        /// <summary>
        /// Gets or sets a value indicating whether to save to a new revision in Google Drive or to replace the current head.
        /// </summary>
        /// <value><c>true</c> to save to a new revision; otherwise, <c>false</c>.</value>
        public bool newRevision { get; set; }
        public bool syncFileNameOnChange { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalApplicationOptions"/> class.
        /// </summary>
        public GlobalApplicationOptions()
        {
            this.setUserOptions();
        }

        private void setUserOptions(){
            newRevision = true;
        }
    }
}
