using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Client
{
    class Client
    {
        
        static void Main(string[] args)
        {

            
            Socket client = null;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(ipep);
                Console.WriteLine(client.Connected);
            }
            catch (Exception e) { Console.WriteLine(e.Message); return; }

            string s = "hey bro!";


            byte[] message = Encoding.ASCII.GetBytes(s);

       

            Thread receivingThread = new Thread(() =>SocketReceive(client));
            receivingThread.Start();

            Thread sendingThread = new Thread(() => SocketSend(client));
            sendingThread.Start();
         }

        public static void SocketReceive(Socket client)
        {
            byte[] Buffer = new byte[1024];
            while (true)
            {
                int byteRec = client.Receive(Buffer);
                Console.WriteLine(Encoding.Default.GetString(Buffer, 0, byteRec));
                Thread.Sleep(500);
            }
        }
        public static void SocketSend(Socket client)
        {
            string s = "Hello Server";
            byte[] message = new byte[1024];
            message = Encoding.ASCII.GetBytes(s);
            while (true)
            {
                Console.WriteLine(s);
                client.Send(message);
                Thread.Sleep(500);
            }
        }
    }
}
