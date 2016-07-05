using iAnywhere.Data.SQLAnywhere;
using System;
using System.Data.Common;

namespace TestConsole
{


    public class Program
    {
        static DbCommand GetDBCommand()
        {
            var conn = new SAConnection(GetConnectionString());
            conn.Open();

            return conn.CreateCommand();
        }

        static string GetConnectionString()
        {
            var fac = new SAConnectionStringBuilder();
            fac.ServerName = "paradat11";
            fac.UserID = "dba";
            fac.Password = "sql";
            fac.Integrated = "true";
            return fac.ToString();
        }


        public static void Main(string[] args)
        {
            var cmd = GetDBCommand();
            cmd.CommandText = "SELECT * FROM KANZLEI";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                        Console.WriteLine(reader.GetName(i) + ": " + reader.GetString(i));
                }
            }

            Console.ReadKey();
        }
    }
}
