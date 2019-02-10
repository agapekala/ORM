using System;
using System.Collections.Generic;
using System.Text;
using orm.Relationships;

namespace orm.Query
{
    class QueryBuilder
    {

        public QueryBuilder() { }
        public string createInsertQuery(string tableName, List<Tuple<string, object>> columns)
        {

            string returnQuery = "INSERT INTO " + tableName + "(";
            foreach (Tuple<string, object> it in columns)
            {
                returnQuery += it.Item1 + ", ";
            }
            returnQuery = returnQuery.Remove(returnQuery.Length - 2);

            returnQuery += ") VALUES (";

            foreach (Tuple<string, object> it in columns)
            {
                if (it.Item2.GetType() == typeof(string))
                {
                    if (it.Item2.Equals("null"))
                    {
                        returnQuery += "null, ";
                    }else
                    returnQuery += "'" + it.Item2 + "'" + ", ";
                }
                else
                {
                   
                    returnQuery += it.Item2 + ", ";
                }
            }
            returnQuery = returnQuery.Remove(returnQuery.Length - 2);
            returnQuery += ");";

            return returnQuery;
        }
        public static readonly Dictionary<Type, string> CsTypesToSql = new Dictionary<Type, string>()
        {
            {typeof(System.Int64),"bigint"},
            {typeof(System.Byte[]),"binary"},
            {typeof(System.Boolean),"bit"},
            {typeof(System.String),"varchar(255)" },
            {typeof(System.Char[]),"varchar(255)" },
            {typeof(System.DateTime),"date" },
            {typeof(System.Decimal),"decimal" },
            {typeof(System.Double),"float" },
            {typeof(System.Int32),"int" },
        };


        public string createCreateTableQuery(string tableName, List<Tuple<string, object>> columns)
        {
            string returnQuery = "DROP TABLE IF EXISTS " + tableName + ";";
            returnQuery += "CREATE TABLE " + tableName + "(";
            Boolean primaryKey = true;
            foreach (Tuple<string, object> it in columns)
            {
                returnQuery += it.Item1 + " ";
                returnQuery += CsTypesToSql[it.Item2.GetType()] ;
                if (primaryKey)
                {
                    returnQuery += " PRIMARY KEY,   ";
                    primaryKey = false;
                }
                else
                {
                    returnQuery +=  ", ";
                }
            }

            returnQuery = returnQuery.Remove(returnQuery.Length - 2);
            returnQuery += ");";

            return returnQuery;
        }
        public string createUpdateQuery(string tableName, List<Tuple<string, object>> columns, List<Tuple<string, object>> conditions)
        {
            string returnQuery = "UPDATE " + tableName + " SET ";
            foreach (Tuple<string, object> it in conditions)
            {
                returnQuery += it.Item1 + "=";
                if (it.Item2.GetType() == typeof(string))
                {
                    returnQuery += "'" + it.Item2 + "'" + ", ";
                }
                else
                {
                    returnQuery += it.Item2 + ", ";
                }
            }
            returnQuery = returnQuery.Remove(returnQuery.Length - 2);
            returnQuery += " WHERE ";
            Tuple<string, object> first = columns[0];
            returnQuery += first.Item1 + "=";
            returnQuery += first.Item2 + ";";
            return returnQuery;
        }
        
        public string createDeleteQuery(string tableName, List<Tuple<string, object>> columns)
        {
            string returnQuery = "DELETE FROM " + tableName ;

            returnQuery += " WHERE ";
            Tuple<string, object> first = columns[0];
            returnQuery += first.Item1 + "=";
            returnQuery += first.Item2 + ";";
            return returnQuery;
        }

    }
}
