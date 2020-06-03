using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class FileDownloadRequest : Message
    {
        public string FileName;
        public int SenderID;
        public FileDownloadRequest (IPAddress address, string filename, int SenderID) : base(address)
        {
            this.FileName = filename;
            this.SenderID = SenderID;
        }
    }
}
