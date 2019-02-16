using System;
using System.Collections.Generic;
using System.Text;
using orm.Attributes;

namespace orm
{
    [Table("Bowls")]
    class Bowl
    {
        [Column("id")]
        [PrimaryKey]
        protected int _id { get; set; }

        public Bowl(int id)
        {
            _id = id;
        }

        public int getId() {
            return _id;
        }

        public Bowl() {
        }
    }
}
