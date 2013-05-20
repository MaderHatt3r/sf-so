using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Google.Apis.Util;
using SFSO.Data;


namespace SFSO.IO
{
    class FileIO
    {
        //Copy Word doc to tmp file for upload
        private string createTmpFile(string fileName)
        {
            String fileCopy = Environment.GetEnvironmentVariable("TMP") + doc.Name + "DriveUploadTmp" + DateTime.Now.ToString().Replace('/', '.').Replace(' ', ',').Replace(':', '.');
            System.IO.File.Copy(fileName, fileCopy);

            return fileCopy;
        }

        //Create MemoryStream for file upload
        public MemoryStream createMemoryStream(string file)
        {
            string fileCopy = this.createTmpFile(file);
            try
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(fileCopy);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                System.IO.File.Delete(file);

                return stream;
            }
            finally
            {
                System.IO.File.Delete(file);
            }
        }

        //Delete the temp word file
        private void deleteTmpFile(string file)
        {
            System.IO.File.Delete(file);
        }

        //Append metadata to word document
        public void setMetadataProperty(Word.Document doc, string propertyName, string value)
        {
            Microsoft.Office.Core.DocumentProperties customProperties = doc.CustomDocumentProperties;
            string googleFileID = getMetadataProperty(customProperties, );

            if (googleFileID.IsNullOrEmpty())
            {
                customProperties.Add(propertyName, false, Office.MsoDocProperties.msoPropertyTypeString, value);
            }
            else
            {
                this.getMetadataProperty(customProperties, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME).Value = newID;
            }
            doc.Saved = false;
            doc.Save();
        }

        //Move this to controller
        public void setGoogleFileID(Word.Document doc, string newID)
        {
            setMetadataProperty(doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME, newID);
        }

        //Read metadata property from Word File
        private Office.DocumentProperty getMetadataProperty(Office.DocumentProperties customProperties, string propertyName)
        {
            foreach (Office.DocumentProperty property in customProperties)
            {
                if (property.Name.Equals(propertyName))
                {
                    return property;
                }
            }
            return null;
        }

        //Move this to controller
        private string getGoogleFileID(Office.DocumentProperties customProperties)
        {
            return getMetadataProperty(customProperties, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME).Value;
        }
    }
}
