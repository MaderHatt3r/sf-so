// ***********************************************************************
// Assembly         : SFSO-E
// Author           : CTDragon
// Created          : 06-13-2013
//
// Last Modified By : CTDragon
// Last Modified On : 06-16-2013
// ***********************************************************************
// <copyright file="ThisAddIn.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

using InternalLibrary.Data;
using InternalLibrary.Controller;
using InternalLibrary.IO;

namespace SFSO_E
{
    /// <summary>
    /// Class ThisAddIn
    /// </summary>
    public partial class ThisAddIn
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
        /// Handles the Startup event of the ThisAddIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.checkForUpdates();
            this.Application.WorkbookBeforeSave += new Excel.AppEvents_WorkbookBeforeSaveEventHandler(this.Application_DocumentBeforeSave);
            this.Application.WorkbookBeforeClose += Application_DocumentBeforeClose;
            //this.Application.DocumentOpen += Application_DocumentOpen;
            this.Application.WorkbookActivate += Application_DocumentNew;
            requestController = new RequestController(userOptions);

            if (!FileIO.uploadIDExists(Globals.ThisAddIn.Application.ActiveWorkbook.CustomDocumentProperties))
            {
                ThreadTasks.RunThread(new System.Threading.Tasks.Task(() => requestController.initializeUploadToGoogleDrive(Globals.ThisAddIn.Application.ActiveWorkbook)));
            }
        }

        /// <summary>
        /// Application_s the document new.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        private void Application_DocumentNew(Excel.Workbook Wb)
        {
            this.Application.WorkbookActivate += Application_DocumentChange;
        }

        /// <summary>
        /// Application_s the document change.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        private void Application_DocumentChange(Excel.Workbook Wb)
        {
            this.checkForUpdates();
            ThreadTasks.WaitForRunningTasks();
        }

        //private void Application_DocumentOpen(Word.Document Doc)
        //{
        //    this.checkForUpdates();
        //    ThreadTasks.WaitForRunningTasks();
        //}

        /// <summary>
        /// Checks for updates.
        /// </summary>
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
        /// <summary>
        /// Application_s the document before save.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="SaveAsUI">if set to <c>true</c> [save as UI].</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        private void Application_DocumentBeforeSave(Excel.Workbook Doc, bool SaveAsUI, ref bool Cancel)
        {
            ThreadTasks.WaitForRunningTasks();
            //Override Word's save functionality by writing own and sending cancel
            if (!this.allowSave)
            {
                this.allowSave = true;
                if (SaveAsUI)
                {
                    //Display Save As dialog
                    var saveAsDialog = this.Application.Dialogs[Excel.XlBuiltInDialog.xlDialogSaveAs];
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

        /// <summary>
        /// Application_s the document before close.
        /// </summary>
        /// <param name="Doc">The doc.</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        private void Application_DocumentBeforeClose(Excel.Workbook Doc, ref bool Cancel)
        {
            //this.Application.ActiveWindow.Visible = false;
            ThreadTasks.WaitForRunningTasks();
            requestController.removeTmpUpload();
        }

        /// <summary>
        /// Handles the Shutdown event of the ThisAddIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
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
