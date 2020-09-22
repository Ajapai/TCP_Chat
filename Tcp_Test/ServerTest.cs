using System;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tcp_Test
{
    [TestClass]
    public class ServerTest
    {
        private TcpClient client;
        [TestMethod]
        public void ConnectionTest()
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 8888);

            var serverStream = client.GetStream();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Jerome$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();

            Thread.Sleep(1000);

            WriteMessage();

            while (true) { }
        }

        private void WriteMessage()
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Hallo das ist ein Test$");
            var serverStream = client.GetStream();
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void getMessage()
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
