using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf_Tic_Tac_Toe
{
    class ChatClient
    {
        public SocketManagement con;
        //static string userName;
        private string host;

        private int port;

        static TcpClient client;
        static NetworkStream stream;
        public ChatClient(SocketManagement _CON)
        {
            con = _CON;
            host = con.IP;//the server ip adre
            port = con._PORT;//
        }


        public void ChatClientProcess()
        {
            //Console.Write("Enter your name: ");
            //userName = Console.ReadLine();
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //connect to the client
                stream = client.GetStream(); // getting stream from the client
                
                //string message = userName;
                //byte[] data = Encoding.Unicode.GetBytes(message);
                //stream.Write(data, 0, data.Length);

               
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
              
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
       
        static void SendMessage()
        {
        

            while (true)
            {
                //string message = Console.ReadLine();
                //byte[] data = Encoding.Unicode.GetBytes(message);
                //stream.Write(data, 0, data.Length);
               // con.sendBoard(,client)
            }
        }
        
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; 
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("Error!"); 
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0); 
        }
    }
}
