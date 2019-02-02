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
                return convertObjectNameToString(t);
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

                // If no attribute was set convert field's name into string. Otherwise take string from attribute.
                if (att.Length == 0) {
                    string columnName = convertObjectNameToString(prp.Name);
                    list.Add(columnName);;
                }
                else{
                    foreach (ColumnAttribute atr in att)
                    {
                        list.Add(atr.ColumnName);
                    }
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

                // If no attribute was set convert field's name into string. Otherwise take string from attribute.
                string columnName;
                if (att.Length == 0)
                {
                    columnName = convertObjectNameToString(prp.Name);
                }
                else { 
                    ColumnAttribute att1 = (ColumnAttribute)att[0];
                    columnName = att1.ColumnName;
                }

                list.Add(new Tuple<string, object>(columnName, val));
                Console.WriteLine(val);
            }
            return list;
        }

        // Function that is used, when no attribute was set.
        public string convertObjectNameToString(Object t) {
            string nameWithNamespaces = t.ToString();
            int appearanceOfLastFullStop = -1;

            // Finding an index of fullstop, which appears right before class name.
            for (int i = nameWithNamespaces.Length - 1; i>0; i--) {
                if (nameWithNamespaces[i] == '.') {
                    appearanceOfLastFullStop = i;
                }
            }
            string nameWithoutNamespaces = nameWithNamespaces.Substring(appearanceOfLastFullStop+1);
            return nameWithoutNamespaces;
        }

    }

}