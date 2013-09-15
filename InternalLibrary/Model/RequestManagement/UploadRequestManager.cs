// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 06-13-2013
//
// Last Modified By : CTDragon
// Last Modified On : 06-16-2013
// ***********************************************************************
// <copyright file="RequestController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using InternalLibrary.IO;
using InternalLibrary.Data;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using InternalLibrary.Model.Bulilder;

namespace InternalLibrary.Model.RequestManagement
{
    /// <summary>
    /// Class RequestController
    /// </summary>
    public class UploadRequestManager
    {
        /// <summary>
        /// The service
        /// </summary>
        private DriveService service = null;
        /// <summary>
        /// The upload builder
        /// </summary>
        private UploadBuilder uploadBuilder;
        /// <summary>
        /// The TMP upload ID
        /// </summary>
        private List<string> tmpUploadID = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadRequestManager"/> class.
        /// </summary>
        /// <param name="userOptions">The user options.</param>
        public UploadRequestManager(DriveService service)
        {
            uploadBuilder = new UploadBuilder();
            this.service = service;
        }

        /// <summary>
        /// Uploads to google drive.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        public void updateDriveFile(dynamic Doc)
        {
            string newFileID = this.upload(Doc, Doc.Name, Doc.FullName);
            this.tmpUploadID.Remove(newFileID);
        }

        /// <summary>
        /// Spawns the initialize upload thread.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="customProps">The custom props.</param>
        public void SpawnInitializeUploadThread(dynamic document, dynamic customProps)
        {
            if (!FileIO.uploadIDExists(document))
            {
                ThreadTasks.RunThread(() => this.initializeDriveFile(document));
            }
        }

        /// <summary>
        /// Initializes the upload to google drive.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        private void initializeDriveFile(dynamic Doc)
        {
            string fileName = "TMP";
            string fullName = null;

            string newFileID = upload(Doc, fileName, fullName);

            this.tmpUploadID.Add(newFileID);
        }

        /// <summary>
        /// Uploads the specified doc.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns>System.String google file id returned from the upload</returns>
        private string upload(dynamic Doc, string fileName, string fullName)
        {
            // Get Google File ID
            string fileID = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);

            // Prepare document for upload
            System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc, fileName, fullName);

            // Create request
            Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, fileID, stream, fileName);

            request.Upload();
            File googleFile = request.ResponseBody;
            
            FileIO.SetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME, googleFile.Id);
            string previousRevision = FileIO.GetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.HEAD_REVISION_ID_PROPERTY_NAME);
            FileIO.SetDocPropValue_ThreadSafe(Doc, GlobalApplicationOptions.HEAD_REVISION_ID_PROPERTY_NAME, googleFile.HeadRevisionId);
            ConflictResolution.CheckForConflicts(googleFile.Id, previousRevision, googleFile.HeadRevisionId);

            return googleFile.Id;
        }

        /// <summary>
        /// Removes the TMP upload.
        /// <Postcondition>Spawns threads. Should wait for threads before closing application.</Postcondition>
        /// </summary>
        public void removeTmpUpload()
        {
            ThreadTasks.WaitForRunningTasks();

            foreach (string googleFileID in this.tmpUploadID)
            {
                ThreadTasks.RunThreadUnmanaged(new System.Threading.Tasks.Task(() => removeTmpUpload(googleFileID)));
            }
        }

        /// <summary>
        /// Removes the TMP upload.
        /// </summary>
        /// <param name="googleFileID">The google file ID.</param>
        private void removeTmpUpload(string googleFileID)
        {
            try
            {
                // Trash file
                FilesResource.TrashRequest trashRequest = this.service.Files.Trash(googleFileID);
                File trashResponse = this.service.Files.Trash(googleFileID).Execute();

                while (trashResponse == null)
                {
                    continue;
                }

                this.waitForTrashCompletion(googleFileID);

                // Delete the trashed file
                FilesResource.DeleteRequest deleteRequest = this.service.Files.Delete(googleFileID);
                deleteRequest.Execute();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A problem occurred removing TMP file:" + Environment.NewLine +
                    e.GetType().ToString() + Environment.NewLine + e.Message);
            }
        }

        /// <summary>
        /// Waits for trash completion.
        /// </summary>
        /// <param name="googleFileID">The google file ID.</param>
        private void waitForTrashCompletion(string googleFileID)
        {
            // Wait for the File to actually move to the trash to avoid the dangling pointer issue
            bool? trashed = this.service.Files.Get(googleFileID).Execute().Labels.Trashed;
            while (!trashed.HasValue || !trashed.Value)
            {
                trashed = this.service.Files.Get(googleFileID).Execute().Labels.Trashed;
                continue;
            }
            System.Threading.Thread.Sleep(100);
        }
        
    }
}
