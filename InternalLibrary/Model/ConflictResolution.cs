using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternalLibrary.Model.RequestManagement;

namespace InternalLibrary.Model
{
    public class ConflictResolution
    {
        public static void CheckForConflicts(string fileID, string previousRevisionID, string nextRevisionID)
        {
            RevisionRequestManager requestManager = new RevisionRequestManager();
            if (!string.IsNullOrEmpty(nextRevisionID) && !string.IsNullOrEmpty(previousRevisionID))
            {
                if (!requestManager.IsRevisionSequential(fileID, previousRevisionID, nextRevisionID))
                {
                    throw new InvalidOperationException("The head revision was overwritten without a proper merge.");
                }
            }
        }


    }
}
