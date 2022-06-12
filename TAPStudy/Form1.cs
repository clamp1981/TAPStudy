using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
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
                task.Start();
            }

            Task.WaitAll(tasks, 10000);
            Console.WriteLine("All task is done.");

            for (var i = 0; i < tasks.Length; i++)
            {
                this.listView1.Items[i].SubItems[3].Text = tasks[i].Result.ToString();

            }




        }
    }
}
