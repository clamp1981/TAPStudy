using System;

namespace TAPStudy
{
    public class ProgressEventArgs : EventArgs
    {
        public int index;
        public int percent;

        public ProgressEventArgs( int index, int percent )
        {
            this.index = index;
            this.percent = percent;
        }
    }
}
