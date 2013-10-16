// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 06-13-2013
//
// Last Modified By : CTDragon
// Last Modified On : 06-13-2013
// ***********************************************************************
// <copyright file="FileIO.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class FileIO
    /// </summary>
    public class FileIO
    {
        //Copy Word doc to tmp file for upload
        /// <summary>
        /// Creates the TMP copy.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fullFileLocation">The full file location.</param>
        /// <returns>System.String.</returns>
        public static string createTmpCopy(string fileName, string fullFileLocation)
        {
            string tmpPath = GlobalApplicationOptions.TMP_PATH;
            string fileCopy = tmpPath + fileName + "DriveUploadTmp" + DateTime.Now.ToString().Replace('/', '.').Replace(' ', ',').Replace(':', '.');
            Directory.CreateDirectory(tmpPath);
            System.IO.File.Copy(fullFileLocation, fileCopy);

            return fileCopy;
        }

        /// <summary>
        /// Creates the TMP file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        private static string createTmpFile(string fileName)
        {
            string fullName = GlobalApplicationOptions.TMP_PATH + fileName + ".docx";
            System.IO.Directory.CreateDirectory(GlobalApplicationOptions.TMP_PATH);
            System.IO.FileStream fileStream = System.IO.File.Create(fullName);
            fileStream.Close();

            return fullName;
        }

        public static string SaveFile(Stream documentStream, string fileName)
        {
            string fullFilePath = GlobalApplicationOptions.TMP_PATH + fileName;
            FileStream fs = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write);
            documentStream.CopyTo(fs);
            fs.Flush();
            fs.Close();

            return fullFilePath;
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
        /// <summary>
        /// Creates the memory stream.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fullFileLocation">The full file location.</param>
        /// <returns>MemoryStream.</returns>
        public static MemoryStream createMemoryStream(dynamic Doc, string fileName, string fullFileLocation)
        {
            string file = "";
            if (uploadIDExists_ThreadSafe(Doc))
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

        /// <summary>
        /// Tears down.
        /// </summary>
        public static void TearDown(){
            removeLocalTmpFolder();
        }

        /// <summary>
        /// Removes the local TMP folder.
        /// </summary>
        private static void removeLocalTmpFolder()
        {
            if (Directory.Exists(GlobalApplicationOptions.TMP_PATH))
            {
                try
                {
                    Directory.Delete(GlobalApplicationOptions.TMP_PATH, true);
                }
                catch { }
            }
        }

        /// <summary>
        /// Sets the doc prop value_ thread safe.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="propertyValue">The property value.</param>
        public static void SetDocPropValue_ThreadSafe(dynamic Doc, string propertyName, string propertyValue)
        {
            ThreadTasks.ActionProtectOfficeObjectModel(() => FileIO.SetDocPropValue(Doc, propertyName, propertyValue));
        }

        /// <summary>
        /// Sets the doc prop value.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="propertyValue">The property value.</param>
        public static void SetDocPropValue(dynamic Doc, string propertyName, string propertyValue)
        {
            object CustomProps = Doc.CustomDocumentProperties;
            Type typeDocCustomProps = CustomProps.GetType();

            try
            {
                typeDocCustomProps.InvokeMember("Item",
                              BindingFlags.Default |
                              BindingFlags.SetProperty,
                              null, CustomProps,
                              new object[] { propertyName, propertyValue });

            }
            catch
            {
                addDocProp(Doc, propertyName, propertyValue);
            }
        }

        /// <summary>
        /// Adds the doc prop.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="propertyValue">The property value.</param>
        private static void addDocProp(dynamic Doc, string propertyName, string propertyValue)
        {
            object CustomProps = Doc.CustomDocumentProperties;
            Type typeDocCustomProps = CustomProps.GetType();

            object[] oArgs = {propertyName,false,
                     Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString,
                     propertyValue};

            typeDocCustomProps.InvokeMember("Add", BindingFlags.Default |
                                       BindingFlags.InvokeMethod, null,
                                       CustomProps, oArgs);
        }

        /// <summary>
        /// Gets the doc prop value_ thread safe.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <returns>String.</returns>
        public static String GetDocPropValue_ThreadSafe(dynamic Doc, string docPropName)
        {
            return (string)ThreadTasks.FunctionProtectOfficeObjectModel(() => FileIO.GetDocPropValue(Doc, docPropName));
        }

        /// <summary>
        /// Gets the doc prop value.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <returns>String.</returns>
        public static String GetDocPropValue(dynamic Doc, string docPropName)
        {
            object CustomProps = Doc.CustomDocumentProperties;
            Type typeDocCustomProps = CustomProps.GetType();

            try
            {
                object property = typeDocCustomProps.InvokeMember("Item",
                                           BindingFlags.Default |
                                           BindingFlags.GetProperty,
                                           null, CustomProps,
                                           new object[] { docPropName });
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

        /// <summary>
        /// Uploads the ID exists_ thread safe.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool uploadIDExists_ThreadSafe(dynamic Doc)
        {
            return (bool)ThreadTasks.FunctionProtectOfficeObjectModel(() => FileIO.uploadIDExists(Doc));
        }

        /// <summary>
        /// Uploads the ID exists.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <returns><c>true</c> if upload id exists, <c>false</c> otherwise</returns>
        public static bool uploadIDExists(dynamic Doc)
        {
            if (GetDocPropValue(Doc, GlobalApplicationOptions.GOOGLE_FILE_ID_PROPERTY_NAME) == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
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
            catch (NullReferenceException)
            {
                return "text/plain";
            }
        }

    }
}
