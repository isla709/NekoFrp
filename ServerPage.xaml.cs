using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace NekoFrp
{
    /// <summary>
    /// ServerPage.xaml 的交互逻辑
    /// </summary>
    public partial class ServerPage : Page
    {
        public ServerPage()
        {
            InitializeComponent();
        }
        Socket LisSocket;
        Thread ListenThrad;
        int P_Port;
        int p_in_Port = 10000;
        private void btn_StartServer_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                if (btn_StartServer.Content.ToString() == "启动服务")
                {
                    LisSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint IPEP = new IPEndPoint(IPAddress.Parse(tb_ip.Text), int.Parse(tb_reqPort.Text));
                    P_Port = int.Parse(tb_StartPort.Text);
                    LisSocket.Bind(IPEP);
                    LisSocket.Listen(10);
                    ListenThrad = new Thread(() =>
                    {
                        while (true)
                        {
                            var reqSocket = LisSocket.Accept();
                            reqSocket.ReceiveTimeout = 300000;
                            reqSocket.SendTimeout = 300000;
                            Thread reqThread = new Thread(reqThreadFunc);
                            reqThread.Start(reqSocket);
                        }
                    })
                    {
                        IsBackground = true
                    };
                    ListenThrad.Start();
                    btn_StartServer.Content = "终止服务";
                }
                else
                {
                    var Pick = MessageBox.Show("是否终止服务?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (Pick == MessageBoxResult.Yes)
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
           // }
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        string ReqOKMsg = "OK123";
        string Server_in_Port;
        string ServerPort;
        string ServerIP;
        void reqThreadFunc(object obj)
        {
            try
            {
                Socket reqSocket = (Socket)obj;
                byte[] Recv_UserName = new byte[128];
                reqSocket.Receive(Recv_UserName);
                String RecvNameString = Encoding.UTF8.GetString(Recv_UserName);
                Dispatcher.Invoke(() =>
                {
                    ServerIP = tb_ip.Text;
                    ServerPort = P_Port++.ToString();
                });
                reqSocket.Send(Encoding.UTF8.GetBytes(ServerIP));
                Thread.Sleep(500);
                reqSocket.Send(Encoding.UTF8.GetBytes(ServerPort));
                Thread.Sleep(500);
                Server_in_Port = p_in_Port++.ToString();
                reqSocket.Send(Encoding.UTF8.GetBytes(Server_in_Port));

                var CilentIP = reqSocket.RemoteEndPoint;
                string ClientIP = CilentIP.ToString();
                Dispatcher.Invoke(() =>
                {
                    Serveice_Show.Items.Add("Name:" + RecvNameString + " " + "IP:" + ClientIP + " " + Server_in_Port);
                });
                Socket S_In_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket S_Out_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("ServerPort:" + ServerPort);
                Console.WriteLine("Server_in_Port:" + Server_in_Port);
                Dispatcher.Invoke(() =>
                {
                    S_In_Socket.Bind(new IPEndPoint(IPAddress.Parse(tb_ip.Text), int.Parse(Server_in_Port)));
                    S_Out_Socket.Bind(new IPEndPoint(IPAddress.Parse(tb_ip.Text), int.Parse(ServerPort)));
                });

                S_In_Socket.Listen(10);
                S_Out_Socket.Listen(10);

                var IN = S_In_Socket.Accept();
                Console.WriteLine("Start");
                var OUT = S_Out_Socket.Accept();
                Console.WriteLine("OK!!!!!!");
                Socket[] socketsGP = { IN, OUT };
                Thread S_In_Thread_TX = new Thread(S_In_Thread_TX_Func);
                S_In_Thread_TX.Start(socketsGP);
                Thread S_In_Thread_RX = new Thread(S_In_Thread_RX_Func);
                S_In_Thread_RX.Start(socketsGP);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void S_In_Thread_TX_Func(object obj)
        {
            try
            {
                Socket[] socketsGP = (Socket[])obj;
                var IN_SCK = socketsGP[0];
                var OUT_SCK = socketsGP[1];
                Console.WriteLine(IN_SCK.RemoteEndPoint);
                Console.WriteLine(OUT_SCK.RemoteEndPoint);
                
                while (true)
                {
                    byte[] buffTX = new byte[1];
                    IN_SCK.Receive(buffTX);
                    OUT_SCK.Send(buffTX);
                }
            }
            catch
            { 
            }
        }
        void S_In_Thread_RX_Func(object obj)
        {
            try
            {
                Socket[] socketsGP = (Socket[])obj;
                var IN_SCK = socketsGP[0];
                var OUT_SCK = socketsGP[1];
                while (true)
                {
                    byte[] buffRX = new byte[1];
                    OUT_SCK.Receive(buffRX);
                    IN_SCK.Send(buffRX);
                }
            }
            catch
            { 
                
            }
        }
    }
}
