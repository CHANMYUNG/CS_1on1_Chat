using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace ConsoleApp1
{
    class Server
    {
        
        static void Main(string[] args)
        {
            byte[] sentMessage = new byte[1024];
           
            
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Bind(ipep);

                server.Listen(5);
                
                Socket client=  server.Accept();

                while (true)
                {
                    int byteRec = client.Receive(sentMessage);
                    Console.WriteLine(Encoding.ASCII.GetString(sentMessage, 0, byteRec));
                    byteRec = client.Receive(sentMessage);
                    Console.WriteLine(Encoding.ASCII.GetString(sentMessage, 0, byteRec));
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

        }
    }
}
