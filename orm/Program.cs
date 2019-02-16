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

using orm.Query;


namespace orm
{
    class Program
    {
        static void Main(string[] args)
        { 
            MSSqlConnection conn = MSSqlConnection.GetInstance();
            // ConnConfiguration conf = new ConnConfiguration("localhost", "tmp", "SA", "Cezarypazura1");
            ConnConfiguration conf = new ConnConfiguration("KAROLINA-PC\\SQLEXPRESS", "Test");
            conn.setConfiguration(conf);
            Manager mng = new Manager(conn);
            
            // User's set-up.
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

            Woman woman = new Woman(15, "Anna", "Nowak", "blond");
            Dog dogWoman = new Dog(11);
            Bowl bowlWoman = new Bowl(21);
            dogWoman.setBowl(bowlWoman);
            woman.setDog(dogWoman);
            Cat catWoman = new Cat(31);
            Cat catWoman2 = new Cat(32);
            Cat catWoman3 = new Cat(33);
            Bowl bowlCatWoman = new Bowl(51);
            Bowl bowlCatWoman2 = new Bowl(52);
            Bowl BowlCatWoman3 = new Bowl(53);
            catWoman.setBowl(bowlCatWoman);
            catWoman2.setBowl(bowlCatWoman2);
            catWoman3.setBowl(BowlCatWoman3);
            woman.addCat(catWoman);
            woman.addCat(catWoman2);
            woman.addCat(catWoman3);


            // Creating a Bowl Table.
            mng.createTable(bowl1);


            // Inserting an object into database.
            // mng.insert(woman);
            // mng.insert(person1);

            // Selecting an object by Id.
            Cat selectedCat = (Cat)mng.selectById(typeof(Cat), 32);
            if (selectedCat == null)
            {
                Console.WriteLine("Such object doesn't exist.");
            }
            else{
                Console.WriteLine("Selected Item: ");
                Console.WriteLine("catId = "+ selectedCat.getId());
                if (selectedCat.getBowl() != null) {
                    Console.WriteLine("bowlId = " + selectedCat.getBowl().getId());
                }
            }


            // Selecting an object from database and creating list of Criteria.
            List<Criteria> myCriterias = new List<Criteria>();
            myCriterias.Add(Criteria.greaterThan("id", 0));

            IEnumerable<object> dogList = (IEnumerable<object>) mng.select(typeof(Dog), myCriterias);
            if (dogList != null) {
                Console.WriteLine("");
                Console.WriteLine("Selected Items:");
                foreach (Dog itDog in dogList) {
                    Console.WriteLine("dogId = " + itDog.getId());
                    //if (itDog.getBowl() != null) { 
                        Console.WriteLine("bowlId = " + itDog.getBowl().getId());
                    //}
                }
            }
            else{
                Console.WriteLine("Such objects don't exist.");
            }



            // Updating an object with the usage of criteria.
            /*List<Criteria> updateCriterias = new List<Criteria>();
            updateCriterias.Add(Criteria.equals("id", 15));
            

            Dog dogUpdate = new Dog(9);

            var updateChanges = new List<Tuple<string, object>> {
                // new Tuple<string, object> ("nameOfColumn", value)
                new Tuple<string, object>("pies", dogUpdate),
                new Tuple<string, object>("wlosy", "czarne")
            };
            mng.update(typeof(Woman), updateChanges, updateCriterias);
            */


            // Deleting an object with usage of criteria.
            /*List<Criteria> deleteCriterias = new List<Criteria>();
            deleteCriterias.Add(Criteria.equals("id", 53));

            mng.delete(typeof(Bowl), deleteCriterias);
            */

        }
    }
}
