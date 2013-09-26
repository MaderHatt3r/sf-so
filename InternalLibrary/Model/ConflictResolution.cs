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
                //try
                //{
                    ResolveNewRevision(Doc, prevFileID, prevRevisionID, googleFile);
                //}
                //catch (NullReferenceException) { }
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

            int firstRevisionIndex = revisions.IndexOf(revisions.First(r => r.Id == prevRevisionID));
            string firstRevision = null;
            for (int i = firstRevisionIndex; i < revisions.Count; i++)
            {
                string fullFilePath = ServiceRequestManagement.GetRequestManager.Save(revisions[i], "SFSO_TempMerge" + revisions[i].Id);
                revisionForks[fullFilePath] = revisions[i];
                if (i == firstRevisionIndex)
                {
                    firstRevision = fullFilePath;
                }
            }
            Microsoft.Office.Interop.Word.Application myApp = (Microsoft.Office.Interop.Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application"); ;
            //Microsoft.Office.Interop.Word.Application myApp = new Microsoft.Office.Interop.Word.Application();
            System.Threading.Thread.Sleep(1000);

            foreach (string revision in revisionForks.Keys)
            {
                //Microsoft.Office.Interop.Word.Document baseRevision = new Microsoft.Office.Interop.Word.Document(firstRevision); //.IsSubdocument;
                //Microsoft.Office.Interop.Word.Document individualRevision = new Microsoft.Office.Interop.Word.Document(revision);
                System.Threading.Thread.Sleep(1000);
                object missing = Type.Missing;
                Microsoft.Office.Interop.Word.Document baseRevision = myApp.Documents.Open(firstRevision, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);
                Microsoft.Office.Interop.Word.Document individualRevision = myApp.Documents.Open(revision, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);

                Microsoft.Office.Interop.Word.Document result = myApp.CompareDocuments(baseRevision, individualRevision, Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationRevised, Microsoft.Office.Interop.Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, revisionForks[revision].LastModifyingUser.DisplayName, false);

                baseRevision.Close(false, ref missing, ref missing);
                //result.Close(false, ref missing, ref missing);

                //Doc.Compare(revision, revisionForks[revision].LastModifyingUser.DisplayName, Microsoft.Office.Interop.Word.WdCompareTarget.wdCompareTargetSelected, true, false, false, false);
            }

            {
                string thisDocument = FileIO.createTmpCopy(Doc.Name, Doc.FullName);
                revisionForks[thisDocument] = null;
                System.Threading.Thread.Sleep(1000);
                object missing = Type.Missing;
                Microsoft.Office.Interop.Word.Document baseRevision = myApp.Documents.Open(firstRevision, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);
                Microsoft.Office.Interop.Word.Document individualRevision = myApp.Documents.Open(thisDocument, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);

                Microsoft.Office.Interop.Word.Document result = myApp.CompareDocuments(baseRevision, individualRevision, Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationRevised, Microsoft.Office.Interop.Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, "CTDragon", false);

                baseRevision.Close(false, ref missing, ref missing);
            }

            foreach (string update in revisionForks.Keys)
            {
                System.Threading.Thread.Sleep(1000);
                //object missing = Type.Missing;
                //Microsoft.Office.Interop.Word.Document baseRevision = myApp.Documents.Open(firstRevision, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);
                //Microsoft.Office.Interop.Word.Document individualRevision = myApp.Documents.Open(update, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, false, ref missing, ref missing, ref missing, ref missing);

                //myApp.MergeDocuments(baseRevision, individualRevision, Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationOriginal, Microsoft.Office.Interop.Word.WdGranularity.wdGranularityWordLevel, true, true, true, true, true, true, true, true, true, true, revisionForks[firstRevision].LastModifyingUser.DisplayName, revisionForks[update].LastModifyingUser.DisplayName, Microsoft.Office.Interop.Word.WdMergeFormatFrom.wdMergeFormatFromPrompt);

                //baseRevision.Close(true, ref missing, ref missing);
                //individualRevision.Close(false, ref missing, ref missing);

                Microsoft.Office.Interop.Word.Document baseRevision = new Microsoft.Office.Interop.Word.Document(firstRevision);
                Microsoft.Office.Interop.Word.Document individualRevision = new Microsoft.Office.Interop.Word.Document(update);
                Doc.Merge(update, Microsoft.Office.Interop.Word.WdMergeTarget.wdMergeTargetCurrent, true, Microsoft.Office.Interop.Word.WdUseFormattingFrom.wdFormattingFromPrompt, false);
            }

            foreach (Microsoft.Office.Interop.Word.Document document in myApp.Documents)
            {
                object missing = Type.Missing;
                if (!document.FullName.Equals(Doc.FullName))
                {
                    document.Close(false, ref missing, ref missing);
                }
            }
            
            //System.Threading.Thread.Sleep(1000);
            //object miss = Type.Missing;
            //Microsoft.Office.Interop.Word.Document endResult = myApp.Documents.Open(firstRevision, ref miss, ref miss, false, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, true, ref miss, ref miss, ref miss, ref miss);

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
