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
using System.IO;
using System.Configuration;
using System.Net;

namespace NekoFrp
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {
        DirectoryInfo directoryInfo = new DirectoryInfo("./img");
        FileInfo[] Info;

        public SettingPage()
        {
            InitializeComponent();
            Info =  directoryInfo.GetFiles();
            foreach (var set in Info)
            {
                BG_List.Items.Add(set.Name);
            }
            loIP.Content = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
        }
            private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            var MW = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is MainWindow) as MainWindow;
            StartPage startPage = new StartPage();
            MW.ViewShow.Content = new Frame() { Content = startPage };
        }
        Configuration CFG = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (BG_List.SelectedIndex is -1)
            {
                var MW = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is MainWindow) as MainWindow;
                StartPage startPage = new StartPage();
                MW.ViewShow.Content = new Frame() { Content = startPage };
            }
            else
            {
                var MW = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is MainWindow) as MainWindow;
                string imgName = BG_List.SelectedValue.ToString();
                MW.ImageBrush.ImageSource = new BitmapImage(new Uri("img/" + imgName, UriKind.RelativeOrAbsolute));
                CFG.AppSettings.Settings["Img"].Value = imgName;
                CFG.Save();
                StartPage startPage = new StartPage();
                MW.ViewShow.Content = new Frame() { Content = startPage };
            }
        }
    }
}
