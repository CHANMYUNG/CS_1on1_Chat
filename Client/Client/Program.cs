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
            catch (Exception e) { Console.WriteLine("Connecting failed"); }

            string s = "hey bro!";


            byte[] message = Encoding.ASCII.GetBytes(s);
            try
            {
                while (true)
                {
                    client.Send(message);
                    Thread.Sleep(500); // 안쓰면 오류
                    client.Send(Encoding.ASCII.GetBytes("!@!"));
                    Console.WriteLine("Sending Succeed");
                }

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
