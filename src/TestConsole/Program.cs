using iAnywhere.Data.SQLAnywhere;
using System;
using System.Data.Common;

namespace TestConsole
{
    public class Program
    {
        static SAConnection conn;

        static DbCommand GetDBCommand()
        {
            var conn = GetConnection();

            return conn.CreateCommand();
        }

        static SAConnection GetConnection()
        {
            if (conn != null)
                return conn;

            conn = new SAConnection(GetConnectionString());
            conn.Open();
            return conn;
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
            //using (var cmd = GetDBCommand())
            //{
            //    cmd.CommandText = "SELECT * FROM KG_PG";
            //    using (var reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            for (var i = 0; i < reader.FieldCount; i++) { }                            
            //        }
            //    }               
            //}

            using (var cmd = GetDBCommand())
            {
                cmd.CommandText = "SELECT * FROM KG_PG WHERE PGNAME = :p1";
                var param = cmd.CreateParameter();
                param.Value = "Graz";
                cmd.Parameters.Add(param);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                            Console.WriteLine(reader.GetName(i) + ": " + reader.GetString(i));
                    }
                }
            }

            conn.Dispose();

            Console.ReadKey();
        }
    }
}
