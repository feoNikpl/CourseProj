using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class ClientEndpoint:Message
    {
        public EndPoint ClientEndPoint;
        public string FileName;
        public ClientEndpoint (IPAddress address, EndPoint endPoint, string filename) : base(address)
        {
            this.ClientEndPoint = endPoint;
            this.FileName = filename;
        }
    }
}
