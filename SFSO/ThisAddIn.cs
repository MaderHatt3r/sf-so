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


namespace SFSO
{
    public partial class ThisAddIn
    {
        bool allowSave = false;
        GlobalApplicationOptions userOptions = new GlobalApplicationOptions();
        RequestController requestController;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.DocumentBeforeSave += new Word.ApplicationEvents4_DocumentBeforeSaveEventHandler(this.Application_DocumentBeforeSave);
            requestController = new RequestController(userOptions);
        }

        //Modeled with code on http://social.msdn.microsoft.com/Forums/en-US/worddev/thread/33332b5b-992a-49a4-9ec2-17739b3a1259
        private void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
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
                this.requestController.uploadToGoogleDrive(Doc);
                this.allowSave = false;
                Cancel = true;
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
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
