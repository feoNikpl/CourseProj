using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using CSfunction;
using System.Collections.Generic;
using System;

namespace ChatClient
{
    public delegate void ReceiveMessage(Message message);
    class Client
    {
        public const int SIZE = 1024 * 1024;
        private static int localPort = 0;
        private const int ServerPort = 8001;
        private Serializer serializer;
        public IPAddress ClientIP;
        private IPEndPoint ServerEndPoint; 
        private Socket UdpSocket;
        public Socket TcpSocket;
        private Thread listenUdpThread;
        private Thread listenTcpThread;
        private Thread ListenDowloadThread;
        private Thread ListenUploadThread;
        public int ClientID = -1;
        public List<FileSet> FileSets;
        public event ReceiveMessage ReceiveMessageEvent;
        private Socket DownloadSocket;
        private string DownloadFile;
        private Socket UploadSocket;
        private List<DownloadClient> clients;
        private const bool SET = true;
        private const bool DELETE = false;
        private const string DownloadDirr = "C:\\Users\\feopl\\Desktop\\сохранить\\";
        private const string UploadDirr = "C:\\Users\\feopl\\Desktop\\загрузить\\";

        public Client()
        {
            UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            DownloadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            UploadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serializer = new Serializer();
            ClientIP = GetIP();
            IPEndPoint localip = new IPEndPoint(ClientIP, localPort);
            FileSets = new List<FileSet>();
            clients = new List<DownloadClient>();
            UdpSocket.Bind(localip);
            TcpSocket.Bind(localip);
            DownloadSocket.Bind(localip);
            UploadSocket.Bind(localip);
            listenUdpThread = new Thread(UDPlistener);
            listenTcpThread = new Thread(TCPListener);
            ListenDowloadThread = new Thread(DownloadListener);
            ListenUploadThread = new Thread(UploadListener);
            ListenDowloadThread.Start();
            ListenUploadThread.Start();

        }
        //Прослушивание сообщений
        public void UDPlistener()
        {
            int byts = 0;
            byte[] dat = new byte[1024];
            EndPoint remoteip = new IPEndPoint(IPAddress.Any, localPort);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                    byts = UdpSocket.ReceiveFrom(dat, ref remoteip);
                    memoryStream.Write(dat, 0, byts);
                }
                while (UdpSocket.Available > 0);
                if (memoryStream.Length > 0)
                {
                    MessageManager(serializer.Deserialize(memoryStream.ToArray()));
                }
            }
        }
        
        public void TCPListener()
        {
            int bytes = 0;
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024];
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        do
                        {
                            bytes = TcpSocket.Receive(data, data.Length, SocketFlags.None);
                            memoryStream.Write(data, 0, bytes);
                        }
                        while (TcpSocket.Available > 0);
                        if (bytes> 0)
                        {
                            MessageManager(serializer.Deserialize(memoryStream.ToArray()));
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Disconnect();
                }
  
            }

        }
        public void DownloadListener()
        {
            int bytes = 0;
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024];
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        do
                        {
                            bytes = DownloadSocket.Receive(data, data.Length, SocketFlags.None);
                            memoryStream.Write(data, 0, bytes);
                        }
                        while (DownloadSocket.Available > 0);
                        if (bytes > 0)
                        {
                            DownloadMessageManager(serializer.Deserialize(memoryStream.ToArray()));
                        }
                    }
                }
                catch (Exception ex)
                {
                }

            }
        }
        public void UploadListener()
        {
            int bytes = 0;
            UploadSocket.Listen(10);
            while (true)
            {
                Socket connectedSocket = UploadSocket.Accept();
                byte[] data = new byte[1024];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    do
                    {
                        bytes = connectedSocket.Receive(data, data.Length, SocketFlags.None);
                        memoryStream.Write(data, 0, bytes);
                    }
                    while (UploadSocket.Available > 0);
                    if (memoryStream.Length > 0)
                    {
                        NewClientAppear(serializer.Deserialize(memoryStream.ToArray()), connectedSocket);
                    }
                }
            }
        }

        //соединение с сервером
        public void UDPConnectServer(string clientName)
        {
            IPEndPoint Broadcast = new IPEndPoint(IPAddress.Broadcast, ServerPort);
            UdpSocket.EnableBroadcast = true;
            UdpSocket.SendTo(serializer.Serialize(MakeServerRequest(clientName)), Broadcast);
            listenUdpThread.Start();
        }

        public bool TCPConnectServer(string clientName, int id)
        {
            try
            {
                TcpSocket.Connect(ServerEndPoint);
                listenTcpThread.Start();
                SendMessage(MakeRegistrationMessage(clientName, id));
                GeneralFunction.CloseSocket(ref UdpSocket);
                GeneralFunction.CloseThread(ref listenUdpThread);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void NewClientAppear(Message message, Socket ConectedSocket)
        {
            if (message is ClientIDMessage)
            {
                ClientIDMessage RegMessage = (ClientIDMessage)message;
                DownloadClient client;
                client = new DownloadClient(RegMessage.Id , ConectedSocket, serializer);
                client.messageManager += UploadMessageManager;
                client.ClientDisconnectedEvent += RemoveConnection;
                clients.Add(client);
            }
        }

        //сортировка сообщений Сервера
        public void MessageManager(Message message)
        {
            if (message is ServerAnswerRequest)
                SerwerAnswerRequest((ServerAnswerRequest)message);
            if (message is ClientEndpoint)
                ClientEndpointManager((ClientEndpoint)message);
            if (message is ClientIDMessage)
                ClientIDMessageManager((ClientIDMessage)message);
            if (message is FileListMessage)
                FileListMessageManager((FileListMessage)message);
            ReceiveMessageEvent(message);
        }
        public void FileListMessageManager(FileListMessage message) 
        {
            FileSets = message.FileSetsList;
            foreach(FileSet file in FileSets)
            {
                if (!File.Exists(UploadDirr + file.FileName))
                {
                    SendMessage(file.FileName, DELETE);
                    break;
                } 
            }
        }
        public void ClientIDMessageManager(ClientIDMessage message)
        {
            ClientID = message.Id;
        }

        public void ClientEndpointManager(ClientEndpoint message)
        {
            DownloadSocket.Connect(message.ClientEndPoint);
            if (DownloadSocket.Connected)
            {
                Thread.Sleep(2000);
                DownloadSocket.Send(serializer.Serialize(new ClientIDMessage(ClientIP, ClientID)));
                Thread.Sleep(2000);
                DownloadSocket.Send(serializer.Serialize(new FileDownloadRequest(ClientIP, message.FileName, ClientID)));
            }
            
        }

        public void SerwerAnswerRequest(ServerAnswerRequest message)
        {
            ServerEndPoint = new IPEndPoint(message.SenderAddress, message.ClientPort);
        }
        //работа клиента при соединении с другим клиентом в качестве клиента 
        public void DownloadMessageManager(Message message)
        {
            if (message is FileHeadderMessage)
                FileHeadderManager((FileHeadderMessage)message);
            if (message is FilePartMessage)
                FilePartManager((FilePartMessage)message);
        }
        public void FileHeadderManager(FileHeadderMessage message)
        {
            DownloadFile = message.FileName;
            FileStream File = new FileStream(DownloadDirr + DownloadFile, FileMode.Create);
            File.Close();
        }

        public void FilePartManager(FilePartMessage message)
        {
            using (FileStream File = new FileStream(DownloadDirr + DownloadFile, FileMode.Append))
            {
                byte[] array = message.Data;
                File.Write(array, 0, array.Length);
                File.Close();
            }
            string extention = Path.GetExtension(DownloadDirr + DownloadFile);
            if (extention == ".jpeg" || extention == ".jpg")
            {
                RLE RLE = new RLE();
                RLE.FileCompression(DownloadDirr + DownloadFile);
            }
            DownloadSocket.Shutdown(SocketShutdown.Both);
            DownloadSocket.Close();
            DownloadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        //работа клиента при соединении с другим клиентом в качестве сервера
        public void UploadMessageManager(Message message)
        {
            if (message is FileDownloadRequest)
                FileDownloadRequestManager((FileDownloadRequest)message);
        }

        public void FileDownloadRequestManager(FileDownloadRequest message)
        {
            DownloadClient downloadClient = null;
            foreach (DownloadClient client in clients)
            {
                if (message.SenderID == client.id)
                {
                    downloadClient = client;
                    break;
                }

            }
            if (File.Exists(UploadDirr + message.FileName) && downloadClient != null)
            {
                downloadClient.tcpSocket.Send(serializer.Serialize(new FileHeadderMessage(ClientIP, message.FileName)));
                int bytes = 0;
                using (FileStream File = new FileStream(UploadDirr + message.FileName, FileMode.Open))
                {
                    byte[] arr = new byte[File.Length];
                    File.Read(arr,bytes,arr.Length);
                    downloadClient.tcpSocket.Send(serializer.Serialize(new FilePartMessage(ClientIP, arr)));
                }
            }
            RemoveConnection(downloadClient);

        }
        public void RemoveConnection(DownloadClient disconnectedClient)
        {
            clients.Remove(disconnectedClient);
            GeneralFunction.CloseSocket(ref disconnectedClient.tcpSocket);
            GeneralFunction.CloseThread(ref disconnectedClient.listenTcpThread);
        }

        //Отправка сообщений Серверу
        public void SendMessage(Message message)
        {
            TcpSocket.Send(serializer.Serialize(message));
        }
        //Создание сообщений для Сервера
        public ServerRequest MakeServerRequest(string clientName)
        {
            IPEndPoint localIp = (IPEndPoint)UdpSocket.LocalEndPoint;
            return new ServerRequest(localIp.Address, localIp.Port, clientName);
        }
        public TCPConnectMessage MakeRegistrationMessage(string clientName, int id)
        {
            return new TCPConnectMessage(ClientIP, clientName, id, UploadSocket.LocalEndPoint);
        }
        public void DownloadFileRequest(string filename)
        {
            SendMessage(new FileDownloadRequest(ClientIP, filename, ClientID));
        }
        public void SendMessage(string filename, bool task)
        {
                FileTaskMessage message = new FileTaskMessage(ClientIP, filename, ClientID, task);
                SendMessage(message);
        }
        //сторонние функции
        public IPAddress GetIP()
        {
            string HostName = Dns.GetHostName();
            return Dns.GetHostByName(HostName).AddressList[2];
        }

        public void Disconnect()
        {
            GeneralFunction.CloseThread(ref listenTcpThread);
            GeneralFunction.CloseSocket(ref TcpSocket);
            GeneralFunction.CloseSocket(ref UdpSocket);
            GeneralFunction.CloseThread(ref listenUdpThread);
            GeneralFunction.CloseSocket(ref DownloadSocket);
            GeneralFunction.CloseThread(ref ListenDowloadThread);
            GeneralFunction.CloseSocket(ref UploadSocket);
            GeneralFunction.CloseThread(ref ListenUploadThread);
        }
        ~Client()
        {
            Disconnect();
        }
    }
}
