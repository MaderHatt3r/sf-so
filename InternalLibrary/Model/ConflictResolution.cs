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

        /// <summary>
        /// Checks for new saves.
        /// </summary>
        /// <param name="Doc">The document.</param>
        public static void CheckForNewSaves(dynamic Doc)
        {
            string prevFileID = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);
            string prevRevisionID = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.HEAD_REVISION_ID_PROPERTY_NAME);
            File googleFile = ServiceRequestManagement.GetRequestManager.GetMetadata(prevFileID);
            string newRevisionID = googleFile.HeadRevisionId;
            if (!string.IsNullOrEmpty(prevRevisionID) && !string.IsNullOrEmpty(newRevisionID) && prevRevisionID != newRevisionID)
            {
                try
                {
                    ResolveNewRevision(Doc, prevFileID, prevRevisionID, googleFile);
                }
                catch (NullReferenceException) { }
            }
        }

        /// <summary>
        /// Resolves the new revision.
        /// </summary>
        /// <param name="Doc">The document.</param>
        /// <param name="prevFileID">The previous file unique identifier.</param>
        /// <param name="googleFile">The google file.</param>
        private static void ResolveNewRevision(dynamic Doc, string prevFileID, string prevRevisionID, File googleFile)
        {
            IList<Revision> revisions = ServiceRequestManagement.RevisionRequestManager.GetRevisions(googleFile.Id);
            Dictionary<string, Revision> revisionForks = new Dictionary<string, Revision>();
            for (int i = revisions.IndexOf(revisions.First(r => r.Id == prevRevisionID))+1; i < revisions.Count; i++)
            {
                string fullFilePath = ServiceRequestManagement.GetRequestManager.Save(revisions[i], "SFSO_TempMerge" + revisions[i].Id);
                revisionForks[fullFilePath] = revisions[i];
            }

            foreach (string revision in revisionForks.Keys)
            {
                ThreadTasks.RunThread(() => Doc.Compare(revision, revisionForks[revision].LastModifyingUser.DisplayName, Microsoft.Office.Interop.Word.WdCompareTarget.wdCompareTargetSelected, true, false, false, false));
            }
            
            foreach (string update in revisionForks.Keys)
            {
                Doc.Merge(update, Microsoft.Office.Interop.Word.WdMergeTarget.wdMergeTargetCurrent, true, Microsoft.Office.Interop.Word.WdUseFormattingFrom.wdFormattingFromPrompt, false);
            }

            //string fileName = ServiceRequestManagement.GetRequestManager.Save(googleFile);
            //Doc.Compare(fileName, googleFile.LastModifyingUserName, Microsoft.Office.Interop.Word.WdCompareTarget.wdCompareTargetCurrent, true, false, false, false);
            //Doc.Merge(fileName, Microsoft.Office.Interop.Word.WdMergeTarget.wdMergeTargetCurrent, true, Microsoft.Office.Interop.Word.WdUseFormattingFrom.wdFormattingFromPrompt, false);
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
