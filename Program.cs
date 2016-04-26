using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace tcpclient
{
    class Program
    {
        static void Main(string[] args)
        {
            Int32 port = 3000;

            Console.WriteLine("Connecting to server 127.0.0.1:{0}", port.ToString());
            TcpClient client = connect(port);
            Console.WriteLine("Connected...\n");
            NetworkStream stream = client.GetStream();

            do
            {
                Console.Write("Enter message: ");

                String message = Console.ReadLine();
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                try
                {
                    // send data
                    stream.Write(data, 0, data.Length);

                    // store response
                    data = new Byte[256];
                    String responseData = "";

                    // read from socket
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    Console.WriteLine("Received: {0}", responseData);
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("ERROR: Remote socket closed!");
                    break;
                }
            }
            while (true);

            Console.Read();
        }

        private static TcpClient connect(Int32 port)
        {
            while (true)
            {
                try
                {
                    return new TcpClient("127.0.0.1", port);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Could not connect to host 127.0.0.1:{0}... Retyring...", port.ToString());
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
