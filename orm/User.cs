using System;
using System.Collections.Generic;
using System.Text;
using orm.Attributes;

namespace orm
{
    [Table("Users")]
    class User
    {
        [Column("id")]
        private int _id { get; set; }
        [Column("name")]
        private string _name { get; set; }

        public User(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public void SetName(string name)
        {
            _name = name;
        }
    }
}
