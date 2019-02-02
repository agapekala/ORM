using System;
using System.Collections.Generic;
using System.Text;

namespace orm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class PrimaryKeyAttribute : Attribute
    { 
        public PrimaryKeyAttribute()
        {
        }
    }
}
