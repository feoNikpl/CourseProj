using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CSfunction
{
    public class RLE
    {
        public void FileCompression(string Filename)
        {
            int Current = 0;
            byte[] tmp = new byte[2];
            byte Currentsymbol;
            byte Buffer;
            byte length = 0;
            const bool Repeat = true;
            bool State = !Repeat;
            FileStream Filestream = new FileStream(Filename, FileMode.Open);
            byte[] arr = new byte[Filestream.Length];
            Filestream.Read(arr, 0, arr.Length);
            Filestream.Close();
            File.Delete(Filename);
            Filestream = new FileStream(Filename, FileMode.Append);
            Currentsymbol = arr[Current];
            for(Current = 0; Current < (arr.Length-1); Current++)
            {
                Buffer = arr[Current + 1];
                if (Buffer == Currentsymbol && length != 255)
                {
                    if (State == !Repeat)
                    {
                       length = 2;
                       State = Repeat;
                    }
                    else
                    {
                       length++;
                    }
                }
                else
                {
                   tmp[0] = length;
                   tmp[1] = Currentsymbol;
                   Filestream.Write(tmp,0,tmp.Length);
                   length = 1;
                   State = !Repeat;
                }
                Currentsymbol = Buffer;
                        
            }
            tmp[0] = length;
            tmp[1] = Currentsymbol;
            Filestream.Write(tmp, 0, tmp.Length);
            Filestream.Close();
        }
        public void FileDeCompression(string Filename)
        {
            int Current;
            FileStream Filestream = new FileStream(Filename, FileMode.Open);
            byte[] arr = new byte[Filestream.Length];
            Filestream.Read(arr, 0, arr.Length);
            Filestream.Close();
            File.Delete(Filename);
            Filestream = new FileStream(Filename, FileMode.Append);
            byte[] tmp = new byte[1]; 
            for(Current = 0; Current < arr.Length; Current = Current + 2)
            {
                tmp[0] = arr[Current + 1];
                for(byte i = 0; i < arr[Current]; i++)
                    Filestream.Write(tmp, 0, 1);
            }
            Filestream.Close();
        }
    }
}
