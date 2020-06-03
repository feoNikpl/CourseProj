using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class ClientIDMessage:Message
    {
        public int Id;
        public ClientIDMessage(IPAddress address, int id) : base(address)
        {
            this.Id = id;
        }
    }
}
