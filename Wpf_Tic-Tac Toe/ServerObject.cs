using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Wpf_Tic_Tac_Toe
{
    public class ServerObject
    {
        private int _PORT;
        public SocketManagement con;

        int currentClientID = 0;

        public int bytesLength = 1212;//data length


        public ServerObject(SocketManagement _CON)
        {
            // _IP = IPAddress.Parse(ip);//not needed
            _PORT = _CON._PORT;
            con = _CON;
        }
        static TcpListener tcpListener; // server listener
        List<ClientObject> clients = new List<ClientObject>(); // all connections

        //add a new connection
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        //remove a connection
        protected internal void RemoveConnection(string id)
        {
            // close the connection by id
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // remove the connection from the connection list
            if (client != null)
                clients.Remove(client);
        }
        // listening to incoming connections
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, _PORT);

                tcpListener.Start();
                Console.WriteLine("The server is running. Waiting for connections...");

                while (true)
                {

                    TcpClient tcpClient = tcpListener.AcceptTcpClient();//Is it transmitted to SocketManagment?
                    SocketManagement._CLIENT = tcpClient;//pass the tcpClient to the socket management
                    bool isMain = false;
                    if (currentClientID % 2 == 0)
                    {
                        isMain = true;
                    }
                        
                     ClientObject clientObject = new ClientObject(tcpClient, this, isMain, currentClientID);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                    currentClientID++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        protected internal void ISMain(AllData allData)
        {
        
        }

        protected internal void ItSelfMessage(AllData allData)//Sending message itself
        {
            
            //Clients are divided into pairs. First is major, second is subordinate.{(0,1),(2,3),...(2k,2k+1)}
            string destID = "";//destination ID
            int intDestID = 0;
            int.TryParse(allData.sourceID, out intDestID);
            
            //Sending message itself
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == intDestID.ToString()) // if client id equals destination client id 
                {
                    con.sendBoard(allData.obj, clients[i]); //send the data to the client with id==intDestID.ToString()
                }
            }
        }
        
        // broadcast message to connected clients
        protected internal void BroadcastMessage(AllData allData)
        {
            //Clients are divided into pairs. First is major, second is subordinate.{(0,1),(2,3),...(2k,2k+1)}
            string destID = "";//destination ID
            int intDestID = 0;
            int.TryParse(allData.sourceID, out intDestID);
            if (allData.IsSourceMain) {
                intDestID++; 
            }//client-source is chief, so id increase on +1
            else
            { intDestID--;}//id decrease 1

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == intDestID.ToString()) // if client id equals destination client id 
                {
                    con.sendBoard(allData.obj, clients[i]); //send the data to the client with id==intDestID.ToString()
                }
            }
        }

        // disconnect all customers
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //server shutdown

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //client disconnection
            }
            Environment.Exit(0); //process completion
        }
    }
}