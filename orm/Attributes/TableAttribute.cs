using System;

namespace orm.Attributes {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public string TableName { get; set; }
       
        public TableAttribute(string tableName = null)
        {
            TableName = tableName;
        }
    }
}