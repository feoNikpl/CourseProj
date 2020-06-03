using System;
using System.Collections.Generic;
using System.Text;

namespace CSfunction
{
    [Serializable]
    public class FileSet
    {
        public string FileName;
        public int HostID;
        public FileSet(string name, int id)
        {
            this.FileName = name;
            this.HostID = id;
        }
    }
}
