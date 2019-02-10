using System;
using System.Collections.Generic;
using System.Reflection;
using orm.Attributes;

namespace orm.Mapper
{

    class PropertiesMapper
    {

        // Fetches the TablesAttribute of the class.
        public string getTableName(Object t)
        {
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
            List<string> list = new List<string> { };
            Type type = t.GetType();  //name of a class
                                      //            Console.WriteLine("type " + type);
            PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo prp in props) //column names
            {
                //                Console.WriteLine("prop " + prp);
                MethodInfo strGetter = prp.GetGetMethod(nonPublic: true); //get id()  (column)
                                                                          //                Console.WriteLine("strGetter  " + strGetter);
                object[] att = prp.GetCustomAttributes(typeof(ColumnAttribute), false);
                var val = strGetter.Invoke(t, null); //attribute np.1, John
                                                     //                Console.WriteLine("val " + val);  
                                                     // If no attribute was set convert field's name into string. Otherwise take string from attribute.
                if (att.Length == 0)
                {
                    string columnName = convertObjectNameToString(prp.Name);
                    list.Add(columnName); ;
                }
                else
                {
                    foreach (ColumnAttribute atr in att)
                    {
                        //column name 
                        //                        Console.WriteLine("atr.ColumnName  " + atr.ColumnName);
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
                else
                {
                    ColumnAttribute att1 = (ColumnAttribute)att[0];
                    columnName = att1.ColumnName;
                }


                // If this property has an OneToOneAttribute, find forgein key.
                object[] oneToOneAtt = prp.GetCustomAttributes(typeof(OneToOneAttribute), false);
                if (oneToOneAtt.Length != 0)
                {
                    var forgeinKey = findPrimaryKey(val);
                    val = forgeinKey;
                }


                list.Add(new Tuple<string, object>(columnName, val));
                Console.WriteLine(val);
            }
            return list;
        }

        public object findPrimaryKey(object obj)
        {
            object primaryKey;
            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo prp in props)
            {
                MethodInfo strGetter = prp.GetGetMethod(nonPublic: true);

                primaryKey = strGetter.Invoke(obj, null);
                object[] att = prp.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                if (att.Length != 0)
                {
                    return primaryKey;
                }
            }
            return null;    //TO-DO: Handle exception!!!
        }

        // Function that is used, when no attribute was set.
        public string convertObjectNameToString(Object t)
        {
            string nameWithNamespaces = t.ToString();
            int appearanceOfLastFullStop = -1;

            // Finding an index of fullstop, which appears right before class name.
            for (int i = nameWithNamespaces.Length - 1; i > 0; i--)
            {
                if (nameWithNamespaces[i] == '.')
                {
                    appearanceOfLastFullStop = i;
                }
            }
            string nameWithoutNamespaces = nameWithNamespaces.Substring(appearanceOfLastFullStop + 1);
            return nameWithoutNamespaces;
        }

    }

}