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
    public class RequestController
    {
        DriveService service = null;
        UploadBuilder uploadBuilder;

        public RequestController(GlobalApplicationOptions userOptions)
        {
            uploadBuilder = new UploadBuilder(userOptions);
            this.service = uploadBuilder.buildService();
        }
        //Create request dependent objects
        //Build the request
        //Initiate the request
        //Return results
        public void uploadToGoogleDrive(object Document)
        {
            Microsoft.Office.Interop.Word.Document Doc = (Microsoft.Office.Interop.Word.Document)Document;
            try
            {
                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.Name, Doc.FullName);

                // Get Google File ID
                object customProperties = Doc.CustomDocumentProperties;
                Type customPropertiesType = customProperties.GetType();
                string googleFileID = FileIO.GetDocPropValue(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildRequest(service, googleFileID, stream, Doc.Name);

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
        public void uploadToGoogleDrive()
        {
            Microsoft.Office.Interop.Word.Document Doc = Globals.ThisAddIn.Application.ActiveDocument;
            try
            {
                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.Name, Doc.FullName);

                // Get Google File ID
                object customProperties = Doc.CustomDocumentProperties;
                Type customPropertiesType = customProperties.GetType();
                string googleFileID = FileIO.GetDocPropValue(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildRequest(service, googleFileID, stream, Doc.Name);

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
        public void initializeUploadToGoogleDrive()
        {
            try
            {
                // Create file
                System.IO.Directory.CreateDirectory(GlobalApplicationOptions.TMP_PATH);
                string fileName = Globals.ThisAddIn.Application.ActiveDocument.Name;
                string fullName = GlobalApplicationOptions.TMP_PATH + Globals.ThisAddIn.Application.ActiveDocument.Name + ".docx";
                System.IO.FileStream fileStream = System.IO.File.Create(fullName);
                fileStream.Close();

                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(fileName, fullName);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildRequest(service, null, stream, fileName);

                // Initiate request and handle response from the server
                request.Upload();
                File googleFile = request.ResponseBody;
                FileIO.SetDocPropValue(Globals.ThisAddIn.Application.ActiveDocument, googleFile.Id);
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

    }
}
