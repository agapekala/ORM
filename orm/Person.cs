using System;
using System.Collections.Generic;
using System.Text;
using orm.Attributes;

namespace orm
{
    [Table("Person")]
    class Person
    {
        [Column("id")]
        [PrimaryKey()]
        private int _id { get; set; }

        [Column("imie")]
        private string _name { get; set; }

        [Column("nazwisko")]
        private string _lastname { get; set; }

        [Column("pies")]
        [OneToOne()]
        private Dog _dog { get; set; }


        public void setDog(Dog dog)
        {
            _dog = dog;
        }

        public Person(int id, string name, string lastname)
        {
            _id = id;
            _name = name;
            _lastname = lastname;
        }
    }
}
