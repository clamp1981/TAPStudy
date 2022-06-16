using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAPStudy
{

    public partial class Form1 : Form
    {
        CalculateFactorial cal = new CalculateFactorial();
        List<Progress<ProgressEventArgs>> progresses = new List<Progress<ProgressEventArgs>>();

        public Form1()
        {
            InitializeComponent();

            for (int i = 10; i <= 100; i += 10)
            {
                AddListViewItem(i);
            }
            AddProgressBarInLisviewItem(1);
            AddButtonInLisviewItem(4);
        }



        private void AddListViewItem(int testNumber)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = testNumber.ToString();
            lvi.SubItems.Add("---");
            lvi.SubItems.Add("---");
            lvi.SubItems.Add("---");
            lvi.SubItems.Add("---");
            lvi.Tag = testNumber.ToString();
            this.listView1.Items.Add(lvi);
        }


        

        private void AddProgressBarInLisviewItem(int pbIndex)
        {
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                ProgressBar pb = new ProgressBar();
                pb.Minimum = 0;
                pb.Maximum = 100;
                pb.Value = 0;
                pb.Visible = true;
                pb.Parent = this.listView1;
                Rectangle rt = new Rectangle();
                rt = this.listView1.Items[i].Bounds;
                SetControlBounds(pb, rt, pbIndex);               
                this.listView1.Controls.Add(pb);
                Progress<ProgressEventArgs> pp = new Progress<ProgressEventArgs>();
                pp.ProgressChanged += Pp_ProgressChanged;
                this.progresses.Add(pp);
            }
        }


        private void AddButtonInLisviewItem(int btIndex)
        {
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                Button bt = new Button();
                bt.Text = "Cancel";
                bt.Click += CancelButton_Click;
                bt.Visible = true;
                bt.Parent = this.listView1;
                Rectangle rt = new Rectangle();
                rt = this.listView1.Items[i].Bounds;
                SetControlBounds(bt, rt, btIndex);
                this.listView1.Controls.Add(bt);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            
        }

        private void SetControlBounds( Control control , Rectangle rt , int addIndex )
        {
            
            int x = rt.X;
            for (int j = 0; j < addIndex; j++)
            {
                x += this.listView1.Columns[j].Width;
            }
            control.SetBounds(x, rt.Y, this.listView1.Columns[addIndex].Width, rt.Height);
        }

        private void Pp_ProgressChanged(object sender, ProgressEventArgs e)
        {
            //int index = this.progresses.FindIndex(x => x == sender);
            Console.WriteLine($"Pp_ProgressChanged : {e.index}, {e.percent }");
            if (e.percent < 100)
                this.listView1.Items[e.index].SubItems[2].Text = "calculating...";
            ProgressBar pb = this.listView1.Controls[e.index] as ProgressBar;
            pb.Value = e.percent;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tasks = new Task<BigInteger>[this.listView1.Items.Count];

            for (var i = 0; i < this.listView1.Items.Count; i++)
            {
                //루프문 안에서는 람다에 넘겨줄 값들을 캡쳐 해 놓아야 한다.
                //람다나 무명 메서드 사용 시 로컬 변수를 사용하면 클로저 처리 되는데, 로컬 변수가 참조값으로 
                //계속 해서 변경 되므로 내부 적으로 로컬 변수로 해당 값을 받아 처리 하여야 한다. 
                int target = Convert.ToInt32(this.listView1.Items[i].SubItems[0].Text);
                int index = i;
                tasks[index] = new Task<BigInteger>(() => { return cal.Calculate((int)target, index, progresses[index]); });
                tasks[index].ContinueWith(x =>
                {
                    Console.WriteLine($"ContinueWith : {target}, {x.Result}");
                    this.listView1.Items[index].SubItems[3].Text = x.Result.ToString();
                    this.listView1.Items[index].SubItems[2].Text = "Finish";
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }

            int itemIndex = 0;
            foreach (var task in tasks)
            {
                this.listView1.Items[itemIndex++].SubItems[2].Text = "Start";
                task.Start();
            }

            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tasks = new Task<BigInteger>[this.listView1.Items.Count];
            for (var i = 0; i < this.listView1.Items.Count; i++)
            {
                int target = Convert.ToInt32(this.listView1.Items[i].SubItems[0].Text);
                int index = i;
                tasks[i] = Task.Factory.StartNew(() => cal.Calculate((int)target, index, progresses[index]));

                this.listView1.Items[i].SubItems[2].Text = "Started";

                tasks[i].ContinueWith(x =>
               {
                   this.listView1.Items[index].SubItems[2].Text = "Finish";
                   this.listView1.Items[index].SubItems[3].Text = x.Result.ToString();
               }, TaskScheduler.FromCurrentSynchronizationContext());
            }

        }

        //비동기 함수 연속 연결
        private void button3_Click(object sender, EventArgs e)
        {

            for (var i = 0; i < this.listView1.Items.Count; i++)
            {
                int target = Convert.ToInt32(this.listView1.Items[i].SubItems[0].Text);
                //비동기 계산이 시작시에 UI 스레드의 Context를 가지고 있는 TaskSchedualer를  ContinueWith 두번째 인자로 넘김. 
                int index = i;
                var task = Task<BigInteger>.Factory.StartNew(() => cal.Calculate((int)target, index, progresses[index])).ContinueWith((x =>
               {
                   Console.WriteLine($"ContinueWith : {target}, {x.Result}");
                   this.listView1.Items[index].SubItems[2].Text = "Finish";
                   this.listView1.Items[index].SubItems[3].Text = x.Result.ToString();

               }), TaskScheduler.FromCurrentSynchronizationContext());

                this.listView1.Items[i].SubItems[2].Text = "Started";

            }

        }


        //async/await 사용하기 
        private async void button4_Click(object sender, EventArgs e)
        {
            var tasks = new Task<BigInteger>[this.listView1.Items.Count];
            for (var i = 0; i < this.listView1.Items.Count; i++)
            {
                this.listView1.Items[i].SubItems[2].Text = "Started";
                int target = Convert.ToInt32(this.listView1.Items[i].SubItems[0].Text);
                tasks[i] = GetCalculateFactorialAsync(i, target);
            }

            await Task.WhenAll(tasks);
        }

        private async Task<BigInteger> GetCalculateFactorialAsync(int itemIndex, int input)
        {

            var result = await Task.Run(() => { return cal.Calculate(input, itemIndex, progresses[itemIndex]); });
            this.listView1.Items[itemIndex].SubItems[3].Text = result.ToString();
            this.listView1.Items[itemIndex].SubItems[2].Text = "Finish";
            return result;

        }
    }

        


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
