using System;
using System.Collections.Generic;
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

namespace NekoFrp
{
    /// <summary>
    /// StartPage.xaml 的交互逻辑
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            Color1 = btn_Setting.Background;
            clientPage.back.Click += Back_Click;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is MainWindow) as MainWindow;
            mw.ViewShow.Content = new Frame() { Content = this };
        }

        Brush Color1;
        Brush Color2 = new SolidColorBrush(Color.FromArgb(250, 20, 20, 20));
        SettingPage settingPage = new SettingPage();
        private void btn_Setting_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mainwindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(Window => Window is MainWindow) as MainWindow;
                mainwindow.ViewShow.Content = new Frame() { Content = settingPage };
            }
        }

        private void btn_Setting_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_Setting.Background = Color2;
        }

        private void btn_Setting_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_Setting.Background = Color1;
        }
        ServerPage serverPage = new ServerPage();
        private void btn_ServerMode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mainw = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is MainWindow) as MainWindow;
                mainw.ViewShow.Content = new Frame() { Content = serverPage };
            }
        }

        private void btn_ServerMode_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_ServerMode.Background = Color2;
        }

        private void btn_ServerMode_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_ServerMode.Background = Color1;
        }
        ClientPage clientPage = new ClientPage();
        private void btn_Client_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mw = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is MainWindow) as MainWindow;
                mw.ViewShow.Content = new Frame() { Content = clientPage };
            }
        }

        private void btn_Client_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_Client.Background = Color2;
        }

        private void btn_Client_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_Client.Background = Color1;
        }
    }
}
