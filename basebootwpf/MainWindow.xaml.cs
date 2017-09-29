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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace basebootwpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            dbBtn.IsEnabled = false;
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            genBtn.IsEnabled = false;
            dbBtn.IsEnabled = false;
            String groupId = this.groupId.Text;
            String artifactId = this.artifactId.Text;
            String version = this.version.Text;
            String package = this.package.Text;
            String projectname = this.projectname.Text;
            String corp = this.corp.Text;

            Process p = new Process();  // 初始化新的进程

            p.StartInfo.FileName = "cmd.exe";      // 命令  

            p.StartInfo.RedirectStandardInput = true; //重定向输入
            p.StartInfo.RedirectStandardOutput = true;//重定向输出
            p.StartInfo.UseShellExecute = false; // 不调用系统的Shell
            p.StartInfo.RedirectStandardError = true; // 重定向Error
            p.StartInfo.CreateNoWindow = true; //不创建窗口

            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            p.EnableRaisingEvents = true;                      // 启用Exited事件  
            p.Exited += CmdProcess_Exited;   // 注册进程结束事件

            p.Start(); // 启动进程
            p.StandardInput.WriteLine("rd/s/q " + projectname); // Cmd 命令
            String cmd = "mvn archetype:generate -B -U -DarchetypeGroupId=com.shurrik -DarchetypeArtifactId=baseboot -DarchetypeRepository=local -DarchetypeVersion=1.0-SNAPSHOT -DgroupId=" + groupId + " -DartifactId=" + artifactId + " -Dversion=" + version + " -Dpackage=" + package + " -Dproject_name=" + projectname + " -Dcorp=" + corp + " -DinteractiveMode=false -X -DarchetypeCatalog=local";
            p.StandardInput.WriteLine(cmd); // Cmd 命令
            p.StandardInput.WriteLine("exit");

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();  

            //p.WaitForExit();  // 等待退出


        }


        private void Db_Click(object sender, EventArgs e)
        {
            this.Hide();
            DbWindow dbWindow = new DbWindow(this.projectname.Text);
            dbWindow.Show();
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Output:" + e.Data);
            Dispatcher.Invoke(() =>
            {
                this.richTextBox1.Document.Blocks.Clear();
                this.richTextBox1.AppendText(e.Data + "\r\n");
                this.richTextBox1.ScrollToEnd();
                this.richTextBox1.Focus();
            });
        }


        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Error:" + e.Data);
            Dispatcher.Invoke(() =>
            {
                this.richTextBox1.Document.Blocks.Clear();
                this.richTextBox1.AppendText(e.Data + "\r\n");
                this.richTextBox1.ScrollToEnd();
                this.richTextBox1.Focus();
            });
        }


        private void CmdProcess_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("CmdProcess_Exited");
            Dispatcher.Invoke(() =>
            {
                this.dbBtn.IsEnabled = true;
                this.genBtn.IsEnabled = true;
                this.richTextBox1.AppendText("生成完成");
                this.richTextBox1.ScrollToEnd();
                this.richTextBox1.Focus();
            });

            MessageBox.Show("生成完成");
        }  
    }
}
