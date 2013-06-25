// ***********************************************************************
// Assembly         : SFSO
// Author           : CTDragon
// Created          : 05-18-2013
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
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

using InternalLibrary.Controller.EventHandlers;

using InternalLibrary.Data;
using InternalLibrary.Controller;
using InternalLibrary.IO;


namespace SFSO
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
        public Word.WdWordDialog SaveAsDialog = Word.WdWordDialog.wdDialogFileSaveAs;

        /// <summary>
        /// Handles the Startup event of the ThisAddIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.checkForUpdates();

            this.handlers = new Handlers(this.SaveAsDialog);
            this.Application.DocumentBeforeSave += new Word.ApplicationEvents4_DocumentBeforeSaveEventHandler(handlers.Application_DocumentBeforeSave);
            this.Application.DocumentBeforeClose += handlers.Application_DocumentBeforeClose;
            this.Application.DocumentChange += Application_DocumentNew;

            handlers.AddIn_Startup(Globals.ThisAddIn.Application.ActiveDocument, Globals.ThisAddIn.Application.ActiveDocument.CustomDocumentProperties);
        }

        /// <summary>
        /// Application_s the document change.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        public void Application_DocumentChange()
        {
            try
            {
                this.handlers.Application_DocumentChange(this.Application.ActiveDocument);
            }
            catch (System.Runtime.InteropServices.COMException ce)
            {

            }
        }

        /// <summary>
        /// Application_s the document new.
        /// </summary>
        /// <param name="Wb">The wb.</param>
        private void Application_DocumentNew()
        {
            this.checkForUpdates();
            this.Application.DocumentChange -= this.Application_DocumentNew;
            this.Application.DocumentChange += this.Application_DocumentChange;
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
