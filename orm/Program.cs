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
            Person person1 = new Person(21, "Jerz", "Smith");
            Person person = new Person(31, "Arek", "Nowak");
            mng.insert(person);

            
            Dog dog1 = new Dog(10);
            Dog dog2 = new Dog(21);
            Bowl bowl1 = new Bowl(11);
            Bowl bowl2 = new Bowl(21);
            Bowl bowl3 = new Bowl(31);
            Bowl bowl4 = new Bowl(41);
            // mng.insert(person1);

            dog1.setBowl(bowl1);
            person1.setDog(dog1);
            
            mng.insert(person1);

            mng.insert(bowl2);
            mng.insert(dog2);
            mng.insert(bowl3);
            mng.delete(dog1);
            var changes = new List<Tuple<string, object>> {
                new Tuple<string, object>( "id", "310" )
                 };
            mng.update(dog2, changes);
        }
    }
}
