﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using System.Windows.Forms;

namespace Os
{
    public partial class Form1 : Form
    {  
        private int whichone = 0;
        private Thread Tfifo = null;
        private Thread Tlru = null;
        private Thread Topt = null;
        private ArrayList list = new ArrayList(); 
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = "页面访问序列：" + change();
            //this.WindowState = FormWindowState.Maximized;

        }

        //change意义是更新List和标签
        private string change()
        {
            string result = "";
            string a = Request.xulie;
            /*
            这个for循环是一个实验性质的，存在问题。比如默认了第一个字母为页面号；
            */
            int max = Request.NumsOfneicun;
            int i;
            for (i = 0; i <max-1; i++)
            {
                string ui;
                ui = a[i * 6].ToString();
                switch (ui)
                {
                    case "A":
                        ui = 10.ToString();
                        break;
                    case "B":
                        ui = 11.ToString();
                        break;
                    case "C":
                        ui = 12.ToString();
                        break;
                    case "D":
                        ui = 13.ToString();
                        break;
                    case "E":
                        ui = 14.ToString();
                        break;
                    case "F":
                        ui = 15.ToString();
                        break;
                }
                result += ui;
                list.Add(ui);
                result += ",";
            }
            result += a[i*6];
            list.Add(a[i * 6]);
            return result;
        }

        public void setlabel3()
        {
            label3.Text = "页面访问序列：" + restart();
            label3.Refresh();
        }
        //左上角的设置选项。
        public  void setlabel()
        {
            if (Request.kuaibiao)
                acce.Text = "快表已开启";
            else
                acce.Text = "快表已关闭";
            acce.Refresh();
            label3.Text = "页面访问序列：" + restart();
        }
        //每一个算法开始前，都应该进行一次restart操作。
        private string restart()
        {       
            list.Clear();
            return change(); 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //FIFO的代码
            whichone = 1;
            FIFOPanel.BringToFront();//将FIFO的Panel送到前台；
            //首先更新ArrayList
            restart();
           
            if (Tfifo==null||Tfifo.ThreadState.ToString()=="Unstart")
            {
                Algorithm algorithm = new Algorithm();
                algorithm.MyPanel = FIFOPanel;
                algorithm.label = label1;
                algorithm.list = list;
                // Tfifo = new Thread(FIFOMethod);
                Tfifo = new Thread(algorithm.Algorithm_FIFO);
                Tfifo.Start();
            }
            
        }
         //FIFO算法
        private void button2_Click(object sender, EventArgs e)
        {
            whichone = 2;
            LRUPanel.BringToFront();
            restart();
        
            if (Tlru==null||Tlru.ThreadState.ToString()== "Unstarted")
            {
                Algorithm algorithm = new Algorithm();
                algorithm.MyPanel = LRUPanel;
                algorithm.list = list;
                algorithm.label = label5;
                Tlru = new Thread(algorithm.Algorithm_LRU);
                Tlru.Start();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OPT_Click(object sender, EventArgs e)
        {
            whichone = 3;
            OPTPanel.BringToFront();
            restart();
            if (Topt==null||Topt.ThreadState.ToString() == "Unstarted")
            {
                Algorithm algorithm = new Algorithm();
                algorithm.MyPanel = OPTPanel;
                algorithm.list = list;
                algorithm.label = label4;
                Topt = new Thread(algorithm.Algorithm_OPT);
                Topt.Start();
            }
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            //如果程序执行完，那么点击会报错。（程序未进行，无法将其挂起）
            switch(whichone)
            {
                case 1:
                    if (Tfifo.ThreadState.ToString() == "Suspended")
                        Tfifo.Resume();
                    else
                        Tfifo.Suspend();
                    break;
                case 2:
                    if (Tlru.ThreadState.ToString()=="Suspended")
                        Tlru.Resume();
                    else
                        Tlru.Suspend();
                    break;
                case 3:
                    if (Topt.ThreadState.ToString()=="Suspended")
                        Topt.Resume();
                    else
                        Topt.Suspend();
                    break;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void LRUPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void AllBegin_Click_1(object sender, EventArgs e)
        {
            All all = new All();
           all. Show();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Setting set = new Setting();
            set.Show(this);
        }
    }
}
