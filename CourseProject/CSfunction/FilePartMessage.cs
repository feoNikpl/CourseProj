using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class FilePartMessage:Message
    {
        public byte[] Data;
        public FilePartMessage(IPAddress address, byte[] data) : base(address)
        {
            this.Data = data;
        }
    }
}
