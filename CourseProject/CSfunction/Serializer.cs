using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CSfunction
{
    public class Serializer
    {
        private BinaryFormatter Bin;
        public Serializer()
        {
            Bin = new BinaryFormatter();
        }
        public byte[] Serialize(Message message)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Bin.Serialize(memoryStream, message);
                return memoryStream.ToArray();
            }
        }
        public Message Deserialize(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (Message)Bin.Deserialize(memoryStream);

            }
        }
    }
}
