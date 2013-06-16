using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

using Google.Apis.Util;
using InternalLibrary.Data;
using System.Reflection;
using Microsoft.Win32;


namespace InternalLibrary.IO
{
    public class FileIO
    {
        //public static bool TmpUploadExists { get; set; }

        //Copy Word doc to tmp file for upload
        private static string createTmpCopy(string fileName, string fullFileLocation)
        {
            string tmpPath = GlobalApplicationOptions.TMP_PATH;
            string fileCopy = tmpPath + fileName + "DriveUploadTmp" + DateTime.Now.ToString().Replace('/', '.').Replace(' ', ',').Replace(':', '.');
            Directory.CreateDirectory(tmpPath);
            System.IO.File.Copy(fullFileLocation, fileCopy);

            return fileCopy;
        }

        private static string createTmpFile(string fileName)
        {
            string fullName = GlobalApplicationOptions.TMP_PATH + fileName + ".docx";
            System.IO.Directory.CreateDirectory(GlobalApplicationOptions.TMP_PATH);
            System.IO.FileStream fileStream = System.IO.File.Create(fullName);
            fileStream.Close();

            return fullName;
        }

        //private static string createEmptyTmpFile(string fileName)
        //{
        //    System.IO.Directory.CreateDirectory(GlobalApplicationOptions.TMP_PATH);
        //    object oFileName = GlobalApplicationOptions.TMP_PATH + fileName + ".docx";
        //    object addToRecentFiles = false;
        //    object isVisible = false;
        //    object missing = Missing.Value;
        //    Word._Document emptyDocument = Globals.ThisAddIn.Application.Documents.Add(ref missing, ref missing, ref missing, ref isVisible);
        //    emptyDocument.SaveAs2(ref oFileName, ref missing, ref missing, ref missing, ref addToRecentFiles, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
        //    emptyDocument.Close();
        //    return oFileName + "";
        //}

        //Create MemoryStream for file upload
        public static MemoryStream createMemoryStream(object CustomProps, string fileName, string fullFileLocation)
        {
            string file = "";
            if (uploadIDExists(CustomProps))
            {
                file = createTmpCopy(fileName, fullFileLocation);
            }
            else
            {
                file = createTmpFile(fileName);
            }

            byte[] byteArray = System.IO.File.ReadAllBytes(file);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            return stream;
        }

        public static void TearDown(){
            removeLocalTmpFolder();
        }

        private static void removeLocalTmpFolder()
        {
            if (Directory.Exists(GlobalApplicationOptions.TMP_PATH))
            {
                Directory.Delete(GlobalApplicationOptions.TMP_PATH, true);
            }
        }

        public static void SetDocPropValue(dynamic Doc, string propertyValue)
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

        private static void addDocProp(dynamic Doc, string propertyValue)
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

        public static String GetDocPropValue(object CustomProps)
        {
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

        public static bool uploadIDExists(object CustomProps)
        {
            if (GetDocPropValue(CustomProps) == null)
            {
                return false;
            }

            return true;
        }

        public static string GetMIMEType(string fileName)
        {
            try
            {
                // get the registry classes root
                RegistryKey classes = Registry.ClassesRoot;

                // find the sub key based on the file extension
                RegistryKey fileClass = classes.OpenSubKey(Path.GetExtension(fileName));
                string contentType = fileClass.GetValue("Content Type").ToString();

                return contentType;
            }
            catch (NullReferenceException nre)
            {
                return "text/plain";
            }
        }

    }
}
