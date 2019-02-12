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
            ConnConfiguration conf = new ConnConfiguration("localhost", "tmp", "SA", "Cezarypazura1");
            conn.setConfiguration(conf);
            // conn.ConnectAndOpen();
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

            Cat cat1 = new Cat(11);
            Cat cat2 = new Cat(12);
            Bowl bowlCat1 = new Bowl(13);
            Bowl bowlCat2 = new Bowl(14);
            cat1.setBowl(bowlCat1);
            cat2.setBowl(bowlCat2);
            person1.addCat(cat1);
            person1.addCat(cat2);

            mng.insert(person1);
        }
    }
}
