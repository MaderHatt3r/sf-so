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
    public class ConflictResolution_E
    {
        private bool _allowSaves;

        private bool AllowSaves
        {
            get { return _allowSaves; }
            // If the value is at any point set to false, we don't want to re-assign it as true
            set { _allowSaves = _allowSaves && value; }
        }

        private ConflictResolutionOptions _userSelection;

        public ConflictResolutionOptions UserSelection
        {
            get { return _userSelection; }
            set { _userSelection = value; }
        }


        public ConflictResolution_E()
        {
            _allowSaves = true;
        }

        /// <summary>
        /// Checks for new saves.
        /// </summary>
        /// <param name="Doc">The document.</param>
        /// <returns><c>true</c> if save allowed, <c>false</c> otherwise.</returns>
        public bool CheckForNewSaves(dynamic Doc)
        {
            if (!FileIO.uploadIDExists_ThreadSafe(Doc))
            {
                return false;
            }

            string prevFileID = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);
            string prevRevisionID = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.HEAD_REVISION_ID_PROPERTY_NAME);
            File googleFile = ServiceRequestManagement.GetRequestManager.GetMetadata(prevFileID);
            string newRevisionID = googleFile.HeadRevisionId;

            if (!string.IsNullOrEmpty(prevRevisionID) && !string.IsNullOrEmpty(newRevisionID) && prevRevisionID != newRevisionID)
            {
                ResolveNewRevision(Doc, prevFileID, prevRevisionID, prevFileID, googleFile);
            }

            return AllowSaves;
        }

        /// <summary>
        /// Resolves the new revision.
        /// </summary>
        /// <param name="Doc">The document.</param>
        /// <param name="prevFileID">The previous file unique identifier.</param>
        /// <param name="googleFile">The google file.</param>
        private void ResolveNewRevision(dynamic Doc, string prevFileID, string prevRevisionID, string fileID, File googleFile)
        {
            ConflictResolutionOptions result = GlobalApplicationOptions.OverrideConflictResolutionDialogResult ?? PromptDialogResult();
            UserSelection = result;

            switch (result)
            {
                case ConflictResolutionOptions.PULL:
                    PullLatest(Doc, fileID, googleFile.Title);
                    AllowSaves = false;
                    //System.Windows.Forms.MessageBox.Show("This feature is not yet implemented. Please download the latest version from Google Drive until this feature is developed.");
                    break;
                case ConflictResolutionOptions.MERGE:
                    MergeRevisions(Doc, prevRevisionID, fileID);
                    AllowSaves = false;
                    break;
                case ConflictResolutionOptions.CREATE_NEW:
                    System.Windows.Forms.MessageBox.Show("This feature is not yet available.");
                    break;
                case ConflictResolutionOptions.FORCE_PUSH:
                    break;
                default:
                    AllowSaves = false;
                    break;
            }
        }

        private ConflictResolutionOptions PromptDialogResult()
        {
            InternalLibrary.Forms.ConflictingVersionDialog dialog = new InternalLibrary.Forms.ConflictingVersionDialog();
            dialog.ShowDialog();
            return dialog.UserSelection;
        }

        private void MergeRevisions(dynamic Doc, string prevRevisionID, string fileID)
        {
            // Load revisions
            IList<Revision> revisions = ServiceRequestManagement.RevisionRequestManager.GetRevisions(fileID);

            // Put revisions into a dictionary and store the key to the base and other user's last revisions
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
                if (i == revisions.Count - 1)
                {
                    lastRevision = fullFilePath;
                }
            }

            // Load the excel app and disable screen updating for performance and visual effect
            //Microsoft.Office.Interop.Excel.Application myApp = (Microsoft.Office.Interop.Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            //myApp.ScreenUpdating = false;

            // Merges the base revision into the local version
            //ThreadTasks.RunThread(() =>
            //{
            //    ThreadTasks.ActionProtectOfficeObjectModel(() =>
            //    {
            //        System.Threading.Thread.Sleep(1000);
            //        string localDocCopy = FileIO.createTmpCopy(Doc.Name, Doc.FullName);

            //        Microsoft.Office.Interop.Excel.Workbook baseRevision = myApp.Workbooks.Open(firstRevision);
            //        baseRevision.MergeWorkbook(localDocCopy);

            //        Doc.MergeWorkbook(firstRevision);
            //});

            //string localDocCopy = FileIO.createTmpCopy(Doc.Name, Doc.FullName);

            //Microsoft.Office.Interop.Excel.Workbook baseRevision = myApp.Workbooks.Open(firstRevision);
            //baseRevision.MergeWorkbook(localDocCopy);

            //Doc.MergeWorkbook(firstRevision);


                //Doc.MergeWorkbook(lastRevision);


            new System.Threading.Tasks.Task(() =>
            {
                while (GlobalApplicationOptions.HandlerBusy) { continue; }
                System.Threading.Thread.Sleep(50);
                GlobalApplicationOptions.OverrideConflictResolutionDialogResult = ConflictResolutionOptions.FORCE_PUSH;
                Doc.MergeWorkbook(lastRevision); // Side-effect (save && save event is called)
                GlobalApplicationOptions.OverrideConflictResolutionDialogResult = null;
                //myApp.ScreenUpdating = true;
            }).Start();

            AllowSaves = false;
        }

        private void PullLatest(dynamic Doc, string fileID, string p)
        {
            string newVersion = ServiceRequestManagement.GetRequestManager.Save(fileID, Doc.Name);

            object missing = Type.Missing;
            object doNotSave = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;

            Microsoft.Office.Interop.Excel.Application myApp = (Microsoft.Office.Interop.Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Microsoft.Office.Interop.Excel.Workbook pulledDocument = myApp.Workbooks.Open(newVersion);

            new System.Threading.Tasks.Task(() =>
            {
                while (GlobalApplicationOptions.HandlerBusy) { continue; }
                System.Threading.Thread.Sleep(10);
                GlobalApplicationOptions.HandlerBusy = true;
                string destinationFileName = Doc.FullName;
                Doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
                //Copy pulledDocument from temp to Doc directory
                string relocatedDoc = FileIO.copyFile(newVersion, destinationFileName);
                //Open the copied document
                Microsoft.Office.Interop.Excel.Workbook relocatedDocument = myApp.Workbooks.Open(relocatedDoc);
                //Close the temp document
                pulledDocument.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
                GlobalApplicationOptions.HandlerBusy = false;
            }).Start();
        }


    }
}
