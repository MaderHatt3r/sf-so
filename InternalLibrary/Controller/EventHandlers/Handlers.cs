// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 06-17-2013
//
// Last Modified By : CTDragon
// Last Modified On : 07-06-2013
// ***********************************************************************
// <copyright file="Handlers.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Office = Microsoft.Office.Core;

using InternalLibrary.IO;
using InternalLibrary.Data;
using InternalLibrary.Model.RequestManagement;
using InternalLibrary.Model;

namespace InternalLibrary.Controller.EventHandlers
{
    /// <summary>
    /// Class Handlers
    /// </summary>
    public class Handlers
    {
        /// <summary>
        /// The allow save
        /// </summary>
        private bool allowSave = false;
        /// <summary>
        /// The request controller
        /// </summary>
        private UploadRequestManager requestController;
        /// <summary>
        /// The save as dialog
        /// </summary>
        private dynamic SaveAsDialog;
        private bool excelIgnoreAfterSave = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handlers" /> class.
        /// </summary>
        /// <param name="SaveAsDialog">The save as dialog.</param>
        public Handlers(dynamic SaveAsDialog)
        {
            this.requestController = ServiceRequestManagement.UploadRequestManager;
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
            this.Application_DocumentOpen(doc);
        }

        public void Application_DocumentOpen(dynamic doc)
        {
            // Check to see if the document is in the middle of a save operation 
            // TODO: (could wait on a new thread for save to exit and check then)
            if (allowSave)
            {
                System.Windows.Forms.MessageBox.Show("The application was in the middle of a save when opening this document, and could not check if there is a new version available.");
            }

            // Only if the handler is not busy and if the file is not a new file (does not contain .docx in file name)
            //(Crashes the program if document is opened from double-click
            if (!GlobalApplicationOptions.HandlerBusy && doc.Name.Contains(".docx"))
            {
                GlobalApplicationOptions.HandlerBusy = true;

                Model.ConflictResolution conflictManager = new Model.ConflictResolution();
                conflictManager.CheckForNewSaves(doc);

                GlobalApplicationOptions.HandlerBusy = false;
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
            GlobalApplicationOptions.OfficeObjectModelProtectionTimeout = new TimeSpan(0, 0, 6);
            ThreadTasks.WaitForRunningTasks();
        }

        /// <summary>
        /// Handles the Shutdown event of the ThisAddIn control.
        /// </summary>
        public void AddIn_Shutdown()
        {
            ThreadTasks.WaitForRunningTasks();
            requestController.removeTmpUpload();
            FileIO.TearDown();
            ThreadTasks.WaitForRunningTasks();
        }

        /// <summary>
        /// Application_s the document change.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        public void Application_DocumentChange(dynamic Doc)
        {
            ThreadTasks.WaitForRunningTasks();
            this.requestController.SpawnInitializeUploadThread(Doc, Doc.CustomDocumentProperties);
            //ConflictResolution resolutionManager = new ConflictResolution();
            //resolutionManager.CheckForNewSaves(Doc);
        }

        #region DocBeforeSave

        //Modeled with code on http://social.msdn.microsoft.com/Forums/en-US/worddev/thread/33332b5b-992a-49a4-9ec2-17739b3a1259
        /// <summary>
        /// Application_s the document before save for Excel.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="SaveAsUI">if set to <c>true</c> [save as UI].</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public void Application_DocumentBeforeSave(dynamic Doc, bool SaveAsUI, ref bool Cancel)
        {
            GlobalApplicationOptions.HandlerBusy = true;

            DocumentBeforeSavePreCheck(Doc);

            Model.ConflictResolution_E conflictManager = new Model.ConflictResolution_E();
            if (!conflictManager.CheckForNewSaves(Doc))
            {
                Cancel = true;
            }
            //if (conflictManager.UserSelection != null && conflictManager.UserSelection == ConflictResolutionOptions.MERGE)
            //{

            //}
        }

        private void DocumentBeforeSavePreCheck(dynamic Doc)
        {
            ThreadTasks.WaitForRunningTasks();
            this.Application_DocumentChange(Doc);
        }

        /// <summary>
        /// Application_s the document after save.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="Success">if set to <c>true</c> [success].</param>
        public void Application_DocumentAfterSave(dynamic Doc, bool Success)
        {
            if (Success && !excelIgnoreAfterSave)
            {
                //this.documentAfterSave(Doc);

                // Enable sharing
                object missing = Type.Missing;
                if (!((Microsoft.Office.Interop.Excel.Workbook)Doc).MultiUserEditing)
                {
                    Doc.Application.DisplayAlerts = false;
                    this.excelIgnoreAfterSave = true;
                    ((Microsoft.Office.Interop.Excel.Workbook)Doc).SaveAs(Doc.FullName, AccessMode: Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared);
                    excelIgnoreAfterSave = false;
                    Doc.Application.DisplayAlerts = true;

                    // Save after enable
                    this.excelIgnoreAfterSave = true;
                    Doc.Save();
                    this.excelIgnoreAfterSave = false;
                }

                requestController.updateDriveFile(Doc);

                try
                {
                    // Save after upload to undirty document
                    this.excelIgnoreAfterSave = true;
                    Doc.Save();
                    this.excelIgnoreAfterSave = false;
                }
                catch
                {
                    // Can't save again after merge or it crashes
                    this.excelIgnoreAfterSave = false;
                }
            }
            GlobalApplicationOptions.HandlerBusy = false;
        }

        /// <summary>
        /// Documents the after save.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        private void documentAfterSave(dynamic Doc)
        {
            //After file is saved
            ThreadTasks.RunThread(() => requestController.updateDriveFile(Doc));
        }

        // Word's event handler
        /// <summary>
        /// Application_s the document before save for Word.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="SaveAsUI">if set to <c>true</c> [save as UI].</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        public void Application_DocumentBeforeSave(dynamic Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            GlobalApplicationOptions.HandlerBusy = true;
            //ThreadTasks.RunThread(() =>
            //{
            //    //System.Threading.Thread.Sleep(2000);
            //    InternalLibrary.Forms.ConflictingVersionDialog dialog = new InternalLibrary.Forms.ConflictingVersionDialog();
            //    dialog.ShowDialog();
            //    ConflictResolutionOptions result = dialog.UserSelection;
            //});
            

            //return;

            this.DocumentBeforeSavePreCheck(Doc);
            Model.ConflictResolution conflictManager = new Model.ConflictResolution();
            if (conflictManager.CheckForNewSaves(Doc))
            {

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
                    if (SaveAsUI)
                    {
                        // if(SaveAsUI) is needed in the event that the user chooses yes after
                        // starting a document, not saving it, closing, then selecting yes to 
                        // save the document
                        requestController.updateDriveFile(Doc);
                    }
                    else
                    {
                        this.documentAfterSave(Doc);
                        Doc.Save();
                    }
                    this.allowSave = false;
                    Cancel = true;
                }
            }

            GlobalApplicationOptions.HandlerBusy = false;
        }

        // saveAsDialog.Show() returns bool type for Excel, short type for Word
        // the following is needed to handle the result returned when a user cancels a save
        /// <summary>
        /// Cancelleds the specified dialog result.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        /// <returns><c>true</c> if the save was cancelled, <c>false</c> otherwise</returns>
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
        /// <param name="COMAddIns">The COM add ins.</param>
        public void CheckForUpdates(Office.COMAddIns COMAddIns)
        {
            DateTime expirationDate = new DateTime(2014, 10, 30);
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
