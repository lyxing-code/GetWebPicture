using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class 抓取图片 : Form
    {
        public 抓取图片()
        {
            InitializeComponent();
        }
        
      
        private void button1_Click(object sender, EventArgs e)
        {
           //匹配需要的下载链接
            Regex reg = new Regex("<img src=\"(?<Link>[\\S]+)\"", RegexOptions.Compiled);
            //模拟浏览器请求一个链接地址
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtUrl.Text);
            //请求类型
            request.Method = "Get";

            //模拟浏览器相应
            using (WebResponse wr = request.GetResponse())
            {
                //创建流 使用流读取浏览器反馈回来的数据流字符
                StreamReader sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                //创建一个变量接收流
                string html = sr.ReadToEnd();
                //关闭流
                sr.Close();
                
                //创建用来存储图片数据流的流对象
                Stream stream;

                //使用正则表达式找到已经匹配的图片链接地址,并且存入集合
                MatchCollection clcount = reg.Matches(html);
                int num = clcount.Count;
                for (int i = 0; i < clcount.Count; i++)
                {
                    //获取链接字符串
                    string countnum = clcount[i].Groups["Link"].Value;
                    //指定地址类型
                    request.ContentType = "jpeg/png";//
                    try
                    {
                        //再次模拟浏览器发送请求
                        request = (HttpWebRequest)WebRequest.Create(countnum);
                        //接收浏览器的响应
                        HttpWebResponse rsp = (HttpWebResponse)request.GetResponse();
                        //使用流来接收浏览器响应的数据流
                        stream = rsp.GetResponseStream();
                        stream.ReadTimeout = 15000;
                        stream.WriteTimeout = 15000;
                        //设置文件名称
                        string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                        //创建图片 来接收浏览器响应的数据流 并且存储到以下路径中
                        Image.FromStream(stream).Save(@"E:\项目\GetInfoapp\WindowsFormsApp1\WindowsFormsApp1\images\" + filename);

                        //----------------------//
                        //将项目中的图片添加到imageList1中
                        imageList1.Images.Add(Image.FromFile(Path.Combine(@"E:\项目\GetInfoapp\WindowsFormsApp1\WindowsFormsApp1\images\", filename)));
                        
                    }
                    catch (Exception)
                    {
                        num = num - 1;
                        continue;
                        //throw;
                    }

                }
                this.listView1.View = View.Tile;
                this.listView1.LargeImageList = imageList1;
                for (int i = 0; i < imageList1.Images.Count; i++)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = i.ToString();
                    li.ImageIndex = i;
                    listView1.Items.Add(li);
                }
                listView1.Visible = true;
                MessageBox.Show("已获取到" + num + "条信息");
            }


        }
        
        private void 抓取图片_Load(object sender, EventArgs e)
        {
            listView1.Visible = false;
        }


      
    }
}
