using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Google.Apis.Util;
using SFSO.Data;
using System.Reflection;


namespace SFSO.IO
{
    public class FileIO
    {
        //Copy Word doc to tmp file for upload
        private static string createTmpFile(string fileName, string fullFileLocation)
        {
            string tmpPath = GlobalApplicationOptions.TMP_PATH;
            string fileCopy = tmpPath + fileName + "DriveUploadTmp" + DateTime.Now.ToString().Replace('/', '.').Replace(' ', ',').Replace(':', '.');
            Directory.CreateDirectory(tmpPath);
            System.IO.File.Copy(fullFileLocation, fileCopy);

            return fileCopy;
        }

        //Create MemoryStream for file upload
        public static MemoryStream createMemoryStream(string fileName, string fullFileLocation)
        {
            string fileCopy = createTmpFile(fileName, fullFileLocation);

            byte[] byteArray = System.IO.File.ReadAllBytes(fileCopy);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            return stream;
        }

        //Delete the temp word file
        private static void deleteTmpFile(string file)
        {
            System.IO.File.Delete(file);
        }

        //Append metadata to word document
        public static void setMetadataProperty(Word.Document doc, string propertyName, string value)
        {
            Microsoft.Office.Core.DocumentProperties customProperties = doc.CustomDocumentProperties;
            Office.DocumentProperty fileIDProperty = FileIO.getMetadataProperty(customProperties, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME);
            string propertyValue;
            //TODO: Extract this duplicate code
            if (fileIDProperty == null)
            {
                propertyValue = null;
            }
            else
            {
                propertyValue = getMetadataProperty(customProperties, propertyName).Value;
            }
            

            if (propertyValue.IsNullOrEmpty())
            {
                customProperties.Add(propertyName, false, Office.MsoDocProperties.msoPropertyTypeString, value);
            }
            else
            {
                getMetadataProperty(customProperties, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME).Value = value;
            }
            doc.Saved = false;
            doc.Save();
        }

        //Read metadata property from Word File
        public static Office.DocumentProperty getMetadataProperty(Office.DocumentProperties customProperties, string propertyName)
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

        public static void TearDown(){
            if (Directory.Exists(GlobalApplicationOptions.TMP_PATH))
            {
                Directory.Delete(GlobalApplicationOptions.TMP_PATH, true);
            }
        }

        public static void SetDocPropValue(Word.Document Doc, string propertyValue)
        {
            object CustomProps = Doc.CustomDocumentProperties;
            Type typeDocCustomProps = CustomProps.GetType();

            try
            {
                object DocCustomProp = typeDocCustomProps.InvokeMember(GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME,
                                           BindingFlags.Default |
                                           BindingFlags.GetProperty,
                                           null, CustomProps,
                                           new object[] { propertyValue });

                Type typeDocResProp = DocCustomProp.GetType();

                String strValue = typeDocResProp.InvokeMember(propertyValue,
                                           BindingFlags.Default |
                                           BindingFlags.SetProperty,
                                           null, DocCustomProp,
                                           new object[] { }).ToString();

            }
            catch
            {
                addDocProp(Doc, propertyValue);
            }
            //finally
            //{
            //    Doc.Saved = false;
            //    Doc.Save();
            //}
        }

        private static void addDocProp(Word.Document Doc, string propertyVale)
        {
            object CustomProps = Doc.CustomDocumentProperties;
            Type typeDocCustomProps = CustomProps.GetType();

            object[] oArgs = {GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME,false,
                     Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString,
                     propertyVale};

            typeDocCustomProps.InvokeMember("Add", BindingFlags.Default |
                                       BindingFlags.InvokeMethod, null,
                                       CustomProps, oArgs);
        }

        public static String GetDocPropValue(Word.Document Doc, string propertyValue)
        {
            object CustomProps = Doc.CustomDocumentProperties;
            Type typeDocCustomProps = CustomProps.GetType();

            try
            {
                object DocCustomProp = typeDocCustomProps.InvokeMember(GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME,
                                           BindingFlags.Default |
                                           BindingFlags.GetProperty,
                                           null, CustomProps,
                                           new object[] { propertyValue });

                Type typeDocResProp = DocCustomProp.GetType();

                String strValue = typeDocResProp.InvokeMember(propertyValue,
                                           BindingFlags.Default |
                                           BindingFlags.GetProperty,
                                           null, DocCustomProp,
                                           new object[] { }).ToString();

                return strValue;
            }
            catch
            {
                return null;
            }
            //if (DocCustomProp == null)
            //{
            //    return null;
            //}

        }

    }
}
