using System;

namespace Crowbar
{
    public class SourceModelProgressEventArgs : EventArgs
    {
        public SourceModelProgressEventArgs(AppEnums.ProgressOptions progress, string message) : base()
        {
            theProgress = progress;
            theMessage = message;
        }

        public AppEnums.ProgressOptions Progress
        {
            get
            {
                return theProgress;
            }
        }

        public string Message
        {
            get
            {
                return theMessage;
            }

            set
            {
                theMessage = value;
            }
        }

        private AppEnums.ProgressOptions theProgress;
        private string theMessage;
    }
}