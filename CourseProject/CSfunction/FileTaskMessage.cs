using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class FileTaskMessage:Message
    {
        public int HostID;
        public string FileName;
        public bool SetDelete;
        public FileTaskMessage(IPAddress address, string filename, int reciverID, bool setdelete) :base(address)
        {
            this.HostID = reciverID;
            this.FileName = filename;
            this.SetDelete = setdelete;
        }
    }
}
