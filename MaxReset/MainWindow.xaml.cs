using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace MaxReset
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

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        int ver { get { return comboBox1.SelectedIndex + 2009; } }
        private string[] getPaths()
        {
            string lang;
            int bit;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    lang = "ENU";
                    bit = 32;
                    break;
                case 1:
                    lang = "ENU";
                    bit = 64;
                    break;
                case 2:
                    lang = "CHS";
                    bit = 32;
                    break;
                default:
                    lang = "CHS";
                    bit = 64;
                    break;
            }
            List<string> pths = new List<string>();
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string pth_s = String.Format(@"{0}\Autodesk\3dsMax\{1} - {2}bit\{3}\", local, ver, bit, lang);
            string pth_d = String.Format(@"{0}\Autodesk\3dsMaxDesign\{1} - {2}bit\{3}\", local, ver, bit, lang);
            if (Directory.Exists(pth_s))
                pths.Add(pth_s);
            if (Directory.Exists(pth_d))
                pths.Add(pth_d);
            return pths.ToArray();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否要清除宏文件？", "MaxReset", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                foreach (string pth in getPaths())
                {
                    string dir = pth + (ver < 2013 ? "UI\\" : "") + "usermacros\\";
                    if (Directory.Exists(dir))
                        try
                        {
                            foreach (string f in Directory.GetFiles(dir, "*.*"))
                                File.Delete(f);
                            MessageBox.Show("处理完毕！", "MaxReset", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        catch { MessageBox.Show("处理失败，请先注销系统！", "MaxReset", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (Process.GetProcessesByName("3dsmax").Length > 0)
                if (MessageBox.Show("请先关闭正在运行的 3dsMax 再进行操作，是否继续？", "MaxReset", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;

            if (MessageBox.Show("是否要清除 Max 的配置？", "MaxReset", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                foreach (string pth in getPaths())
                    try
                    {
                        Directory.Delete(pth, true);
                        MessageBox.Show("处理完毕！", "MaxReset", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    catch { MessageBox.Show("处理失败，请先注销系统！", "MaxReset", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
