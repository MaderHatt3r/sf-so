using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private dynamic ThisAddIn;

        public Handlers(dynamic ThisAddIn)
        {
            this.requestController = new RequestController(userOptions);
            this.ThisAddIn = ThisAddIn;
        }

        public void InitializeUpload(dynamic document, dynamic customProps)
        {
            if (!FileIO.uploadIDExists(customProps))
            {
                ThreadTasks.RunThread(new System.Threading.Tasks.Task(() => requestController.initializeUploadToGoogleDrive(document)));
            }
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
            ThreadTasks.WaitForRunningTasks();
            FileIO.TearDown();
        }

        /// <summary>
        /// Application_s the document change.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        public void Application_DocumentChange(dynamic Doc)
        {
            ThreadTasks.WaitForRunningTasks();
            this.InitializeUpload(Doc, Doc.CustomDocumentProperties);
        }

        //Modeled with code on http://social.msdn.microsoft.com/Forums/en-US/worddev/thread/33332b5b-992a-49a4-9ec2-17739b3a1259
        /// <summary>
        /// Application_s the document before save.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="SaveAsUI">if set to <c>true</c> [save as UI].</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public void Application_DocumentBeforeSave(dynamic Doc, bool SaveAsUI, ref bool Cancel)
        {
            this.InitializeUpload(Doc, Doc.CustomDocumentProperties);
            ThreadTasks.WaitForRunningTasks();
            //Override Word's save functionality by writing own and sending cancel
            if (!this.allowSave)
            {
                this.allowSave = true;
                if (SaveAsUI)
                {
                    //Display Save As dialog
                    var saveAsDialog = Doc.Application.Dialogs[this.ThisAddIn.SaveAsDialog];
                    object timeOut = 0;
                    //saveAsDialog.Show(ref timeOut);
                    // If Cancel, exit
                    bool cancelled = !saveAsDialog.Show();
                    if (cancelled)
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
                ThreadTasks.RunThread(new System.Threading.Tasks.Task(() => requestController.uploadToGoogleDrive(Doc)));

                this.allowSave = false;
                Cancel = true;
            }
        }
    }
}
