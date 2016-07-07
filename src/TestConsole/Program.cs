using iAnywhere.Data.SQLAnywhere;
using Provider.Tests;
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
            fac.ServerName = "test";
            fac.UserID = "dba";
            fac.Password = "sql";
            fac.Integrated = "true";
            return fac.ToString();
        }

        public static void Main(string[] args)
        {
            new SqlAnywhereDBContextTests().NewSqlAnyhwereDBContectShouldNotThrowOnNew();
            new SqlAnywhereDBContextTests().AddNewBlogPostShouldAffect1Record();
                //TestSelect();

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void NewMethod()
        {
            using (var cmd = GetDBCommand())
            {
                cmd.CommandText = "SELECT * FROM BLOG WHERE blogid = :p1";
                var param = cmd.CreateParameter();
                param.Value = "1";
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
        }
    }
}
