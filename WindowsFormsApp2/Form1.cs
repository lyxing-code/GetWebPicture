using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
        }

        int num = 10;
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    timer1.Start();
        //    progressBar1.Visible = true;
        //    this.progressBar1.Value += 10;
        //    if (this.progressBar1.Value == 100)
        //    {
        //        MessageBox.Show("加载完成!");
        //        return;
        //    }
        //}

        private delegate void SetPos(int ipos);

        private void SetTextMessage(int ipos)
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(SetTextMessage);
                this.Invoke(setpos, new object[] { ipos });
            }
            else
            {
                this.label1.Text = ipos.ToString() + "%";
                this.progressBar1.Value = Convert.ToInt32(ipos);
            }
        }

        public Thread fThread;

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            fThread = new Thread(new ThreadStart(SleepT));//开辟一个新的线程
            fThread.Start();
            button1.Enabled = false;
        }

        private void SleepT()
        {
            for (int i = 0; i <= 500; i++)
            {
                System.Threading.Thread.Sleep(10); //线程休眠，控制进度条的速度。
                SetTextMessage(100 * i / 500);
            }

            if (progressBar1.Value == 100)
            {
                if (MessageBox.Show("已完成！", "提示", MessageBoxButtons.OK)
                     == DialogResult.OK)
                {
                    progressBar1.Value = 0;
                    this.label1.Text = "";
                    button1.Enabled = true;
                }
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
        }
    }
}
