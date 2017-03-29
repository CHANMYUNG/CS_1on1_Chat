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
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Windows.Threading;

namespace _1on1Client
{
    /// <summary>
    /// Client.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Client : UserControl
    {
        private Grid root;
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        Thread receivingThread = null;
        private bool shiftPressed = false;
        public Client(Grid root)
        {
            
            InitializeComponent();
            this.root = root;
            clientOn();
            sendButton.Click += new RoutedEventHandler(sendButton_Click);
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            sendMsg();
        }

        private void sendMsg()
        {
            string message = inputField.Text;
            if (message.Equals("")) return;
            message = "Client : " + message;
            textField.AppendText(message.Trim() + "\n");
            textField.ScrollToEnd();
            try
            {
                socket.Send(Encoding.Default.GetBytes(message));
            }catch(Exception e) { MessageBox.Show("Sending Error"); }
            inputField.Text = "";
        }

        private void clientOn()
        {
            try
            {
                socket.Connect(ipep);
            }catch(Exception e) { MessageBox.Show("Connection Error"); return; }
            textField.AppendText("System : Connecting Succeed...\n");

            receivingThread = new Thread(new ThreadStart(receiving));
            receivingThread.Start();
        }
        private void receiving(){
            byte[] bytes = null;
            while (true)
            {
                try
                {
                    bytes = new byte[1024];
                    socket.Receive(bytes, bytes.Length, SocketFlags.None);
                }
                catch (Exception e) {
                    socket.Close();
                    MessageBox.Show("Receiving Error");
                    return;
                }
                string message = Encoding.Default.GetString(bytes);
                message = message.TrimEnd('\0');
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                    textField.AppendText(message + "\n");
                    textField.ScrollToEnd();
                }));
                bytes = null;
            }
            

        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {

                shiftPressed = true;
            }
            if(e.Key == Key.Return)
            {
                if (shiftPressed == true)
                {
                    inputField.AppendText(Environment.NewLine);
                    inputField.SelectionStart = inputField.Text.Length;
                    
                }
                else
                    sendMsg();
               
            }
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                shiftPressed = false;
            }
        }
    }
}
