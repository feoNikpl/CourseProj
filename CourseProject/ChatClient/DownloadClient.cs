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

namespace ChatClient
{
    class DownloadClient
    {
        public Socket tcpSocket;
        public int id;
        public Thread listenTcpThread;
        private Serializer serializer;
        public delegate void UploadMessageManager(Message message);
        public event UploadMessageManager messageManager;
        public delegate void ClientDisconnected(DownloadClient client);
        public event ClientDisconnected ClientDisconnectedEvent;

        public DownloadClient(int id, Socket socket, Serializer serializer)
        {
            this.serializer = serializer;
            this.id = id;
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
