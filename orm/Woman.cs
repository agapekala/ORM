using System;
using System.Collections.Generic;
using System.Text;
using orm.Attributes;


namespace orm
{
    [Table("Women")]
    class Woman : Person
    {
        [Column("wlosy")]
        protected string hair { get; set; }
        public Woman(int id, string name, string last_name, string hair) : base(id, name, last_name)
        {

            this.hair = hair;
        }

        public String getHair()
        {
            return this.hair;
        }
    }
}