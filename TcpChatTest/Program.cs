using System;
using System.Net.Sockets;
using System.Threading;

namespace TcpChatTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                AddClient();
            }
        }

        public static void AddClient()
        {
            Console.WriteLine("Add new client");
            string clientName = Console.ReadLine();
            var client = new TcpClient();
            client.Connect("127.0.0.1", 8888);

            var serverStream = client.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes($"{clientName}$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(() => getMessage(client));
            ctThread.Start();

            Thread.Sleep(1000);

            while (true) { }

        }

        public static void getMessage(TcpClient client)
        {
            while (true)
            {
                var serverStream = client.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[65536];
                buffSize = client.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                Console.WriteLine(returndata);
            }
        }
    }
}
