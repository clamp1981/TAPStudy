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
        public Form1()
        {
            InitializeComponent();

            for( int i = 10; i <= 100; i += 10 )
            {
                AddListViewItem(i);
            }
        }


        private ListViewItem AddListViewItem(int testNumber)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = testNumber.ToString();
            lvi.SubItems.Add("Not Started");
            lvi.SubItems.Add("1");            
            lvi.SubItems.Add("---");
            this.listView1.Items.Add(lvi);

            return lvi;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            var tasks = new Task<BigInteger>[this.listView1.Items.Count];
            for (var i = 0; i < this.listView1.Items.Count; i++)
            {
                int target = Convert.ToInt32(this.listView1.Items[i].SubItems[0].Text);
                var task = new Task<BigInteger>(() => cal.Calculate((int)target));
                tasks[i] = task;

            }

            foreach (var task in tasks)
            {
                int index = task.Id - 1; 
                this.listView1.Items[index].SubItems[1].Text = "Started";
                task.Start();
            }

            //모든 비동기 작업이 완료 되기를 기다린다. 
            //Task.WaitAll(tasks, 10000);
            //Console.WriteLine("All task is done.");
            //for (var i = 0; i < tasks.Length; i++)
            //{
            //    this.listView1.Items[i].SubItems[3].Text = tasks[i].Result.ToString();

            //}

            //task중 하나라도 완료 하면 블럭이 해제
            var taksIndex = Task.WaitAny(tasks, 10000);
            Console.WriteLine($"완료 : {taksIndex}");
            for (var i = 0; i < tasks.Length; i++)
            {
                if(tasks[i].Status == TaskStatus.RanToCompletion)
                {
                    this.listView1.Items[i].SubItems[1].Text = "Finish";
                    this.listView1.Items[i].SubItems[3].Text = tasks[i].Result.ToString();
                }
                else
                    Console.WriteLine("   Task {0}: {1}", tasks[i].Id, tasks[i].Status);

            }

        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            var tasks = new Task<BigInteger>[this.listView1.Items.Count];
            for (var i = 0; i < this.listView1.Items.Count; i++)
            {
                int target = Convert.ToInt32(this.listView1.Items[i].SubItems[0].Text);
                tasks[i] = Task.Factory.StartNew(() =>
               {
                   return cal.Calculate((int)target);
               });

                this.listView1.Items[i].SubItems[1].Text = "Started";
            }

            Task.WaitAll(tasks, 10000);


            for (var i = 0; i < this.listView1.Items.Count; i++)
            {
                this.listView1.Items[i].SubItems[1].Text = "Finish";
                this.listView1.Items[i].SubItems[3].Text = tasks[i].Result.ToString();
            }



        }
    }
}
