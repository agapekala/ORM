using System;
using System.Collections.Generic;
using System.Reflection;
using orm.Attributes;
using System.Data.SqlClient;
using System.Linq;
using System.Collections;

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
                return attr.TableName;
            }
        }

        //        public Boolean primaryKeyExists(Object t)
        //        {
        //                PrimaryKeyAttribute attr = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(t.GetType(), typeof(PrimaryKeyAttribute));
        //                Console.Write(attr.PrimaryKeyName);
        //                if (attr == null)
        //                {
        //                    Console.WriteLine("Such a primary key already exists");
        //                    return true;
        //                }else
        //                {
        //                    Console.WriteLine("New primary key");
        //                    return false;
        //                }
        //        }
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

                if (att.Length == 0)
                {
                    continue; // If certain field is not declared as column, skip it.
                }

                string columnName;
                ColumnAttribute att1 = (ColumnAttribute)att[0];

                // If no attribute was set convert field's name into string. Otherwise take string from attribute.
                if (att1.ColumnName == null)
                {
                    columnName = convertObjectNameToString(prp.Name);
                }
                else
                {
                    columnName = att1.ColumnName;
                }

                // If this property has an OneToOneAttribute, find forgein key.
                object[] oneToOneAtt = prp.GetCustomAttributes(typeof(OneToOneAttribute), false);
                if (oneToOneAtt.Length != 0)
                {
                    if (val != null)
                    {
                        var forgeinKey = findPrimaryKey(val);
                        val = forgeinKey;
                    }
                    else
                    {
                        val = "null";

                    }
                }
                list.Add(new Tuple<string, object>(columnName, val));
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


        // Returns the name of the field that is set to be a primary key.
        public string findPrimaryKeyFieldName(object obj) {
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
                    string columnName;
                    object[] attColumn = prp.GetCustomAttributes(typeof(ColumnAttribute), false);
                    ColumnAttribute att1 = (ColumnAttribute)attColumn[0];
                    if (att1.ColumnName == null)
                    {
                        columnName = convertObjectNameToString(prp.Name);
                    }
                    else
                    {
                        columnName = att1.ColumnName;
                    }
                    return columnName;
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


        public Dictionary<string, object> createDictionaryFromTable(SqlDataReader reader) {
            Dictionary<string, object> columnNameAndItsValue = new Dictionary<string, object>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columnNameAndItsValue.Add(reader.GetName(i), reader[i]);
                    string formatString = "{0}";
                    // Console.Write(String.Format(formatString, reader[i]));
                    // Console.WriteLine(String.Format(formatString, reader.GetName(i)));
                }
            }
            return columnNameAndItsValue;
        }

        public object getValueOfForeignKey(PropertyInfo prp, SqlDataReader reader) {
            Dictionary<string, object> columnNameAndItsValue = createDictionaryFromTable(reader);
            reader.Close();
            if (columnNameAndItsValue.Count == 0) {
                return null;
            }
            object[] attColumn = prp.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (attColumn.Length == 0)
            {
                // TO-DO: sprawdü, czy jest OneToMany
                // string columnName = convertObjectNameToString(prp.Name);
                // list.Add(columnName); ;
            }
            else
            {
                string columnNameInObject;
                foreach (ColumnAttribute atr in attColumn)
                {
                    if (atr.ColumnName == null)
                    {
                        columnNameInObject = convertObjectNameToString(prp.Name);
                    }
                    else
                    {
                        columnNameInObject = atr.ColumnName;
                    }
                    return columnNameAndItsValue[columnNameInObject];
                }
            }
            
            return null; //TO-DO: handle exception, when value is not set.
        }

        public object mapTableIntoObject(object obj, SqlDataReader reader) {

            // Getting values and column names from database.
            Dictionary<string, object> columnNameAndItsValue = createDictionaryFromTable(reader);
            reader.Close();

            Type type = obj.GetType();  // Name of a class.
            PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo prp in props)
            {
                MethodInfo strGetter = prp.GetGetMethod(nonPublic: true); //get id()  (column)
                                                                          
                object[] attColumn = prp.GetCustomAttributes(typeof(ColumnAttribute), false);
                object[] attOneToOne = prp.GetCustomAttributes(typeof(OneToOneAttribute), false);
                // If no attribute was set convert field's name into string. Otherwise take string from attribute.
                if (attColumn.Length == 0)
                {
                    // TO-DO: sprawdü, czy jest OneToMany
                    string columnName = convertObjectNameToString(prp.Name);
                }
                else
                {                   
                    if (attOneToOne.Length != 0) { continue; }
                    string columnNameInObject;
                    foreach (ColumnAttribute atr in attColumn)
                    {
                        if (atr.ColumnName == null)
                        {
                            columnNameInObject = convertObjectNameToString(prp.Name);
                        }
                        else
                        {
                            columnNameInObject = atr.ColumnName;
                        }
                        var value = columnNameAndItsValue[columnNameInObject];
                        prp.SetValue(obj, value, null);
                    }
                }
            }

            return obj;
        }

        public object setCertainListField(object parent, object children, PropertyInfo prp) {
            IList childTmp = children as IList;
            IList list = Activator.CreateInstance(prp.PropertyType) as IList;
            foreach (var it in childTmp) {
                list.Add(it);
            }
            prp.SetValue(parent, list, null);
            return parent; 
        }

        public object setCertainField(object parent, object child, PropertyInfo prp) {
            prp.SetValue(parent, child, null);
            
            return parent;
        }
    }

}