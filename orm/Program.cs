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
            Person person1 = new Person(1, "Jerz", "Smith");
            Person person = new Person(4141121, "Arek", "Nowak");
            mng.insert(person);
            var del = new List<Tuple<string, object>> {
                new Tuple<string, object>( "id", "414111" )
            };
            mng.delete(person, del);
            
            Dog dog1 = new Dog(134101);
            Dog dog2 = new Dog(213401);
            Bowl bowl1 = new Bowl(51);
            Bowl bowl2 = new Bowl(515);
            Bowl bowl3 = new Bowl(5515);
            Bowl bowl4 = new Bowl(55515);
            // mng.insert(person1);

            dog1.setBowl(bowl1);
            person1.setDog(dog1);
            
            mng.insert(person1);

            mng.insert(bowl2);
            mng.insert(dog2);
            mng.insert(bowl3);

            var changes = new List<Tuple<string, object>> {
                new Tuple<string, object>( "id", "30" )
                 };
            mng.update(dog1, changes);
        }
    }
}
