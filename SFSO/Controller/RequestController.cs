using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using SFSO.IO;
using Office = Microsoft.Office.Core;
using SFSO.Model;
using SFSO.Data;

using Word = Microsoft.Office.Interop.Word;
using System.Reflection;

namespace SFSO.Controller
{
    internal class RequestController
    {
        DriveService service = null;
        UploadBuilder uploadBuilder;

        internal RequestController(GlobalApplicationOptions userOptions)
        {
            uploadBuilder = new UploadBuilder(userOptions);
            this.service = uploadBuilder.buildService();
        }
        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        internal void uploadToGoogleDrive(object Document)
        {
            Microsoft.Office.Interop.Word.Document Doc = (Microsoft.Office.Interop.Word.Document)Document;
            try
            {
                // Get Google File ID
                string googleFileID = FileIO.GetDocPropValue();

                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.Name, Doc.FullName);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, googleFileID, stream, Doc.Name);

                // Initiate request and handle response from the server
                request.Upload();
                File googleFile = request.ResponseBody;
                FileIO.SetDocPropValue(Doc, googleFile.Id);
            }
            catch (OperationCanceledException oce)
            {
                //MessageBox.Show("Sync to Google Drive canceled by user");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A problem occurred while uploading" + Environment.NewLine +
                    e.GetType().ToString() + Environment.NewLine + e.Message);
            }
        }

        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        internal void uploadToGoogleDrive()
        {
            Microsoft.Office.Interop.Word.Document Doc = Globals.ThisAddIn.Application.ActiveDocument;
            try
            {
                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.Name, Doc.FullName);

                // Get Google File ID
                string googleFileID = FileIO.GetDocPropValue();

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, googleFileID, stream, Doc.Name);

                // Initiate request and handle response from the server
                request.Upload();
                File googleFile = request.ResponseBody;
                FileIO.SetDocPropValue(Doc, googleFile.Id);
            }
            catch (OperationCanceledException oce)
            {
                //MessageBox.Show("Sync to Google Drive canceled by user");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A problem occurred while uploading" + Environment.NewLine +
                    e.GetType().ToString() + Environment.NewLine + e.Message);
            }
        }

        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        internal void initializeUploadToGoogleDrive()
        {
            try
            {
                // Create file
                string fileName = Globals.ThisAddIn.Application.ActiveDocument.Name;
                string fullName = null;

                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(fileName, fullName);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildUploadRequest(service, null, stream, fileName);

                // Initiate request and handle response from the server
                System.Threading.Thread.CurrentThread.Suspend();

                request.Upload();
                File googleFile = request.ResponseBody;
                FileIO.SetDocPropValue(Globals.ThisAddIn.Application.ActiveDocument, googleFile.Id);
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

        internal void removeTmpUpload()
        {
            
            string googleFileID = FileIO.GetDocPropValue();

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
            //FilesResource.DeleteRequest deleteRequest = this.service.Files.Delete(googleFileID);
            //deleteRequest.Fetch();
            

            foreach (ParentReference label in labels.Items)
            {
                this.service.Children.Delete(label.Id, googleFileID);
            }
        }
    }
}
