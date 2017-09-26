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

namespace basebootwpf
{
    /// <summary>
    /// StartUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartUpWindow : Window
    {
        private String projectname;

        public StartUpWindow(String projectname)
        {
            this.projectname = projectname;
            InitializeComponent();
        }

        private void StartUp_Click(object sender, EventArgs e)
        {


            Process p = new Process();  // 初始化新的进程
            p.StartInfo.FileName = "CMD.EXE"; //创建CMD.EXE 进程
            p.StartInfo.RedirectStandardInput = true; //重定向输入
            p.StartInfo.RedirectStandardOutput = true;//重定向输出
            p.StartInfo.UseShellExecute = false; // 不调用系统的Shell
            p.StartInfo.RedirectStandardError = true; // 重定向Error
            p.StartInfo.CreateNoWindow = true; //不创建窗口
            p.Start(); // 启动进程
            p.StandardInput.WriteLine("cd " + projectname); // Cmd 命令

            p.StandardInput.WriteLine("startup.bat");

            string s = p.StandardOutput.ReadToEnd(); //将输出赋值给 S

        }
    }
}
