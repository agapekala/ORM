using System;
using orm.Configuration;
using orm.Connection;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using orm.Attributes;
using orm.Mapper;
using System.Collections.Generic;

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
            //conn.ConnectAndOpen();
            //SqlDataReader r=conn.executeReader(conn.execute("SELECT * FROM Users; "));
            //Console.WriteLine("Wiersze tabeli:");
            //while (r.Read())
            //{
            //    Console.WriteLine(r["id"].ToString() + "   " + r["name"].ToString());
            //}
            //r.Close();
            //conn.Dispose();
            
            User user1 = new User(18, "John");
            PropertiesMapper mapper = new PropertiesMapper();
            mapper.getTableName(user1);

            Type t = user1.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo prp in props)
            {
                MethodInfo strGetter = prp.GetGetMethod(nonPublic: true);

                object[] att = prp.GetCustomAttributes(typeof(ColumnAttribute), false);
                var val = strGetter.Invoke(user1, null);

                foreach (ColumnAttribute atr in att)
                {
                    Console.WriteLine(atr.ColumnName);
                }

                // object value = prp.GetValue(user1, new object[] { });
                Console.WriteLine(prp.Name);
                Console.WriteLine(val);
            }

            Console.WriteLine("Hello World!");

        }
    }
}
