// Base class for Source___FileData classes.
using System.Collections.Generic;

namespace Crowbar
{
    public class SourceFileData
    {

        #region Creation and Destruction

        public SourceFileData()
        {
            theFileSeekLog = new FileSeekLog();
            theUnknownValues = new List<UnknownValue>();
        }

        #endregion

        #region Data

        public FileSeekLog theFileSeekLog;
        public List<UnknownValue> theUnknownValues;

        #endregion

    }
}