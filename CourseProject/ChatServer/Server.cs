using System.Text;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using CSfunction;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ChatServer
{
    class Server
    {
        private const int ServerPort = 8001;
        private Serializer serializer;
        private IPAddress ServerIP;
        private Socket UdpSocket;
        private Socket TcpSocket;
        private Thread listenUdpThread;
        private Thread listenTcpThread;
        private List<Client> clients;
        private SqlConnection connection;
        private const string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\УНИВЕР\КСиС\CourseProject\ChatServer\Database1.mdf;Integrated Security=True";
        private const int NOTFOUND = -1;
        private const bool SET = true;
        public Server()
        {
            UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serializer = new Serializer();
            ServerIP = GetIP();
            IPEndPoint localipUdp = new IPEndPoint(IPAddress.Any, ServerPort);
            IPEndPoint localipTcp = new IPEndPoint(ServerIP, ServerPort);
            clients = new List<Client>();
            UdpSocket.Bind(localipUdp);
            TcpSocket.Bind(localipTcp);
            listenUdpThread = new Thread(UDPlistener);
            listenTcpThread = new Thread(TCPListener);
            listenUdpThread.Start();
            listenTcpThread.Start();
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        //Прослушивание
        public void UDPlistener()
        {
            int bytes = 0;
            byte[] data = new byte[1024];
            EndPoint remoteip = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    do
                    {
                        bytes = UdpSocket.ReceiveFrom(data, ref remoteip);
                        memoryStream.Write(data, 0, bytes);
                    }
                    while (UdpSocket.Available > 0);
                    if (memoryStream.Length > 0)
                    {
                        MessageManager(serializer.Deserialize(memoryStream.ToArray()));
                    }
                }
            }
        }

        public void TCPListener()
        {
            int bytes = 0;
            TcpSocket.Listen(10);
            while (true)
            {
                Socket connectedSocket = TcpSocket.Accept();
                byte[] data = new byte[1024];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    do
                    {
                        bytes = connectedSocket.Receive(data, data.Length, SocketFlags.None);
                        memoryStream.Write(data, 0, bytes);
                    }
                    while (TcpSocket.Available > 0);
                    if (memoryStream.Length > 0)
                    {
                        NewClientAppear(serializer.Deserialize(memoryStream.ToArray()), connectedSocket);
                    }
                }
            }
        }

        //добавление новых клиентов
        public void NewClientAppear(Message message, Socket ConectedSocket)
        {
            if (message is TCPConnectMessage)
            {
                TCPConnectMessage RegMessage = (TCPConnectMessage)message;
                Client client;
                if (RegMessage.Id == 0)
                {
                    client = new Client(RegMessage.Name, GetID(), ConectedSocket, serializer, RegMessage.ClientEndPoint);
                    FillinDBClients(RegMessage.Name, client.id);
                }
                else
                {
                    if (CheckNameID(RegMessage.Name, RegMessage.Id))
                    {
                        client = new Client(RegMessage.Name, RegMessage.Id, ConectedSocket, serializer, RegMessage.ClientEndPoint);
                    }
                    else
                    {
                        GeneralFunction.CloseSocket(ref ConectedSocket);
                        return;
                    }
                }
                client.messageManager += MessageManager;
                client.ClientDisconnectedEvent += RemoveConnection;
                clients.Add(client);
                Console.WriteLine(RegMessage.Name + " join server");
                SendMessageClient( new ClientIDMessage(RegMessage.SenderAddress,client.id), client);
                SendMessageToAll(new FileListMessage(ServerIP,GetFileSet()));
            }
        }

        //сортировка сообщений
        public void MessageManager(Message message)
        {
            if (message is ServerRequest)
                SerwerAnswerRequest((ServerRequest)message);
            if (message is FileTaskMessage)
                FileTaskMessageManager((FileTaskMessage)message);
            if (message is FileDownloadRequest)
                FileDownloadRequestManager((FileDownloadRequest)message);
        }

        public void SerwerAnswerRequest(ServerRequest message)
        {
            UdpSocket.SendTo(serializer.Serialize(new ServerAnswerRequest(ServerIP, ServerPort, FindNameDB(message.ClientName))), new IPEndPoint(message.SenderAddress, message.ClientPort));
        }

        public void FileTaskMessageManager(FileTaskMessage message)
        {
            if (message.SetDelete == SET)
            {
                InsertintoDB(message);
            }
            else
            {
                DeletefromDB(message);
            }
            SendMessageToAll(new FileListMessage(ServerIP, GetFileSet()));
        }

        public void FileDownloadRequestManager(FileDownloadRequest message)
        {
            int HostID = FindHostID(message.FileName);
            Client HostClient = null;
            Client SenderClient = null;
            if (HostID != NOTFOUND)
                foreach (Client client in clients)
                {
                    if (client.id == HostID)
                        HostClient = client;
                    if (client.id == message.SenderID)
                        SenderClient = client;
                }
            if (SenderClient != null && HostClient != null)
                SendMessageClient(new ClientEndpoint(ServerIP, HostClient.ClientEndpoint, message.FileName), SenderClient);

        }

        //Пересылка сообщений
        public void SendMessageToAll(Message message)
        {
            foreach( Client client in clients)
            {
                SendMessageClient(message, client);
            }
        }

        public void SendMessageClient(Message message, Client client)
        {
            client.tcpSocket.Send(serializer.Serialize(message));
        }

        public IPAddress GetIP()
        {
            string HostName = Dns.GetHostName();
            return Dns.GetHostByName(HostName).AddressList[2];
        }
        //
        public void RemoveConnection(Client disconnectedClient)
        {
            if (clients.Remove(disconnectedClient))
                Console.WriteLine(disconnectedClient.name + " left from the server!");
            SendMessageToAll(new FileListMessage(ServerIP, GetFileSet()));
        }

        public int GetID()
        {
            return (ClientCounter()+1);
        }
        public int ClientCounter()
        {
            string sql = "SELECT * FROM Clients ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds.Tables[0].Rows.Count;
        }

        public void FillinDBClients(string ClientName, int id)
        {
            string sql = "INSERT INTO Clients (Id,Name) Values(" + id +",'"+ ClientName + "')";
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(sql, connection);
            adapter.InsertCommand.ExecuteNonQuery();

        }
        public void InsertintoDB(FileTaskMessage message)
        {
            string sql = "INSERT INTO Files (HostId,FileName) Values(" + message.HostID + ",'" + message.FileName + "')";
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(sql, connection);
            adapter.InsertCommand.ExecuteNonQuery();
        }

        public void DeletefromDB(FileTaskMessage message)
        {
            string sql = "DELETE FROM Files WHERE FileName = '" + message.FileName + "'";
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(sql, connection);
            adapter.InsertCommand.ExecuteNonQuery();
        }

        public List<FileSet> GetFileSet()
        {
            List<FileSet> files = new List<FileSet>();
            string sql = "SELECT * FROM Files";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            foreach(Client client in clients)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    if ((int)row.ItemArray[0] == client.id)
                    {
                        files.Add(new FileSet((string)row.ItemArray[1], (int)row.ItemArray[0]));
                    }
                }
            }
            return files;
        }
        public bool FindNameDB(string ClientName)
        {
            string sql = "SELECT * FROM Clients WHERE Name='"+ClientName+"'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
                return false;
            else
                return true;
        }

        public bool CheckNameID(string ClientName,int id)
        {
            string sql = "SELECT * FROM Clients WHERE Name='" + ClientName + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables[0].Rows[0].ItemArray[0].ToString() != id.ToString())
                return false;
            else
                return true;
        }

        public int FindHostID(string FileName)
        {
            string sql = "SELECT * FROM Files WHERE FileName='" + FileName + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
                return -1;
            else
                return (int)ds.Tables[0].Rows[0].ItemArray[0];
        }
        public void Close()
        {
            foreach (Client client in clients)
            {
                GeneralFunction.CloseSocket(ref client.tcpSocket);
                GeneralFunction.CloseThread(ref client.listenTcpThread);
            }
            GeneralFunction.CloseSocket(ref TcpSocket);
            GeneralFunction.CloseSocket(ref UdpSocket);
            GeneralFunction.CloseThread(ref listenTcpThread);
            GeneralFunction.CloseThread(ref listenUdpThread);
        }
        ~Server()
        {
            Close();
            connection.Close();
        }
    }
}
