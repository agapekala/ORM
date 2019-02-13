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
        protected int _id { get; set; }

        [Column("imie")]
        protected string _name { get; set; }

        [Column("nazwisko")]
        protected string _lastname { get; set; }

        [Column("pies")]
        [OneToOne()]
        protected Dog _dog { get; set; }

        [OneToMany()]
        protected List<Cat> _cats { get; set; } = new List<Cat>();

        public void setDog(Dog dog)
        {
            _dog = dog;
        }

        public List<Cat> getCats() {
            return _cats;
        }

        public Dog getDog()
        {
            return _dog;
        }

        public string getName() {
            return _name;
        }

        public string getLastname() {
            return _lastname;
        }

        public int getId() {
            return _id;
        }
        public void setCats(List<Cat> cats)
        {
            if (_cats == null)
            {
                _cats = new List<Cat>();
            }
            this._cats = cats;
        }

        public void addCat(Cat cat)
        {
            if (_cats == null)
            {
                _cats = new List<Cat>();
            }
            _cats.Add(cat);
        }

        public Person() { }
        public Person(int id, string name, string lastname)
        {
            _id = id;
            _name = name;
            _lastname = lastname;
            _dog = null;
            _cats = null ;
        }
    }
}
