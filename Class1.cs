using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ethernet_Server
{
    public class TCPServer
    {
        private TcpListener _listener;

        public void Begin(string host, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(host), port);
            _listener.Start();
            Console.WriteLine($"Server started on {host}:{port}");

            try
            {
                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    // Handle each client in a new task
                    Task.Run(() => HandleClient(client));
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: " + ex.Message);
            }
            finally
            {
                _listener.Stop();
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[17];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    Console.WriteLine("Received:");
                    for (int i = 0; i < bytesRead; i++)
                    {
                        Console.Write("{0:X2} ", buffer[i]);
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        public static void Main()
        {
            TCPServer server = new TCPServer();
            server.Begin("127.0.0.2", 8080);
        }
    }
}
