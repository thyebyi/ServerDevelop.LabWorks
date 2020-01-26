using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpFileTransfer
{
    class Program
    {
        static string remoteAddress;
        static int remotePort;
        static int localPort;
        static string Filepath;
        static Client Client;

        static void Main(string[] args)
        {
            Client = new Client();
            try
            {
                Console.Write("Enter the ip adress to connect: ");
                remoteAddress = Console.ReadLine();
                Console.WriteLine("Choose an action \n 1. Send file \n 2. Recieve file \n");
                int choice = int.Parse(Console.ReadKey().ToString());
                if (choice == 1)
                {
                    Console.Write("Enter the port to connect: ");
                    remotePort = int.Parse(Console.ReadLine());
                    Console.Write("Enter file path: ");
                    Filepath = Console.ReadLine();
                    ConfirmAndSendFile();
                }
                else if (choice == 2)
                {
                    Console.Write("Enter the port to recieve: ");
                    localPort = int.Parse(Console.ReadLine());
                    Thread receiveThread = new Thread(new ThreadStart(StartReceiving));
                    receiveThread.Start();
                }
                else throw new ArgumentException();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
            Console.ReadKey();
        }

        static void StartReceiving()
        {
            Client.ReceiveFile(remoteAddress, localPort);
        }

        static void ConfirmAndSendFile()
        {
            Console.WriteLine($"Send file {Filepath}? y/n: ");

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.WriteLine();
                Client.SendFile(Filepath, remoteAddress, remotePort);
            }
        }
    }
}
