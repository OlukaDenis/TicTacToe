using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf_Tic_Tac_Toe
{
   public class SocketManagement
    {
        //  public IPAddress _IP;
        public string IP;
        public int _PORT;
        
        //part of the network protocol on the client:
       public static TcpClient _CLIENT;//TcpClient object in the client program!! (static)
        public static NetworkStream stream;//NetworkStream object in the client program!! (static)
          
        public static ServerObject server; // server object in the client program!! (static)
        public static Thread listenThread; // thread in the client program!! (static)


        public int bytesLength = 12;/*255;*///1212;


        public SocketManagement(String ip, int port,TcpClient client)
        {
            IP = ip;
            _PORT = port;
            _CLIENT = client;
        }


        public bool StartAsServer(SocketManagement con)
        {   
            
                server = new ServerObject(con/*this*/);//pass back Socket Managment
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //start the thread
            
            return true;
        }
        

        public bool StartAsClient()
        {
            
            try {                
                _CLIENT = new TcpClient();
                _CLIENT.Connect(IP, _PORT); 
                stream = _CLIENT.GetStream(); 
                
            }            
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
            //finally
            //{
            //    Disconnect();
            //}
            return true;
        }
        public bool amImain()
        {
            stream = _CLIENT.GetStream(); 
            byte[] bytes = new byte[bytesLength];

            stream.Read(bytes, 0, bytes.Length);
            string temp = new ASCIIEncoding().GetString(bytes);
            char[] charOfTemp = temp.ToCharArray();
            int[][] obj = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };
            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                    obj[y][x] = Int32.Parse("" + charOfTemp[(y * 3) + x]);
            if (obj[0][0] == 1/*1001*/) { return true; }//main
            return false;//not main
        }
        
        //send the game board over a network
        public bool sendBoard(int[][] obj, ClientObject client)
        {        
            
           try
            {
                string temp = "";
                for (int y = 0; y< 3; y++)
                    for (int x = 0; x< 3; x++)
                        temp += obj[y][x];

                // try to read data from client
                byte[] bytes = new byte[bytesLength];

                bytes = new ASCIIEncoding().GetBytes(temp);
                
                 client.Stream.Write(bytes, 0,bytes.Length);//send board
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.Message);
                return false; 
            }
                
            
            return true;

        }

        public AllData getBoard(ClientObject client)
        {

            //byte[] bytes = new byte[255];
            byte[] bytes = new byte[bytesLength];

            client.Stream.Read(bytes, 0, bytes.Length);
            string temp = new ASCIIEncoding().GetString(bytes);
            char[] charOfTemp = temp.ToCharArray();
            int[][] obj = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };
            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                    obj[y][x] = Int32.Parse("" + charOfTemp[(y * 3) + x]);



            return new AllData(obj, client.Id, client.isMain);
            //return obj;
        }
        //getBoard method for client app
        public /*int[][]*/AllData getBoard(TcpClient client)
        {          
            //byte[] bytes = new byte[255];
            byte[] bytes = new byte[bytesLength];

            stream.Read(bytes, 0, bytes.Length);
            string temp = new ASCIIEncoding().GetString(bytes);
            char[] charOfTemp = temp.ToCharArray();
            int[][] obj = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };
            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                    obj[y][x] = Int32.Parse("" + charOfTemp[(y * 3) + x]);

            int clientId = 1000;//it does not matter what value
            bool clientIsMain = false;//it does not matter what value

            return new AllData(obj, clientId.ToString(), clientIsMain);
            //return obj;
        }
        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//close the network stream
            if (_CLIENT != null)
                _CLIENT.Close();//close the tcp client
            Environment.Exit(0); //disconnect
        }

    }
}
