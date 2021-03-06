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
/*
        这个类主要是为了提取各种置换算法
     
     */
namespace Os
{
    class Algorithm
    {
        public Panel MyPanel;
        public ArrayList list;
        public Label label;
        private int WithKuai = Request.TimeOfkuaibiao + Request.TimeOfneicun;
        private int WithKuai_queye = Request.TimeOfkuaibiao +Request.TimeOfneicun+ Request.TimeOfqueye + Request.TimeOfkuaibiao + Request.TimeOfneicun + Request.TimeOfneicun;
        private int WithoutKuai = Request.TimeOfneicun * 2;
        private int WithoutKuai_queye = Request.TimeOfneicun + Request.TimeOfqueye + Request.TimeOfneicun * 2;

        public void Algorithm_FIFO()
        {
            All hah = new All();
                //这个q是物理块的队列。
                Queue<int> q = new Queue<int>();
                 int time = 0;
                int wulikuai = Request.NumsOfwulikuai;
                int queyecishu = 0;
                for (int i = 0; i <= list.Count; i++)//列数
                {
                    //设置是否缺页的标志
                    bool queye = false;
                //这个b的值为当前将要置换的值，底下是为了初始化，没什么意义。
                    int b =0;
                    if (i != 0)
                    {
                        //只能这样转换，否则会产生错误。
                        String a = Convert.ToString(list[i - 1]);
                        b = Convert.ToInt32(a);
                        if (!q.Contains(b))
                        {
                            q.Enqueue(b);//加入队列
                            queyecishu++;
                            queye = true;//设置需要缺页中断的标志。
                        }
                    }
                    for (int j = 0; j < wulikuai + 3; j++)//行数
                    {
                        Label lab = new Label();        
                        if (i == 0)
                        {
                        lab = StyleLable(lab, 1, i, j);
                        if (j == 0)
                            {
                                lab.Text = "访问序列";
                            MyPanel.Invoke(new Action(() =>
                                {
                                    label.Text = "Hello";
                                    MyPanel.Controls.Add(lab);
                                }
                      ));
                            }
                            else if (j <= wulikuai)
                            {
                                lab.Text = "物理块" + j.ToString();
                                MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.Controls.Add(lab);
                                }
                      ));
                            }
                            else
                            {
                                if (j == wulikuai + 1)
                                {
                                    lab.Text = "是否缺页";
                                MyPanel.Invoke(new Action(() =>
                                    {
                                        MyPanel.Controls.Add(lab);
                                    }
                          ));
                                }
                                else
                                {
                                    lab.Text = "缺页率";
                                MyPanel.Invoke(new Action(() =>
                                    {
                                        MyPanel.Controls.Add(lab);
                                    }
                          ));
                                }

                            }
                        }
                        else
                        {
                             lab = StyleLable(lab, 2, i, j);
                            if (q.Count > Request.NumsOfwulikuai)
                            {
                                q.Dequeue();
                            }
                            int[] num = q.ToArray();

                            if (j == 0)
                            {
                                lab.Text = Convert.ToString(list[i - 1]);
                            }
                            else if (j <= wulikuai)
                            {
                               if (j <= q.Count)
                                {
                                    //这里应该设置label的颜色
                                    if (queye&&num[j-1]==b)
                                    {
                                        lab.ForeColor = Color.Red;
                                    }
                                 lab.Text = num[j - 1].ToString();
                                 }
                            }
                            else
                            {
                                if (j == wulikuai + 1)
                                {
                                    if (queye)
                                        lab.Text = "√";
                                }
                                else//缺页率的计算
                                {
                                    //在label中显式缺页率，同时传给Request的静态变量。
                                    double jieguo= (double)queyecishu / (double)(i) * 100;
                                    lab.Text = ((double)(double)queyecishu / (double)(i) * 100).ToString();
                                    Request.test_FIFO.Add(jieguo);
                                }
                            }
                            if (MyPanel.IsHandleCreated)
                            {
                            MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.AutoScrollPosition = new Point(0, 0);
                                    MyPanel.Controls.Add(lab);
                                }
                              ));
                            }

                        }
                    }
                MyPanel.Invoke(new Action(() =>
                {
                    time = FlashTime("FIFO算法时间 ", queye, time);
                }
                   ));
                Sleep(TimeSpan.FromSeconds(1));

                }
            
        }

          public void Algorithm_LRU()
        {
            int time = 0;
            LinkedList<int> q = new LinkedList<int>();
            int queyecishu = 0;
            int wulikuai = Request.NumsOfwulikuai;
            int b = 0;
                for (int i = 0; i <= list.Count; i++)
                {
                    bool queye = true;
                    if (i != 0)
                    {
                        String a = Convert.ToString(list[i - 1]);
                        b = Convert.ToInt32(a);

                        if (q.Contains(b))
                        {  
                            queye = false;
                            q.Remove(b);
                        }
                        q.AddFirst(b);
                    if (queye)//如果缺页，那么增加缺页次数
                        queyecishu++;
              
                    }
                    for (int j = 0; j < wulikuai + 3; j++)
                    {
                        Label lab = new Label();
                       
                        if (i == 0)
                        {
                        lab = StyleLable(lab, 1, i, j);
                            if (j == 0)
                            {
                                lab.Text = "访问序列";
                            MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.Controls.Add(lab);
                                }
                      ));
                            }
                            else if (j <= wulikuai)
                            {
                                lab.Text = "物理块" + j.ToString();
                            MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.Controls.Add(lab);
                                }
                      ));
                            }
                            else
                            {
                            if (j == wulikuai + 1)
                            {
                                lab.Text = "是否缺页";
                                MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.Controls.Add(lab);
                                }
                          ));
                            }
                            else
                            {
                                lab.Text = "缺页率";
                                MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.Controls.Add(lab);
                                }
                          ));
                            }
                        }
                        }
                        else
                        {
                        lab = StyleLable(lab, 2, i, j);
                        if (q.Count > wulikuai)
                            {
                                q.RemoveLast();
                            }
                            int[] num = q.ToArray();
                            if (j == 0)
                            {
                                lab.Text = Convert.ToString(list[i - 1]);
                            }
                            else if (j <= wulikuai)
                            {
                                if (j <= q.Count)
                                {
                                    if (queye&&b==num[j-1])
                                    {
                                        lab.ForeColor = Color.Red;
                                    }
                                    lab.Text = num[j - 1].ToString();
                                }

                            }
                            else
                            {
                                    if (j == wulikuai + 1)
                                 {
                                        if (queye)
                                           lab.Text = "√";
                                  }
                                 else//缺页率的计算
                                 {
                                double jieguo = (double)queyecishu / (double)(i) * 100;
                                lab.Text = ((double)(double)queyecishu / (double)(i) * 100).ToString();
                                Request.test_LRU.Add(jieguo);
                                }
                              }
                            if (MyPanel.IsHandleCreated)
                            {
                            MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.AutoScrollPosition = new Point(0, 0);
                                    MyPanel.Controls.Add(lab);
                                }
                              ));
                            }
                        }

                    }
                MyPanel.Invoke(new Action(() =>
                {
                    time = FlashTime("LRU算法时间 ", queye, time);

                }
             ));
                Sleep(TimeSpan.FromSeconds(1));
                }     
        }

          public void Algorithm_OPT()
        {
            LinkedList<int> q = new LinkedList<int>();
            int queyecishu = 0;
            int time = 0;
            int b = 0;
            int wulikuai = Request.NumsOfwulikuai;
            for (int i = 0; i <= list.Count; i++)
            {
                bool queye = true;
                if (i != 0)
                {
                    String a = Convert.ToString(list[i - 1]);
                    b = Convert.ToInt32(a);

                    if (q.Contains(b))
                    {
                        queye = false;
                    }
                    else//不包含，那么需要有页面出来
                    {
                        queyecishu++;
                        if (q.Count == wulikuai)
                        {
                            int[] nums = q.ToArray();
                            q.Remove(DeleteOpt(i - 1, nums));
                        }
                        q.AddFirst(b);
                    }

                }
                for (int j = 0; j < wulikuai + 3; j++)
                {
                    Label lab = new Label();

                    if (i == 0)
                    {
                        lab = StyleLable(lab, 1, i, j);
                        if (j == 0)
                        {
                            lab.Text = "访问序列";
                            MyPanel.Invoke(new Action(() =>
                            {
                                MyPanel.Controls.Add(lab);
                            }
                  ));
                        }
                        else if (j <= wulikuai)
                        {
                            lab.Text = "物理块" + j.ToString();
                            MyPanel.Invoke(new Action(() =>
                            {
                                MyPanel.Controls.Add(lab);
                            }
                  ));
                        }
                        else
                        {
                            if (j == wulikuai + 1)
                            {
                                lab.Text = "是否缺页";
                                MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.Controls.Add(lab);
                                }
                          ));
                            }
                            else
                            {
                                lab.Text = "缺页率";
                                MyPanel.Invoke(new Action(() =>
                                {
                                    MyPanel.Controls.Add(lab);
                                }
                          ));
                            }
                        }
                    }
                    else
                    {
                        lab = StyleLable(lab, 2, i, j);
                        int[] num = q.ToArray();
                        if (j == 0)
                        {
                            lab.Text = Convert.ToString(list[i - 1]);
                        }
                        else if (j <= wulikuai)
                        {
                            if (j <= q.Count)
                            {
                                if (queye&&b==num[j-1])
                                {
                                    lab.ForeColor = Color.Red;
                                }
                                lab.Text = num[j - 1].ToString();
                            }

                        }
                        else
                        {
                            if (j == wulikuai + 1)
                            {
                                if (queye)
                                    lab.Text = "√";
                            }
                            else//缺页率的计算
                            {
                                double jieguo = (double)queyecishu / (double)(i) * 100;
                                lab.Text = ((double)(double)queyecishu / (double)(i) * 100).ToString();
                                Request.test_OPT.Add(jieguo);
                            }
                        }
                        if (MyPanel.IsHandleCreated)
                        {
                            MyPanel.Invoke(new Action(() =>
                            {
                                MyPanel.AutoScrollPosition = new Point(0, 0);
                                MyPanel.Controls.Add(lab);
                            }
                          ));
                        }
                    }

                }
                MyPanel.Invoke(new Action(() =>
                {
                    time=FlashTime("OPT算法时间 ", queye, time);
                }
                 ));
                Sleep(TimeSpan.FromSeconds(1));
            }
        }

          private int FlashTime(String name,bool queye,int time)
        {
            
            if (Request.kuaibiao)
            {
                if (queye)
                {
                    time += WithKuai_queye;
           
                    label.Text =name+ WithKuai_queye.ToString() + "ns，总用时 "+time+"ns" ;
                   
                }
                else
                {
                    time += WithKuai; ;
                    label.Text = name+ WithKuai.ToString() + "ns，总用时 " + time + "ns";
                    
                }
            }
            else
            {
                if (queye)
                {
                    time += WithoutKuai_queye;
                    label.Text =name + WithoutKuai_queye.ToString() + "ns，总用时 " + time + "ns";
                   
                }
                else
                {
                    time += WithoutKuai;
                    label.Text = name + WithoutKuai.ToString() + "ns，总用时 " + time + "ns";
                   
                }

            }
            return time;
        }
          private int DeleteOpt(int index, int[] nums)
        {

            int[] target = new int[Request.NumsOfneicun];
            //或许这里可以优化性能
            for (int i = 0; i < list.Count; i++)
            {
                String a = Convert.ToString(list[i]);
                target[i] = Convert.ToInt32(a);
            }
            //坐标数与nums数是一一对应的，都是wulikuai、坐标数开始时都是0；
            int[] pinlv = new int[Request.NumsOfwulikuai];
            for (int i = 0; i < Request.NumsOfwulikuai; i++)
            {
                bool NoHave = true;//为了防止该页号再也不出现，因此设置如果其不在出现，那么就把他踢出去。
                for (int j = index; j < list.Count; j++)
                {
                    if (nums[i] == target[j])
                    {
                        pinlv[i] = j;
                        NoHave = false;
                        break;
                    }
                }
                if (NoHave)
                {
                    return nums[i];
                }

            }
            //找出距离当前最远页，返回页号。
            int max = pinlv[0];
            int fanhui = 0;
            for (int i = 0; i < Request.NumsOfwulikuai; i++)
            {
                if (pinlv[i] > max)
                {
                    max = pinlv[i];
                    fanhui = i;
                }
            }
            return nums[fanhui];
        }
          private Label StyleLable(Label lab,int index,int i,int j)
        {
            switch (index)
            {
                case 1:
                    lab.Width = 60;//如果不声明这个宽度有问题！宽度还需要微调。
                    lab.Left = 30;
                    lab.Top = j * 25+40;
                    break;
                case 2:
                    lab.Width =30;
                    lab.Left = i * 30 + 90;
                    lab.Top = j * 25+40;
                    break;
            }
          
            return lab;
        }
    }
}
