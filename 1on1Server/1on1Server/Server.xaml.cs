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
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Windows.Threading;

namespace _1on1Server
{
    /// <summary>
    /// Server.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    

    public partial class Server : UserControl
    {
        private Grid root;
        private Socket socket = null;
        private Socket workingSocket = null;
        private Thread receivingThread = null;
        private Thread sendingThread = null;
        private List<string> ipList = new List<string>();
        private IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
       
        public Server(Grid root)
        {
            InitializeComponent();
            this.root = root;
            ServerOn();
            sendButton.Click += new RoutedEventHandler(sendButton_Click);
        }
        private void ServerOn()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                
                socket.Bind(ipep);
                socket.Listen(5);
                
                receivingThread = new Thread(new ThreadStart(receiving));
                receivingThread.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Server haven't launched\n" + e.ToString());
            }
        }
        private void receiving()
        {
            byte[] bytes = new byte[1024];
            workingSocket = socket.Accept();

            while (true)
            {
                try
                {
                    workingSocket.Receive(bytes, bytes.Length, SocketFlags.None);
                }
                catch (NullReferenceException ne) { MessageBox.Show("NullReference"); }
                catch (SocketException se) {MessageBox.Show("Socket Error Occured"); }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                string message = Encoding.Default.GetString(bytes);
                message = message.TrimEnd('\0');

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    textField.AppendText(message + "\n");
                    textField.ScrollToEnd();
                }));
                
            }
        }
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            sendMsg();
        }

        private void sendMsg()
        {
            if (inputField.Text.Equals("")) return;
                textField.AppendText(inputField.Text.Trim() + "\n");
            textField.ScrollToEnd();
            try
            {
                workingSocket.Send(Encoding.Default.GetBytes(inputField.Text));
            }catch(Exception e)
            {
                MessageBox.Show("sending failed");
            }
            inputField.Text = "";
        }
    }
}
