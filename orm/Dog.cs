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
        [PrimaryKey]
        private int _id { get; set; }

        [Column("miska")]
        [OneToOne()]
        private Bowl _bowl { get; set; }

        public void setBowl(Bowl bowl) {
            _bowl = bowl;
        }

        public Dog(int id) {
            _id = id;
        }
    }
}
