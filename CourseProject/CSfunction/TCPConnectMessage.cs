using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class TCPConnectMessage:Message
    {
        public string Name;
        public int Id;
        public EndPoint ClientEndPoint;
        public TCPConnectMessage(IPAddress address, string name, int id, EndPoint endPoint) : base(address)
        {
            this.Id = id;
            this.Name = name;
            this.ClientEndPoint = endPoint;
        }
    }
}
