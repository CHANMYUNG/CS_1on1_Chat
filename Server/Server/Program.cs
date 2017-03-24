using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace ConsoleApp1
{
    class Server
    {
        
        static void Main(string[] args)
        {
            byte[] sentMessage = new byte[1024];
            Socket client = null;

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Bind(ipep);

                server.Listen(5);

                client = server.Accept();

                Thread receivingThread = new Thread(() => SocketReceive(client));
                receivingThread.Start();

                Thread sendingThread = new Thread(() => SocketSend(client));
                sendingThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public static void SocketReceive(Socket client)
        {
            byte[] Buffer = new byte[1024];
            while (true) {
                int byteRec = client.Receive(Buffer);
                Console.WriteLine(Encoding.ASCII.GetString(Buffer, 0, byteRec));
                Thread.Sleep(500);
            }
        }
        public static void SocketSend(Socket client)
        {
            string s = "Hello Client";
            byte[] message = new byte[1024];
            message = Encoding.ASCII.GetBytes(s);
            while (true)
            {
                client.Send(message);
                Thread.Sleep(500);
            }
        }
    }
}
