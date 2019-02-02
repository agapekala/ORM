using System;
using System.Collections.Generic;
using System.Text;

namespace orm.Query
{
    class QueryBuilder
    {

        public QueryBuilder() { }

        public string createInsertQuery(string tableName, List<Tuple<string, object>> columns)
        {

            string returnQuery = "INSERT INTO " + tableName + "(";
            foreach (Tuple<string,object> it in columns)
            {
                returnQuery +=it.Item1+", ";
            }
            returnQuery = returnQuery.Remove(returnQuery.Length-2);

            returnQuery += ") VALUES (";

            foreach (Tuple<string, object> it in columns)
            {
                if (it.Item2.GetType() == typeof(string))
                {
                    returnQuery +="'" + it.Item2 +"'" + ", ";
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

    }
}
