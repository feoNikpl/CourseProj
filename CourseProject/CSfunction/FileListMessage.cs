using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class FileListMessage:Message
    {
        public List<FileSet> FileSetsList;
        public FileListMessage(IPAddress address, List<FileSet> fileSetsList) : base(address)
        {
            this.FileSetsList = fileSetsList;
        }
    }
}