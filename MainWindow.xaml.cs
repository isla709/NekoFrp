using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Configuration;
using System.IO;

namespace NekoFrp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        StartPage startPage = new StartPage();
        public MainWindow()
        {
            InitializeComponent();
            CloseBGBrush1 = CloseBG.Fill;
            try
            {
                if (File.Exists("img/" + ConfigurationManager.AppSettings["Img"].ToString()))
                {
                    ImageBrush.ImageSource = new BitmapImage(new Uri("img/" + ConfigurationManager.AppSettings["Img"].ToString(), UriKind.RelativeOrAbsolute));
                }
                else
                {
                    try
                    {
                        ImageBrush.ImageSource = new BitmapImage(new Uri("img/[2266]天河運想-63835037.jpg", UriKind.RelativeOrAbsolute));
                    }
                    catch
                    {
                        
                    }
                }
            }
            catch
            {
                MessageBox.Show("背景初始化错误！图片丢失或损坏。\n详细信息: img目录下没有可用图片");
            }
            ViewShow.Content = new Frame() { Content = startPage };
            Thread TitleTimeThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Dispatcher.Invoke(() =>
                    {
                        TitleTime.Content = DateTime.Now.ToString();
                    });
                }
            })
            { 
                IsBackground = true,
            } ;
            TitleTimeThread.Start();
        }

        private void Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        Brush CloseBGBrush1;
        Brush CloseBGBrush2 = new SolidColorBrush(Color.FromArgb(40,255,0,0));
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseBG.Fill = CloseBGBrush2;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseBG.Fill = CloseBGBrush1;
        }
    }
}
