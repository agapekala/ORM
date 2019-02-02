using System;
using System.Collections.Generic;
using System.Text;
using orm.Connection;
using orm.Attributes;
using orm.Mapper;
using System.Data.SqlClient;
using System.Reflection;
using orm.Query;

namespace orm
{
    class Manager
    {
        private MSSqlConnection _connection;


        public Manager(MSSqlConnection connection)
        {
            _connection=connection;
        }

        public void insert(Object obj)
        {
            PropertiesMapper mapper = new PropertiesMapper();
            string tableName=mapper.getTableName(obj);
            List<string> ColumnList = mapper.getColumnName(obj);
            List<Tuple<string, object>> list = mapper.getColumnAndValue(obj);

            QueryBuilder query = new QueryBuilder();
            string insertQuery=query.createInsertQuery(tableName, list);
            Console.WriteLine(insertQuery);

           // _connection.ConnectAndOpen();
            //SqlCommand command = _connection.execute(insertQuery);
            //command.ExecuteNonQuery();
            //_connection.Dispose();

        }
    }
}
