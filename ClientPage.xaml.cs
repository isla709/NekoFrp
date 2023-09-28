using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Net;

namespace NekoFrp
{
    /// <summary>
    /// ClientPage.xaml 的交互逻辑
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
        }
        
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string ServericeName =  GetNum().ToString() + " " + tb_Name.Text;
            string PCIP = tb_PCIP.Text;
            int      PCPORT = int.Parse(tb_PCPort.Text);
            string ServerIP = tb_ServerIP.Text;
            int      ServerPORT = int.Parse(tb_ServerPort.Text);

            Socket ReqSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ReqSoket.Connect(new IPEndPoint(IPAddress.Parse(ServerIP), ServerPORT));
            Thread ReqThread = new Thread((object obj) => {
                var ReqSok = (Socket)obj;
                ReqSok.Send(Encoding.UTF8.GetBytes(ServericeName));
                byte[] Recv_IP = new byte[128];
                ReqSok.Receive(Recv_IP);
                byte[] Recv_Port = new byte[128];
                ReqSok.Receive(Recv_Port);
                byte[] Recv_INPORT = new byte[128];
                ReqSok.Receive(Recv_INPORT);
                string ServerOUTIP = Encoding.UTF8.GetString(Recv_IP);
                string ServerOUTPORT = Encoding.UTF8.GetString(Recv_Port);
                string ServerINPORT = Encoding.UTF8.GetString(Recv_INPORT);
                Dispatcher.Invoke(() => {
                    ServericeList.Items.Add("[" + ServericeName + "]" + "本地:" + PCPORT + "=> 远程:" + ServerOUTIP + ":" + ServerOUTPORT);
                });
                Socket localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket roatSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                localSocket.Connect(new IPEndPoint(IPAddress.Parse(PCIP), PCPORT));
                roatSocket.Connect(new IPEndPoint(IPAddress.Parse(ServerIP), int.Parse(ServerINPORT)));
                Socket[] SocGP = { localSocket, roatSocket };
                Thread TXThread = new Thread(TX);
                TXThread.Start(SocGP);
                Thread RXThread = new Thread(RX);
                RXThread.Start(SocGP);
            });
            ReqThread.Start(ReqSoket);
        }
        
        void TX(object obj)
        {
            Socket[] SoketGP = (Socket[])obj;
            var local = SoketGP[0];
            var roat = SoketGP[1];
            
            while (true)
            {
                try
                {
                    byte[] buffTX = new byte[1];
                    local.Receive(buffTX);
                    roat.Send(buffTX);
                }
                catch { }
            }
        }
        void RX(object obj)
        {
            Socket[] SoketGP = (Socket[])obj;
            var local = SoketGP[0];
            var roat = SoketGP[1];
            while (true)
            {
                try
                {
                    byte[] buffRX = new byte[1];
                    roat.Receive(buffRX);
                    local.Send(buffRX);
                }
                catch { }
            }
        }
        int GetNum()
        {
            long A = DateTime.Now.Ticks;
            int B = DateTime.Now.Millisecond;
            int C = DateTime.Today.Year;
            long D = (A + B + C) << 5;
            long E = D * 125;
            int F = (int)E;
            return F;
        }
    }
}
