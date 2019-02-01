using System;
using orm.Configuration;
using orm.Connection;
using System.Data.SqlClient;
using System.Reflection;
using orm.Attributes;

namespace orm
{
    class Program
    {
        static void Main(string[] args)
        {
            MSSqlConnection conn = MSSqlConnection.GetInstance();
            ConnConfiguration conf = new ConnConfiguration("DESKTOP-OP36O3L\\SQLEXPRESS", "Test");
            conn.setConfiguration(conf);
            Manager mng = new Manager(conn);

            User user = new User(5, "Maria");
            //mng.insert(user);
            TableAttribute attr = (TableAttribute)Attribute.GetCustomAttribute(user.GetType(), typeof(TableAttribute));

            if (attr == null)
            {
                Console.WriteLine("The attribute was not found.");
            }
            else
            {
                // Get the Name value.
                Console.WriteLine("The Name Attribute is: {0}.", attr.TableName);
            }

            //conn.ConnectAndOpen();
            //SqlDataReader r=conn.executeReader(conn.execute("SELECT * FROM Users; "));
            //Console.WriteLine("Wiersze tabeli:");
            //while (r.Read())
            //{
            //    Console.WriteLine(r["id"].ToString() + "   " + r["name"].ToString());
            //}
            //r.Close();
            //conn.Dispose();

            Console.WriteLine("Hello World!");

        }
    }
}
