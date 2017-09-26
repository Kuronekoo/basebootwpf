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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, EventArgs e)
        {

            String groupId = this.groupId.Text;
            String artifactId = this.artifactId.Text;
            String version = this.version.Text;
            String package = this.package.Text;
            String projectname = this.projectname.Text;
            String corp = this.corp.Text;

            Process p = new Process();  // 初始化新的进程
            p.StartInfo.FileName = "CMD.EXE"; //创建CMD.EXE 进程
            p.StartInfo.RedirectStandardInput = true; //重定向输入
            p.StartInfo.RedirectStandardOutput = true;//重定向输出
            p.StartInfo.UseShellExecute = false; // 不调用系统的Shell
            p.StartInfo.RedirectStandardError = true; // 重定向Error
            p.StartInfo.CreateNoWindow = true; //不创建窗口
            p.Start(); // 启动进程
            String cmd = "mvn archetype:generate -B -U -DarchetypeGroupId=com.shurrik -DarchetypeArtifactId=baseboot -DarchetypeRepository=local -DarchetypeVersion=1.0-SNAPSHOT -DgroupId=" + groupId + " -DartifactId=" + artifactId + " -Dversion=" + version + " -Dpackage=" + package + " -Dproject_name=" + projectname + " -Dcorp=" + corp + " -DinteractiveMode=false -X -DarchetypeCatalog=local";
            p.StandardInput.WriteLine(cmd); // Cmd 命令

            p.StandardInput.WriteLine("exit"); // 退出

            string s = p.StandardOutput.ReadToEnd(); //将输出赋值给 S

            this.richTextBox1.Document.Blocks.Clear();

            Paragraph para = new Paragraph();
            Run r = new Run(s);
            para.Inlines.Add(r);
            this.richTextBox1.Document.Blocks.Add(para);

            p.WaitForExit();  // 等待退出
            MessageBox.Show("生成完成");
            this.Hide();
            DbWindow dbWindow = new DbWindow(projectname);
            dbWindow.Show();

        }

    }
}
