using System;
using System.Collections.Generic;
using System.Text;

namespace orm.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public string TableName { get; set; }

        //Default table name is lowercase class name
        public TableAttribute(string tableName = null)
        {
            TableName = tableName;
        }
    }
}
