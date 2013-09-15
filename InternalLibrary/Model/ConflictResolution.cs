// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 09-07-2013
//
// Last Modified By : CTDragon
// Last Modified On : 09-07-2013
// ***********************************************************************
// <copyright file="ConflictResolution.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Drive.v2.Data;
using InternalLibrary.Data;
using InternalLibrary.IO;
using InternalLibrary.Model.RequestManagement;

namespace InternalLibrary.Model
{
    /// <summary>
    /// Class ConflictResolution.
    /// </summary>
    public class ConflictResolution
    {

        public static void CheckForNewSaves(dynamic Doc)
        {
            string fileID = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);
            string currentRevisionID = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.HEAD_REVISION_ID_PROPERTY_NAME);
            File googleFile = ServiceRequestManagement.GetRequestManager.GetMetadata(fileID);
            string newRevisionID = googleFile.HeadRevisionId;
            if (!string.IsNullOrEmpty(currentRevisionID) && !string.IsNullOrEmpty(newRevisionID) && currentRevisionID != newRevisionID)
            {
                ResolveNewRevision(Doc, fileID, googleFile);
            }
        }

        private static void ResolveNewRevision(dynamic Doc, string fileID, File googleFile)
        {

            string fileName = ServiceRequestManagement.GetRequestManager.Save(googleFile);
            //Doc.Compare(fileName, googleFile.LastModifyingUserName, Microsoft.Office.Interop.Word.WdCompareTarget.wdCompareTargetCurrent, true, false, false, false);
            
            Doc.Merge(fileName, Microsoft.Office.Interop.Word.WdMergeTarget.wdMergeTargetCurrent, true, Microsoft.Office.Interop.Word.WdUseFormattingFrom.wdFormattingFromPrompt, false);
        }

        public static void MergeNewSave()
        {

        }

        /// <summary>
        /// Checks for conflicts.
        /// </summary>
        /// <param name="fileID">The file unique identifier.</param>
        /// <param name="previousRevisionID">The previous revision unique identifier.</param>
        /// <param name="nextRevisionID">The next revision unique identifier.</param>
        /// <exception cref="System.InvalidOperationException">The head revision was overwritten without a proper merge.</exception>
        public static void CheckForConflicts(string fileID, string previousRevisionID, string nextRevisionID)
        {
            if (!string.IsNullOrEmpty(nextRevisionID) && !string.IsNullOrEmpty(previousRevisionID))
            {
                if (!ServiceRequestManagement.RevisionRequestManager.IsRevisionSequential(fileID, previousRevisionID, nextRevisionID))
                {
                    //throw new InvalidOperationException("The head revision was overwritten without a proper merge.");
                }
            }
        }


    }
}
