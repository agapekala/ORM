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

            //User user1 = new User(18, "John");
            Person person1 = new Person(1, "John", "Smith");
            Dog dog1 = new Dog(10);
            Bowl bowl1 = new Bowl(7);
            dog1.setBowl(bowl1);
            person1.setDog(dog1);

            
            mng.insert(person1);
            Console.ReadLine();
        }
    }
}
