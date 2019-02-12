using System;
using System.Collections.Generic;
using System.Text;
using orm.Attributes;
namespace orm
{
    [Table("Dog")]
    class Dog
    {
        [Column("id")]
        [PrimaryKey()]
        protected int _id { get; set; }

        [Column("miska")]
        [OneToOne()]
        protected Bowl _bowl { get; set; } = null;

        public void setBowl(Bowl bowl)
        {
            _bowl = bowl;
        }

        public Dog(int id, Bowl bowl = null)
        {
            _id = id;
            _bowl = bowl;
        }

        public Dog() { }
    }
}
