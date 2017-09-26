using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace basebootwpf
{
    /// <summary>
    /// DbWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DbWindow : Window
    {

        private String projectname;

        public DbWindow(String projectname)
        {
            this.projectname = projectname;
            InitializeComponent();
        }

        private void ImportSQL_Click(object sender, EventArgs e)
        {

            String path = this.path.Text;
            String host = this.host.Text;
            String username = this.username.Text;
            String password = this.password.Text;
            String port = this.port.Text;


            this.UpdatePom(projectname, host, username, password, port);

            Process p = new Process();  // 初始化新的进程
            p.StartInfo.FileName = "CMD.EXE"; //创建CMD.EXE 进程
            p.StartInfo.RedirectStandardInput = true; //重定向输入
            p.StartInfo.RedirectStandardOutput = true;//重定向输出
            p.StartInfo.UseShellExecute = false; // 不调用系统的Shell
            p.StartInfo.RedirectStandardError = true; // 重定向Error
            p.StartInfo.CreateNoWindow = true; //不创建窗口
            p.Start(); // 启动进程
            p.StandardInput.WriteLine("cd "+projectname); // Cmd 命令

            p.StandardInput.WriteLine("mysql -h "+host+" -P "+port+" -u" + username + " -p" + password + " -e \"create database "+projectname+";\"");
            p.StandardInput.WriteLine("mysql -h "+host+" -P "+port+" -u" + username + " -p" + password + " " + projectname + "<sys.sql");
            p.StandardInput.WriteLine("@echo init success!");



            p.StandardInput.WriteLine("exit"); // 退出

            string s = p.StandardOutput.ReadToEnd(); //将输出赋值给 S

            this.richTextBox1.Document.Blocks.Clear();

            Paragraph para = new Paragraph();
            Run r = new Run(s);
            para.Inlines.Add(r);
            this.richTextBox1.Document.Blocks.Add(para);

            p.WaitForExit();  // 等待退出
            MessageBox.Show("导入完成");
            this.Hide();
            StartUpWindow startUpWindow = new StartUpWindow(projectname);
            startUpWindow.Show();
                    
        }

        private void UpdatePom(String projectname,String host,String username,String password,String port)
        {

            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load("pom.xml");
            xmlDoc.Load(projectname + "/" + projectname + "-web/pom.xml");

            //XmlNodeList nodeList = xmlDoc.SelectSingleNode("properties").ChildNodes;//获取bookstore节点的所有子节点
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("properties");
            XmlNode proNode = nodeList.Item(0);

            foreach (XmlNode xn in proNode.ChildNodes)//遍历所有子节点
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型
                if (xe.Name == "jdbc.url")
                {
                    xe.InnerText = "jdbc:mysql://" + host + ":" + port + "/" + projectname + "?characterEncoding=UTF-8";
                }
                if (xe.Name == "jdbc.username")
                {
                    xe.InnerText = username;
                }
                if (xe.Name == "jdbc.password")
                {
                    xe.InnerText = password;
                }
            }
            xmlDoc.Save(projectname + "/" + projectname + "-web/pom.xml");
        }
    }
}
