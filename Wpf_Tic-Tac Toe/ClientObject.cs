using System;
using System.Net.Sockets;
using System.Text;

namespace Wpf_Tic_Tac_Toe
{
    public class ClientObject
    {
        public string Id { get; private set; }
        public static int id;
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // server object
        public bool isMain;//true if this client is main gamer 
        int _currentClientID;


        public int bytesLength = 1212;//data length

        public ClientObject(TcpClient tcpClient, ServerObject serverObject, bool _isMain, int currentClientID)
        {
            Id = id.ToString();
            client = tcpClient;
            Stream = client.GetStream();
            server = serverObject;
            try { serverObject.AddConnection(this); }
            catch (Exception) { }
            isMain = _isMain;
            _currentClientID = currentClientID;


            //notify the client or it is the main
            int[][] is_Main = new int[3][];
            is_Main[0] = new int[3];
            is_Main[1] = new int[3];
            is_Main[2] = new int[3];
            if (isMain)
            {
                is_Main[0][0] = 1;/*1001*/;
            }
            else is_Main[0][0] = 0;/*1000*/;
           
            try { server.ItSelfMessage(new AllData(is_Main, Id.ToString(), isMain)); }
            catch (Exception)
            {
                //this method does thing if this is on server (not client)
            };

            id++;

        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();

                //// get username
                //// send a message about logging into the chat to all connected users
               
                AllData message;


                //in an infinite loop which receives messages from the client
                while (true)
                {
                    try
                    {
                        message = server.con.getBoard(this);
                        server.BroadcastMessage(message);

                    }
                    catch
                    {
                        //--user has left the game--//
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // remove the connection
                server.RemoveConnection(this.Id);
                Close();
            }
        }


        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}