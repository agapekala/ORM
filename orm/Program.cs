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
            //ConnConfiguration conf = new ConnConfiguration("localhost", "tmp", "SA", "Cezarypazura1");
            ConnConfiguration conf = new ConnConfiguration("DESKTOP-OP36O3L\\SQLEXPRESS", "Test");
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
            //Person person1 = new Person(1, "John", "Smith");
            /*
                        Woman person1 = new Woman(1, "John", "Smith", "czarne");
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

            //mng.insert(person1);

            Woman p = (Woman)mng.select(typeof(Woman), 1);
            Console.WriteLine("personId = " + p.getId());
            Console.WriteLine("personName = " + p.getName());
            Console.WriteLine("personLastname = " + p.getLastname());
            Console.WriteLine("piesId = " + p.getDog().getId().ToString());
            Console.WriteLine("bowlId = " + p.getDog().getBowl().getId());
            Console.WriteLine("Hair = " + p.getHair());
            */
            List<object> value =new List<object>() { 1,2,3,4} ;
            var containedType = typeof(int).GenericTypeArguments.First();
            value.Select(item => Convert.ChangeType(item, containedType)).ToList();

        }
    }
}
