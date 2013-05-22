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
        public void uploadToGoogleDrive(Microsoft.Office.Interop.Word.Document Doc)
        {
            try
            {
                // Prepare document for upload
                System.IO.MemoryStream stream = FileIO.createMemoryStream(Doc.Name, Doc.FullName);

                // Get Google File ID
                Office.DocumentProperties customProperties = (Office.DocumentProperties)Doc.CustomDocumentProperties;
                string googleFileID = this.getGoogleFileID(customProperties);

                // Create request
                Google.Apis.Upload.ResumableUpload<File, File> request = this.uploadBuilder.buildRequest(service, googleFileID, stream, Doc.Name);

                // Initiate request and handle response from the server
                request.Upload();
                File googleFile = request.ResponseBody;
                this.setGoogleFileID(Doc, googleFile.Id);
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

        private String getGoogleFileID(Office.DocumentProperties customProperties)
        {
            Office.DocumentProperty fileIDProperty = FileIO.getMetadataProperty(customProperties, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);
            //TODO: Extract this duplicate code
            if (fileIDProperty == null)
            {
                return null;
            }
            return fileIDProperty.Value;
        }

        private void setGoogleFileID(Word.Document doc, string newID)
        {
            FileIO.setMetadataProperty(doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME, newID);
        }

    }
}
