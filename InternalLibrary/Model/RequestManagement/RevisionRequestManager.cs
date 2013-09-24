// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 09-07-2013
//
// Last Modified By : CTDragon
// Last Modified On : 09-07-2013
// ***********************************************************************
// <copyright file="RevisionRequestManager.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using InternalLibrary.Data;

namespace InternalLibrary.Model.RequestManagement
{
    /// <summary>
    /// Class RevisionRequestManager.
    /// </summary>
    public class RevisionRequestManager
    {
        /// <summary>
        /// The service
        /// </summary>
        private DriveService service = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevisionRequestManager" /> class.
        /// </summary>
        public RevisionRequestManager(DriveService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Determines whether [is revision sequential] [the specified revision unique identifier].
        /// Tested by saving the file, getting a revision id, saving the file a couple more times,
        /// adding a break point to this method, replacing the previousID with the old one, then hitting continue
        /// </summary>
        /// <param name="fileID">The file unique identifier.</param>
        /// <param name="previousID">The previous unique identifier.</param>
        /// <param name="nextID">The next unique identifier.</param>
        /// <returns><c>true</c> if [is revision sequential] [the specified file unique identifier]; otherwise, <c>false</c>.</returns>
        public bool IsRevisionSequential(string fileID, string previousID, string nextID)
        {
            IList<Revision> revisions = RetrieveRevisions(fileID);
            int prevIndex = revisions.IndexOf(revisions.First(r => r.Id == previousID));
            int nextIndex = revisions.IndexOf(revisions.First(r => r.Id == nextID));

            return nextIndex == (prevIndex + 1);
        }

        /// <summary>
        /// Retrieve a list of revisions.
        /// </summary>
        /// <param name="fileId">ID of the file to retrieve revisions for.</param>
        /// <returns>List of revisions.</returns>
        public IList<Revision> RetrieveRevisions(String fileId)
        {
            try
            {
                RevisionList revisions = this.service.Revisions.List(fileId).Execute();
                return revisions.Items;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }


    }
}
