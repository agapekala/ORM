using System;
using System.Collections.Generic;
using System.Text;

namespace orm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName { get; private set; }

        public ColumnAttribute(string columnName = null)
        {
            ColumnName = columnName;
        }

    }
}
