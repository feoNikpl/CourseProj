using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSfunction;

namespace ChatServer
{
    class Client
    {
        public string name;
        public Socket tcpSocket;
        public EndPoint ClientEndpoint;
        public int id;
        public Thread listenTcpThread;
        private Serializer serializer;
        public delegate void MessageManager(Message message);
        public event MessageManager messageManager;
        public delegate void ClientDisconnected(Client client);
        public event ClientDisconnected ClientDisconnectedEvent;

        public Client(string name, int id,Socket socket, Serializer serializer, EndPoint clientEndpoint)
        {
            this.serializer = serializer;
            this.name = name;
            this.id = id;
            this.ClientEndpoint = clientEndpoint;
            tcpSocket = socket;
            listenTcpThread = new Thread(ListenTcp);
            listenTcpThread.Start();
        }

        public void ListenTcp()
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
                            bytes = tcpSocket.Receive(data, data.Length, SocketFlags.None);
                            memoryStream.Write(data, 0, bytes);
                        }
                        while (tcpSocket.Available > 0);
                        if (memoryStream.Length > 0)
                        {
                            messageManager(serializer.Deserialize(memoryStream.ToArray()));
                        }
                    }
                }

                catch (SocketException)

                {
                    ClientDisconnectedEvent(this);
                    GeneralFunction.CloseSocket(ref tcpSocket);
                    GeneralFunction.CloseThread(ref listenTcpThread);
                }
            }
        }
    }
}
