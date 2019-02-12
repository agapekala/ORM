using System;
using System.Collections.Generic;
using System.Text;
using orm.Attributes;


namespace orm
{
    [Table("Cats")]
    class Cat
    {
        [Column("id")]
        [PrimaryKey]
        protected int _id { get; set; }

        [Column("miska")]
        [OneToOne()]
        protected Bowl _bowl { get; set; }

        public void setBowl(Bowl bowl)
        {
            _bowl = bowl;
        }

        public Cat(int id)
        {
            this._id = id;
        }
    }
}