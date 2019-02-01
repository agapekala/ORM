using System;
using System.Collections.Generic;
using System.Text;
using orm.Connection;
using System.Data.SqlClient;

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
            //_connection.ConnectAndOpen();
            //SqlCommand command = _connection.execute("INSERT INTO Users VALUES (3, 'Maria')");
            //command.ExecuteNonQuery();
            //_connection.Dispose();

        }
    }
}
