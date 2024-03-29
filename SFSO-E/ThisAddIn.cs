﻿// ***********************************************************************
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

using InternalLibrary.Controller.EventHandlers;

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
        /// The handlers
        /// </summary>
        private Handlers handlers;
        /// <summary>
        /// The save as dialog
        /// </summary>
        public Excel.XlBuiltInDialog SaveAsDialog = Excel.XlBuiltInDialog.xlDialogSaveAs;

        /// <summary>
        /// Handles the Startup event of the ThisAddIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            try
            {
                FileIO.createTempDirectory();
                this.handlers = new Handlers(this.SaveAsDialog);
                this.handlers.CheckForUpdates(this.Application.COMAddIns);

                this.Application.WorkbookBeforeSave += new Excel.AppEvents_WorkbookBeforeSaveEventHandler(handlers.Application_DocumentBeforeSave);
                this.Application.WorkbookAfterSave += new Excel.AppEvents_WorkbookAfterSaveEventHandler(handlers.Application_DocumentAfterSave);
                this.Application.WorkbookBeforeClose += handlers.Application_DocumentBeforeClose;
                this.Application.WorkbookActivate += Application_DocumentNew;
                this.Application.WorkbookOpen += Application_WorkbookOpen;

                if (this.Application.ProtectedViewWindows.Count <= 0)
                {
                    handlers.AddIn_Startup(Globals.ThisAddIn.Application.ActiveWorkbook, Globals.ThisAddIn.Application.ActiveWorkbook.CustomDocumentProperties);
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("A problem occured during startup of SFSO Add-In. Please try opening the application, then openeing the document from the application if opening the application directly from the document (ex double-click) is giving you issues." +
                //    Environment.NewLine + Environment.NewLine + ex.Message);
            }

        }


        /// <summary>
        /// Handles the workbook open.
        /// This is needed because the program disposes of the current wb when a document is opened
        /// and the methods that make calls to FunctionProtectOfficeObjectModel and ActionProtect...
        /// never stop throwing errors, and the threads calling them never complete until the timeout 
        /// which is set for 1 hr.
        /// This handler for Open (don't know if before or after) seems to keep the wb document
        /// alive long enough for the work to complete.
        /// To reproduce the bug: 
        /// - Open then save a new document
        /// - Close Excel
        /// - Open excel, click File, then the name of your document
        /// --(recent should already be selected, if not then you are not reproducing the error)
        /// - Type anything, then click save. The program locks up.
        /// Hint: look at the Output window in VS to see a first time exception that is thrown,
        /// then find where that exception is being caught (FunctionProtectionOfficeObjectModel)
        /// </summary>
        /// <param name="Wb">The wb.</param>
        void Application_WorkbookOpen(Excel.Workbook Wb)
        {
            ThreadTasks.WaitForRunningTasks();
        }

        /// <summary>
        /// Application_s the document change.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        public void Application_DocumentChange(Excel.Workbook Wb)
        {
            try
            {
                this.handlers.Application_DocumentChange(Wb);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // The document is closed, so you cannot access the "Active" Document because there isn't one
            }
        }

        /// <summary>
        /// Application_s the document new.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        private void Application_DocumentNew(Excel.Workbook Wb)
        {
            this.handlers.CheckForUpdates(this.Application.COMAddIns);
            this.Application.WorkbookActivate -= this.Application_DocumentNew;
            this.Application.WorkbookActivate += this.Application_DocumentChange;
        }

        /// <summary>
        /// Handles the Shutdown event of the ThisAddIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            this.handlers.AddIn_Shutdown();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(this.ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(this.ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
