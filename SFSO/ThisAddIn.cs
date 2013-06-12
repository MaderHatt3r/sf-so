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
        private bool allowSave = false;
        private GlobalApplicationOptions userOptions = new GlobalApplicationOptions();
        private RequestController requestController;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.checkForUpdates();
            this.Application.DocumentBeforeSave += new Word.ApplicationEvents4_DocumentBeforeSaveEventHandler(this.Application_DocumentBeforeSave);
            this.Application.DocumentBeforeClose += Application_DocumentBeforeClose;
            this.Application.DocumentOpen += Application_DocumentOpen;
            requestController = new RequestController(userOptions);

            if (!FileIO.uploadIDExists())
            {
                ThreadTasks.RunThread(new ThreadStart(requestController.initializeUploadToGoogleDrive));
            }
        }

        private void Application_DocumentOpen(Word.Document Doc)
        {
            this.checkForUpdates();
            FileIO.TmpUploadExists = false;
            ThreadTasks.waitForRunningThreads();
            ThreadTasks.abortSuspendedThreads();
        }

        private void checkForUpdates()
        {
            DateTime expirationDate = new DateTime(2013, 7, 31);
            if (DateTime.Now.CompareTo(expirationDate) >= 0)
            {
                foreach (Office.COMAddIn addin in this.Application.COMAddIns)
                {
                    if (addin.Description.ToUpper().Equals("SFSO"))
                    {
                        System.Windows.Forms.MessageBox.Show("This beta version of SFSO has expired. Please upgrade to the newest release by visiting http://ctdragon.com. This add-in will now uninstall itself.");
                        addin.Connect = false;
                    }
                }
            }
        }

        //Modeled with code on http://social.msdn.microsoft.com/Forums/en-US/worddev/thread/33332b5b-992a-49a4-9ec2-17739b3a1259
        private void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            ThreadTasks.waitForRunningThreads();
            ThreadTasks.resumeSuspendedThreads();
            ThreadTasks.waitForRunningThreads();
            //Override Word's save functionality by writing own and sending cancel
            if (!this.allowSave)
            {
                this.allowSave = true;
                if (SaveAsUI)
                {
                    //Display Save As dialog
                    Word.Dialog saveAsDialog = this.Application.Dialogs[Word.WdWordDialog.wdDialogFileSaveAs];
                    object timeOut = 0;
                    //saveAsDialog.Show(ref timeOut);
                    if (saveAsDialog.Show(ref timeOut) != -1)
                    {
                        this.allowSave = false;
                        return;
                    }
                }
                else
                {
                    //Simple save
                    Doc.Save();
                }

                //After file is saved
                ThreadTasks.RunThread(new ParameterizedThreadStart(requestController.uploadToGoogleDrive), Doc);

                this.allowSave = false;
                Cancel = true;
            }
        }

        

        private void removeTmpUpload()
        {
            if (FileIO.TmpUploadExists)
            {
                requestController.removeTmpUpload();
            }
        }

        private void Application_DocumentBeforeClose(Word.Document Doc, ref bool Cancel)
        {
            this.Application.ActiveWindow.Visible = false;

            ThreadTasks.waitForRunningThreads();

            ThreadTasks.abortSuspendedThreads();

            //try
            //{
            //    removeTmpUpload();
            //}
            //catch
            //{

            //}
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
