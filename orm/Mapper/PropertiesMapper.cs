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
                }
            }
            return list;
        }

        public List<Tuple<string, object>> getColumnAndValue(Object obj)
        {
            List<Tuple<string, object>> list = new List<Tuple<string, object>> { };

            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo prp in props)
            {
                MethodInfo strGetter = prp.GetGetMethod(nonPublic: true);

                var val = strGetter.Invoke(obj, null);

                object[] att = prp.GetCustomAttributes(typeof(ColumnAttribute), false);
                ColumnAttribute att1 = (ColumnAttribute)att[0];

                list.Add(new Tuple<string, object>(att1.ColumnName,val));
                Console.WriteLine(val);
            }
            return list;
        }
    }

}