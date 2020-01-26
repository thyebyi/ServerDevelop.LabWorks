using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpFileTransfer
{
    class Client : UdpClient
    {
        public void SendFile(string path, string ip, int port = 8000)
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                byte[] data = File.ReadAllBytes(path);

                UdpClient sender = new UdpClient();
                try
                {
                    sender.Send(data, (int)info.Length, ip, port);
                    Console.WriteLine("File sended.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    sender.Close();
                }
            }
            else throw new FileNotFoundException();
        }

        public void ReceiveFile(string ip, int port = 8000)
        {
            UdpClient receiver = new UdpClient(port);
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting...");
                    byte[] data = receiver.Receive(ref remoteIp);
                    Console.Write("Enter saved file name: ");
                    string fileName = Console.ReadLine();
                    if (fileName == "") throw new ArgumentException();

                    string decodedData = Encoding.Default.GetString(data);
                    var stream = new StreamWriter(fileName, false, Encoding.Default);
                    stream.Write(decodedData);
                    stream.Close();
                    Console.WriteLine("File recieved.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
