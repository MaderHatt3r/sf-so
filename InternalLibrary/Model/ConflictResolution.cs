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
                ResolveNewRevision(Doc, prevFileID, prevRevisionID, googleFile);
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
            InternalLibrary.Forms.ConflictingVersionDialog dialog = new InternalLibrary.Forms.ConflictingVersionDialog();
            dialog.ShowDialog();
            ConflictResolutionOptions result = dialog.UserSelection;

            switch (result)
            {
                case ConflictResolutionOptions.PULL:
                    System.Windows.Forms.MessageBox.Show("This feature is not yet implemented. Please download the latest version from Google Drive until this feature is developed.");
                    break;
                case ConflictResolutionOptions.MERGE:
                    MergeRevisions(Doc, prevRevisionID, googleFile);
                    break;
                case ConflictResolutionOptions.CREATE_NEW:
                    System.Windows.Forms.MessageBox.Show("This feature is not yet available.");
                    break;
                case ConflictResolutionOptions.FORCE_PUSH:
                    break;
                default:
                    break;
            }
        }

        private static void MergeRevisions(dynamic Doc, string prevRevisionID, File googleFile)
        {
            IList<Revision> revisions = ServiceRequestManagement.RevisionRequestManager.GetRevisions(googleFile.Id);
            Dictionary<string, Revision> revisionForks = new Dictionary<string, Revision>();

            int firstRevisionIndex = revisions.IndexOf(revisions.First(r => r.Id == prevRevisionID));
            string firstRevision = null;
            string lastRevision = null;
            for (int i = firstRevisionIndex; i < revisions.Count; i++)
            {
                string fullFilePath = ServiceRequestManagement.GetRequestManager.Save(revisions[i], "SFSO_TempMerge" + revisions[i].Id);
                revisionForks[fullFilePath] = revisions[i];
                if (i == firstRevisionIndex)
                {
                    firstRevision = fullFilePath;
                }
                if (i == revisions.Count-1)
                {
                    lastRevision = fullFilePath;
                }
            }
            Microsoft.Office.Interop.Word.Application myApp = (Microsoft.Office.Interop.Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application"); ;

            {
                object missing = Type.Missing;
                Microsoft.Office.Interop.Word.Document baseRevision = myApp.Documents.Open(firstRevision, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);

                //Microsoft.Office.Interop.Word.Document result = myApp.CompareDocuments(baseRevision, Doc, Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationRevised, Microsoft.Office.Interop.Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, "CTDragon", false);
                myApp.MergeDocuments(baseRevision, Doc, Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationRevised, Microsoft.Office.Interop.Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, revisionForks[firstRevision].LastModifyingUser.DisplayName, "CTDragon", Microsoft.Office.Interop.Word.WdMergeFormatFrom.wdMergeFormatFromRevised);
                baseRevision.Close(false, ref missing, ref missing);
            }

            //List<Microsoft.Office.Interop.Word.Range> endChanges = new List<Microsoft.Office.Interop.Word.Range>();
            //foreach (string revision in revisionForks.Keys)
            {
                object missing = Type.Missing;
                Microsoft.Office.Interop.Word.Document baseRevision = myApp.Documents.Open(firstRevision, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);
                Microsoft.Office.Interop.Word.Document individualRevision = myApp.Documents.Open(lastRevision, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);

                //Microsoft.Office.Interop.Word.Document result = myApp.CompareDocuments(baseRevision, individualRevision, Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationRevised, Microsoft.Office.Interop.Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, revisionForks[revision].LastModifyingUser.DisplayName, false);
                Microsoft.Office.Interop.Word.Document result = myApp.MergeDocuments(baseRevision, individualRevision, Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationRevised, Microsoft.Office.Interop.Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, revisionForks[firstRevision].LastModifyingUser.DisplayName, revisionForks[lastRevision].LastModifyingUser.DisplayName, Microsoft.Office.Interop.Word.WdMergeFormatFrom.wdMergeFormatFromRevised);
                foreach (Microsoft.Office.Interop.Word.Revision change in result.Revisions)
                {
                    if (change.Range.Start >= baseRevision.Content.End-1)
                    {
                        change.Range.Copy();
                        Doc.Range(Doc.Content.End - 1, Doc.Content.End).Paste();
                        change.Reject();
                    }
                }

                baseRevision.Close(false, ref missing, ref missing);
            }

            foreach (string update in revisionForks.Keys)
            {
                Doc.Merge(update, Microsoft.Office.Interop.Word.WdMergeTarget.wdMergeTargetCurrent, true, Microsoft.Office.Interop.Word.WdUseFormattingFrom.wdFormattingFromPrompt, false);
            }

            foreach (Microsoft.Office.Interop.Word.Document document in myApp.Documents)
            {
                object missing = Type.Missing;
                if (document.FullName.Contains(lastRevision))
                {
                    document.Close(false, ref missing, ref missing);
                }
            }
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
