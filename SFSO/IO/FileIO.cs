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
            try
            {
                string tmpPath = GlobalApplicationOptions.TMP_PATH;
                string fileCopy = tmpPath + fileName + "DriveUploadTmp" + DateTime.Now.ToString().Replace('/', '.').Replace(' ', ',').Replace(':', '.');
                Directory.CreateDirectory(tmpPath);
                System.IO.File.Copy(fullFileLocation, fileCopy);

                return fileCopy;
            }
            catch (FileNotFoundException fnfe)
            {
                //return "";
                return createEmptyTmpFile(fileName);
            }
        }

        private static string createEmptyTmpFile(string fileName)
        {
            return "";
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
                typeDocCustomProps.InvokeMember("Item",
                              BindingFlags.Default |
                              BindingFlags.SetProperty,
                              null, CustomProps,
                              new object[] { GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME, propertyValue });

            }
            catch
            {
                addDocProp(Doc, propertyValue);
            }
        }

        private static void addDocProp(Word.Document Doc, string propertyValue)
        {
            object CustomProps = Doc.CustomDocumentProperties;
            Type typeDocCustomProps = CustomProps.GetType();

            object[] oArgs = {GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME,false,
                     Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString,
                     propertyValue};

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
                object property = typeDocCustomProps.InvokeMember("Item",
                                           BindingFlags.Default |
                                           BindingFlags.GetProperty,
                                           null, CustomProps,
                                           new object[] { GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME });
                Type typeDocAuthorProp = property.GetType();
                String strValue = typeDocAuthorProp.InvokeMember("Value",
                                           BindingFlags.Default |
                                           BindingFlags.GetProperty,
                                           null, property,
                                           new object[] { }).ToString();

                return strValue;
            }
            catch
            {
                return null;
            }

        }

    }
}
