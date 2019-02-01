using System;
using System.Collections.Generic;
using System.Text;

namespace orm
{
    class User
    {
        private int _id;
        private string _name;

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
