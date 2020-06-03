using System;
using System.Net;

namespace CSfunction
{
    [Serializable]
    public class Message 
    {
        public IPAddress SenderAddress;

        public Message(IPAddress SenderAddress)
        {
            this.SenderAddress = SenderAddress;
        }
    }
}
