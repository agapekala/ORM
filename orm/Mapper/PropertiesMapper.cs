using System;
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

    }

}