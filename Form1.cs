using DotNet.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EnglishWeeklyDaSearchTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("提取码不能为空");
                return;
            }
            // 发送请求
            HttpHelper httpKey = new HttpHelper();
            HttpItem itemKey = new HttpItem()
            {
                URL = "http://test.airlind.net/check.html",
                Method = "GET",
                Encoding = Encoding.UTF8,
                ResultType = ResultType.String
            };
            //得到HTML代码
            HttpResult resultKey = httpKey.GetHtml(itemKey);
            string htmlKey = resultKey.Html;
            if (resultKey.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("放弃吧，骚年，工具已废");
            }
            if (textBox1.Text.Trim() != htmlKey)
            {
                MessageBox.Show("提取码不正确");
                return;
            }
            label4.Visible = true;
            string whatNum = textBox2.Text.Trim();
            if (int.Parse(whatNum) < 10)
            {
                whatNum = "0" + whatNum;
            }
            if (int.Parse(whatNum) > 99)
            {
                MessageBox.Show("没有可以找到的答案了");
            }
            if (int.Parse(whatNum) < 1)
            {
                MessageBox.Show("什么鬼？！");
            }
            // 发送请求
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://app.ew.com.cn/Weekly/index.php?c=ResourceController&a=getResourceById&id=00538023"+ whatNum + "80&from=true",
                Method = "GET",
                Encoding = Encoding.UTF8,
                ResultType = ResultType.String
            };
            //得到HTML代码
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            Regex idRegex = new Regex("\"resource_path\":\"(.*?)\"");
            string idData = idRegex.Match(html).Groups[1].Value;
            // 请求2
            // 去除转义字符
            idData = idData.Replace("\\", "");
            // 发送请求
            HttpHelper http2 = new HttpHelper();
            HttpItem item2 = new HttpItem()
            {
                URL = idData+"/question.txt",
                Method = "GET",
                Encoding = Encoding.UTF8,
                ResultType = ResultType.String
            };
            HttpResult result2 = http.GetHtml(item2);
            string html2 = result2.Html;
            textBox3.Text = html2;
            label4.Visible = false;
        }
    }
}
