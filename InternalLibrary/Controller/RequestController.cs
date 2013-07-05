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
using InternalLibrary.Model;
using InternalLibrary.Data;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace InternalLibrary.Controller
{
    /// <summary>
    /// Class RequestController
    /// </summary>
    public class RequestController
    {
        /// <summary>
        /// The service
        /// </summary>
        DriveService service = null;
        /// <summary>
        /// The upload builder
        /// </summary>
        UploadBuilder uploadBuilder;
        /// <summary>
        /// The TMP upload ID
        /// </summary>
        private List<string> tmpUploadID = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestController"/> class.
        /// </summary>
        /// <param name="userOptions">The user options.</param>
        public RequestController(GlobalApplicationOptions userOptions)
        {
            uploadBuilder = new UploadBuilder(userOptions);
            this.service = uploadBuilder.buildService();
        }

        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        /// <summary>
        /// Uploads to google drive.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        public void updateDriveFile(dynamic Doc)
        {
            //try
            //{
                // Get Google File ID
                
                //this.checkForDocumentChanges(fileID);
                string newFileID = this.upload(Doc, Doc.Name, Doc.FullName);
                this.tmpUploadID.Remove(newFileID);
            //}
            //catch (OperationCanceledException)
            //{
            //    //MessageBox.Show("Sync to Google Drive canceled by user");
            //}
            //catch (Exception e)
            //{
            //    System.Windows.Forms.MessageBox.Show("A problem occurred uploading the file" + Environment.NewLine +
            //        e.GetType().ToString() + Environment.NewLine + e.Message);
            //}
        }

        private void checkForDocumentChanges(string fileID)
        {
            throw new NotImplementedException();
        }

        public void SpawnInitializeUploadThread(dynamic document, dynamic customProps)
        {
            if (!FileIO.uploadIDExists(customProps))
            {
                ThreadTasks.RunThread(() => this.initializeDriveFile(document));
            }
        }

        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        /// <summary>
        /// Initializes the upload to google drive.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        private void initializeDriveFile(dynamic Doc)
        {
            //try
            //{
                string fileName = "TMP";
                string fullName = null;

                string newFileID = upload(Doc, fileName, fullName);

                this.tmpUploadID.Add(newFileID);
            //}
            //catch (OperationCanceledException)
            //{
            //    //MessageBox.Show("Sync to Google Drive canceled by user");
            //}
            //catch (Exception e)
            //{
            //    System.Windows.Forms.MessageBox.Show("A problem initializing the upload" + Environment.NewLine +
            //        e.GetType().ToString() + Environment.NewLine + e.Message);
            //}
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
            string fileID = FileIO.GetDocPropValue(Doc.CustomDocumentProperties);

            // Prepare document for upload
            System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.CustomDocumentProperties, fileName, fullName);

            // Create request
            Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, fileID, stream, fileName);

            request.Upload();
            File googleFile = request.ResponseBody;
            
            FileIO.SetDocPropValue(Doc, googleFile.Id);

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

        private void removeTmpUpload(string googleFileID)
        {
            try
            {
                // Trash file
                FilesResource.TrashRequest trashRequest = this.service.Files.Trash(googleFileID);
                File trashResponse = this.service.Files.Trash(googleFileID).Fetch();

                while (trashResponse == null)
                {
                    continue;
                }

                // Wait for the File to actually move to the trash to avoid the dangling pointer issue
                bool? trashed = this.service.Files.Get(googleFileID).Fetch().Labels.Trashed;
                while (!trashed.HasValue || !trashed.Value)
                {
                    trashed = this.service.Files.Get(googleFileID).Fetch().Labels.Trashed;
                    continue;
                }
                System.Threading.Thread.Sleep(2000);


                // Remove labels to prevent dangling pointers
                //ParentsResource.ListRequest listRequest = this.service.Parents.List(googleFileID);
                //ParentList labels = listRequest.Fetch();

                // Delete the trashed file
                //this.service.Files.Delete(FileIO.GetDocPropValue()).Fetch();

                // Delete the trashed file
                FilesResource.DeleteRequest deleteRequest = this.service.Files.Delete(googleFileID);
                deleteRequest.Fetch();


                //foreach (ParentReference label in labels.Items)
                //{
                //    this.service.Children.Delete(label.Id, googleFileID);
                //}

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A problem initializing the upload" + Environment.NewLine +
                    e.GetType().ToString() + Environment.NewLine + e.Message);
            }
        }
        
    }
}
