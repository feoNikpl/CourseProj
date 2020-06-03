using System;
using System.Data.SqlClient;
using System.Data;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server server = new Server();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
