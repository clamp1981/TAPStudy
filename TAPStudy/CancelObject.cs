using System.Threading;
using System.Windows.Forms;

namespace TAPStudy
{
    public class CancelObject
    {
        public int Index { get; private set; }
        public Button CancelButton { get; private set;}
        public CancellationTokenSource CancleTokenSource { get; private set; }

        public CancelObject( int index, Button cancelBtn , CancellationTokenSource cancleTokenSource )
        {
            this.Index = index;
            this.CancelButton = cancelBtn;
            this.CancleTokenSource = cancleTokenSource;
        }

        public void Cancel()
        {
            this.CancleTokenSource.Cancel();
        }

    }
}
