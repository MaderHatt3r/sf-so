﻿using System;
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
    public class RequestController
    {
        DriveService service = null;
        UploadBuilder uploadBuilder;
        private string tmpUploadID;

        public RequestController(GlobalApplicationOptions userOptions)
        {
            uploadBuilder = new UploadBuilder(userOptions);
            this.service = uploadBuilder.buildService();
        }
        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        public void uploadToGoogleDrive(dynamic Doc)
        {
            try
            {
                // Get Google File ID
                string googleFileID = FileIO.GetDocPropValue(Doc.CustomDocumentProperties);

                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.CustomDocumentProperties, Doc.Name, Doc.FullName);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, googleFileID, stream, Doc.Name);

                // Initiate request and handle response from the server
                //FileIO.TmpUploadExists = false;
                request.Upload();
                //FileIO.TmpUploadExists = false;
                File googleFile = request.ResponseBody;
                FileIO.SetDocPropValue(Doc, googleFile.Id);
                this.tmpUploadID = null;
            }
            catch (OperationCanceledException oce)
            {
                //MessageBox.Show("Sync to Google Drive canceled by user");
            }
            //catch (Exception e)
            //{
            //    System.Windows.Forms.MessageBox.Show("A problem occurred while uploading" + Environment.NewLine +
            //        e.GetType().ToString() + Environment.NewLine + e.Message);
            //}
        }

        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        //public void uploadToGoogleDrive()
        //{
        //    Microsoft.Office.Interop.Word.Document Doc = Globals.ThisAddIn.Application.ActiveDocument;
        //    try
        //    {
        //        // Prepare document for upload
        //        System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.Name, Doc.FullName);

        //        // Get Google File ID
        //        string googleFileID = FileIO.GetDocPropValue();

        //        // Create request
        //        Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, googleFileID, stream, Doc.Name);

        //        // Initiate request and handle response from the server
        //        //FileIO.TmpUploadExists = false;
        //        request.Upload();
        //        //FileIO.TmpUploadExists = false;
        //        File googleFile = request.ResponseBody;
        //        FileIO.SetDocPropValue(Doc, googleFile.Id);
        //        this.tmpUploadID = null;
        //    }
        //    catch (OperationCanceledException oce)
        //    {
        //        //MessageBox.Show("Sync to Google Drive canceled by user");
        //    }
        //    catch (Exception e)
        //    {
        //        System.Windows.Forms.MessageBox.Show("A problem occurred while uploading" + Environment.NewLine +
        //            e.GetType().ToString() + Environment.NewLine + e.Message);
        //    }
        //}

        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        public void initializeUploadToGoogleDrive(dynamic Doc)
        {
            try
            {
                // Create file
                string fileName = "TMP";
                string fullName = null;

                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.CustomDocumentProperties, fileName, fullName);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, null, stream, fileName);

                request.Upload();
                File googleFile = request.ResponseBody;
                this.tmpUploadID = googleFile.Id;
                FileIO.SetDocPropValue(Doc, googleFile.Id);
            }
            catch (OperationCanceledException oce)
            {
                //MessageBox.Show("Sync to Google Drive canceled by user");
            }
            //catch (Exception e)
            //{
            //    System.Windows.Forms.MessageBox.Show("A problem occurred while uploading" + Environment.NewLine +
            //        e.GetType().ToString() + Environment.NewLine + e.Message);
            //}
        }

        public void removeTmpUpload()
        {
            ThreadTasks.WaitForRunningTasks();
            if (String.IsNullOrEmpty(this.tmpUploadID))
            {
                return;
            }
            string googleFileID = tmpUploadID;

            // Remove labels to prevent dangling pointers
            //ParentsResource.ListRequest listRequest = this.service.Parents.List(googleFileID);
            //ParentList labels = listRequest.Fetch();
            //foreach (ParentReference label in labels.Items)
            //{
            //    this.service.Children.Delete(label.Id, googleFileID);
            //}

            //System.Threading.Thread.Sleep(2000);

            // Trash file
            FilesResource.TrashRequest trashRequest = this.service.Files.Trash(googleFileID);
            File trashResponse = this.service.Files.Trash(googleFileID).Fetch();

            while (trashResponse == null)
            {
                continue;
            }
            
            //System.Threading.Thread.Sleep(2000);


            // Remove labels to prevent dangling pointers
            ParentsResource.ListRequest listRequest = this.service.Parents.List(googleFileID);
            ParentList labels = listRequest.Fetch();

            // Delete the trashed file
            //this.service.Files.Delete(FileIO.GetDocPropValue()).Fetch();

            // Delete the trashed file
            FilesResource.DeleteRequest deleteRequest = this.service.Files.Delete(googleFileID);
            deleteRequest.Fetch();
            

            foreach (ParentReference label in labels.Items)
            {
                this.service.Children.Delete(label.Id, googleFileID);
            }
        }
    }
}