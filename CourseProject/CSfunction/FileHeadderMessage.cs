using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class FileHeadderMessage:Message
    {
        public string FileName;
        public FileHeadderMessage(IPAddress address, string filename) : base(address)
        {
            this.FileName = filename;
        }
    }
}
