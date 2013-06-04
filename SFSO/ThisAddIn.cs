using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

using SFSO.Data;
using SFSO.Controller;
using SFSO.IO;
using System.Threading;


namespace SFSO
{
    public partial class ThisAddIn
    {
        bool allowSave = false;
        GlobalApplicationOptions userOptions = new GlobalApplicationOptions();
        RequestController requestController;
        List<Thread> threads = new List<Thread>();

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.checkForUpdates();
            this.Application.DocumentBeforeSave += new Word.ApplicationEvents4_DocumentBeforeSaveEventHandler(this.Application_DocumentBeforeSave);
            requestController = new RequestController(userOptions);

            threads.Add(new Thread(new ThreadStart(requestController.initializeUploadToGoogleDrive)));
            threads[threads.Count - 1].Start();

            //requestController.uploadToGoogleDrive(Globals.ThisAddIn.Application.ActiveDocument);

            //Thread oThread = new Thread(new ParameterizedThreadStart(requestController.uploadToGoogleDrive));
            //oThread.Start(Globals.ThisAddIn.Application.ActiveDocument);

            //string tmpFile = FileIO.createTmpFile(Globals.ThisAddIn.Application.ActiveDocument.Name, Globals.ThisAddIn.Application.ActiveDocument.FullName);
            
            //System.IO.Directory.CreateDirectory(GlobalApplicationOptions.TMP_PATH);
            //object fileName = "DriveProtectedDocument.docx";
            //Word._Document emptyDocument = Globals.ThisAddIn.Application.Documents.Add();
            //requestController.uploadToGoogleDrive(emptyDocument);
            //object addToRecentFiles = false;
            //Globals.ThisAddIn.Application.ActiveDocument.SaveAs2(ref fileName, ref missing, ref missing, ref missing, ref addToRecentFiles, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            //requestController.uploadToGoogleDrive(Globals.ThisAddIn.Application.ActiveDocument);
        }

        private void checkForUpdates()
        {
            DateTime expirationDate = new DateTime(2013, 6, 30);
            if (DateTime.Now.CompareTo(expirationDate) >= 0)
            {
                foreach (Office.COMAddIn addin in this.Application.COMAddIns)
                {
                    if (addin.Description.ToUpper().Equals("SFSO"))
                    {
                        System.Windows.Forms.MessageBox.Show("This beta version of SFSO has expired. Please upgrade to the newest release by visiting http://ctdragon.com. This add-in will now uninstall itself.");
                        addin.Connect = false;
                        //addin.Installed = false;
                        //addin.Delete();
                    }
                }
            }
        }

        //Modeled with code on http://social.msdn.microsoft.com/Forums/en-US/worddev/thread/33332b5b-992a-49a4-9ec2-17739b3a1259
        private void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            this.waitForRunningThreads();
            //Override Word's save functionality by writing own and sending cancel
            if (!this.allowSave)
            {
                this.allowSave = true;
                if (SaveAsUI)
                {
                    //Display Save As dialog
                    Word.Dialog saveAsDialog = this.Application.Dialogs[Word.WdWordDialog.wdDialogFileSaveAs];
                    object timeOut = 0;
                    saveAsDialog.Show(ref timeOut);
                }
                else
                {
                    //Simple save
                    Doc.Save();
                }

                //After file is saved
                threads.Add(new Thread(new ParameterizedThreadStart(requestController.uploadToGoogleDrive)));
                threads[threads.Count-1].Start(Doc);
                //this.requestController.uploadToGoogleDrive(Doc);
                this.allowSave = false;
                Cancel = true;
            }
        }

        private void waitForRunningThreads()
        {
            foreach (Thread thread in threads)
            {
                thread.Join(10000);
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            
            FileIO.TearDown();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
