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
using orm.Criterias;


namespace orm
{
    class Program
    {
        static void Main(string[] args)
        {
            MSSqlConnection conn = MSSqlConnection.GetInstance();
            //ConnConfiguration conf = new ConnConfiguration("localhost", "tmp", "SA", "Cezarypazura1");
            ConnConfiguration conf = new ConnConfiguration("KAROLINA-PC\\SQLEXPRESS", "Test");
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
            
            Criteria.greaterThan("id", 0);
            Criteria.getListOfCriteria();

            List<Criteria> myCriterias = new List<Criteria>();
            myCriterias.Add(Criteria.greaterThan("id", 0));
            //LinkedList<Bowl> woman = (LinkedList<Bowl>)mng.select( typeof(Bowl), Criteria.getListOfCriteria());

            /*IEnumerable<object> woman = (IEnumerable<object>) mng.select( typeof(Woman), myCriterias);

              foreach (Woman w in woman ){
                  Console.WriteLine("personId = " + w.getId());
                  Console.WriteLine("personName = " + w.getName());
                  Console.WriteLine("personLastname = " + w.getLastname());
                  Console.WriteLine("personHair = " + w.getHair());
              }
            */
            //Dog d = (Dog) mng.select(/*typeof(Dog)*/ typeof(Dog), 10);
            Woman p = (Woman)mng.selectById(/*typeof(Dog)*/ typeof(Woman), 3);
            
           /* Cat cat = (Cat)mng.selectById(typeof(Cat), 11);
            if (cat == null || cat.getId() == null) {
                Console.WriteLine("is null");
            }
            else {
                Console.WriteLine("catId = " + cat.getId());
                Console.WriteLine("miskaId = " + cat.getBowl().getId());
            }
            */
            if (p == null || p.getId() == null) {
                Console.WriteLine("The object doesn't exist.");
            }
            else { 
                Console.WriteLine("personId = " + p.getId());
                Console.WriteLine("personName = " + p.getName());
                Console.WriteLine("personLastname = " + p.getLastname());
                Console.WriteLine("personHair = " + p.getHair());
                Console.WriteLine("piesId = " + p.getDog().getId().ToString());
                Console.WriteLine("bowlId = " + p.getDog().getBowl().getId());
                foreach (Cat o in p.getCats()) {
                    Console.WriteLine("catId= " + o.getId());
                    Console.WriteLine("catsBowlId=" + o.getBowl().getId());
                }
                
            }
        }
    }
}
