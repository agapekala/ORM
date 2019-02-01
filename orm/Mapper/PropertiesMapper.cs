using System;
using System.Collections.Generic;
using System.Reflection;
using orm.Attributes;

namespace orm.Mapper {

    class PropertiesMapper { 

        // Fetches the TablesAttribute of the class.
        public string getTableName(Object t) {
            TableAttribute attr = (TableAttribute)Attribute.GetCustomAttribute(t.GetType(), typeof(TableAttribute));
            if (attr == null)
            {
                Console.WriteLine("The attribute was not found.");
                return null;
            }
            else
            {
                Console.WriteLine("The Name Attribute is: {0}.", attr.TableName);
                return attr.TableName;
            }
            
        }
        public List<string> getColumnName(Object t)
        {
            List<string> list= new List<string> { };
            Type type = t.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo prp in props)
            {
                MethodInfo strGetter = prp.GetGetMethod(nonPublic: true);

                object[] att = prp.GetCustomAttributes(typeof(ColumnAttribute), false);
                var val = strGetter.Invoke(t, null);

                foreach (ColumnAttribute atr in att)
                {
                    list.Add(atr.ColumnName);
                    //Console.WriteLine(atr.ColumnName);
                }
            }
            return list;
        }
    }

}