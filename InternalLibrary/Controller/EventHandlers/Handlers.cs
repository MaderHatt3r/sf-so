using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Office = Microsoft.Office.Core;

using InternalLibrary.IO;
using InternalLibrary.Controller;
using InternalLibrary.Data;

namespace InternalLibrary.Controller.EventHandlers
{
    public class Handlers
    {
        /// <summary>
        /// The allow save
        /// </summary>
        private bool allowSave = false;
        /// <summary>
        /// The user options
        /// </summary>
        private GlobalApplicationOptions userOptions = new GlobalApplicationOptions();
        /// <summary>
        /// The request controller
        /// </summary>
        private RequestController requestController;
        /// <summary>
        /// The save as dialog
        /// </summary>
        private dynamic SaveAsDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handlers"/> class.
        /// </summary>
        /// <param name="ThisAddIn">The this add in.</param>
        public Handlers(dynamic SaveAsDialog)
        {
            this.requestController = new RequestController(userOptions);
            this.SaveAsDialog = SaveAsDialog;
        }

        /// <summary>
        /// Handles the Startup event of the ThisAddIn control.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="customProp">The custom prop.</param>
        public void AddIn_Startup(dynamic doc, dynamic customProp)
        {
            this.requestController.SpawnInitializeUploadThread(doc, customProp);
        }

        /// <summary>
        /// Application_s the document before close.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public void Application_DocumentBeforeClose(dynamic Doc, ref bool Cancel)
        {
            //this.Application.ActiveWindow.Visible = false;
            ThreadTasks.WaitForRunningTasks();
            //requestController.removeTmpUpload();
        }

        /// <summary>
        /// Handles the Shutdown event of the ThisAddIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void AddIn_Shutdown()
        {
            requestController.removeTmpUpload();
            FileIO.TearDown();
            ThreadTasks.WaitForRunningTasks();
        }

        /// <summary>
        /// Application_s the document change.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        public void Application_DocumentChange(dynamic Doc)
        {
            ThreadTasks.WaitForRunningTasks();
            this.requestController.SpawnInitializeUploadThread(Doc, Doc.CustomDocumentProperties);
        }

        #region DocBeforeSave

        //Modeled with code on http://social.msdn.microsoft.com/Forums/en-US/worddev/thread/33332b5b-992a-49a4-9ec2-17739b3a1259
        /// <summary>
        /// Application_s the document before save.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="SaveAsUI">if set to <c>true</c> [save as UI].</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public void Application_DocumentBeforeSave(dynamic Doc, bool SaveAsUI, ref bool Cancel)
        {
            ThreadTasks.WaitForRunningTasks();
            this.Application_DocumentChange(Doc);

            //Override Word's save functionality by writing own and sending cancel
            if (!this.allowSave)
            {
                this.allowSave = true;
                if (SaveAsUI)
                {
                    //Display Save As dialog
                    var saveAsDialog = Doc.Application.Dialogs[this.SaveAsDialog];
                    //object timeOut = 0;
                    //saveAsDialog.Show(ref timeOut);
                    // If Cancel, exit
                    bool cancelled = this.cancelled(saveAsDialog.Show());
                    if (cancelled)
                    {
                        Cancel = true;
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
                ThreadTasks.RunThread(() => requestController.updateDriveFile(Doc));
                this.allowSave = false;
                Cancel = true;
            }
        }

        public void Application_DocumentBeforeSave(dynamic Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            this.Application_DocumentBeforeSave(Doc, SaveAsUI, ref Cancel);
        }

        // saveAsDialog.Show() returns bool type for Excel, short type for Word
        // the following is needed to handle the result returned when a user cancels a save
        private bool cancelled(dynamic dialogResult)
        {
            try
            {
                return !dialogResult;
            }
            catch
            {
                return dialogResult != -1;
            }
        }

        #endregion

        /// <summary>
        /// Checks for updates.
        /// </summary>
        public void CheckForUpdates(Office.COMAddIns COMAddIns)
        {
            DateTime expirationDate = new DateTime(2013, 7, 31);
            if (DateTime.Now.CompareTo(expirationDate) >= 0)
            {
                foreach (Office.COMAddIn addin in COMAddIns)
                {
                    if (addin.Description.ToUpper().Equals("SFSO"))
                    {
                        System.Windows.Forms.MessageBox.Show("This beta version of SFSO has expired. Please upgrade to the newest release by visiting http://ctdragon.com. This add-in will now uninstall itself.");
                        addin.Connect = false;
                    }
                }
            }
        }
    }
}
