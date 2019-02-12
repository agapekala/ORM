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

        [OneToMany()]
        private LinkedList <Cat> _cats { get; set; }

        public void setDog(Dog dog) {
            _dog = dog;
        }

        public void setCats(LinkedList<Cat> cats) {
            if (_cats == null) {
                _cats = new LinkedList<Cat>();
            }
            this._cats = cats;
        }

        public void addCat(Cat cat) {
            if (_cats == null)
            {
                _cats = new LinkedList<Cat>();
            }
            _cats.AddLast(cat);
        }


        public Person(int id, string name, string lastname)
        {
            _id = id;
            _name = name;
            _lastname = lastname;
            _dog = null;
            _cats = null;
        }
    }
}
