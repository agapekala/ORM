using System;
using System.Collections.Generic;
using System.Text;
using orm.Connection;
using orm.Attributes;
using orm.Mapper;
using System.Data.SqlClient;
using System.Reflection;
using orm.Query;
using orm.Relationships;

namespace orm
{
    class Manager
    {
        private MSSqlConnection _connection;
        private PropertiesMapper _propertiesMapper;
        private RelationshipsMapper _relationshipsMapper;

        private List<string> _queries;
        private List<Tuple<string, object>> _conditions;


        public Manager(MSSqlConnection connection)
        {
            _connection = connection;
            _propertiesMapper = new PropertiesMapper();
            _relationshipsMapper = new RelationshipsMapper();
            _queries = new List<string>();
        }

        public void insert(Object obj)
        {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            if (oneToOneRelationshipsList.Count == 0)
            {
                string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
                string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
                _queries.Add(createTableQuery);
                _queries.Add(insertQuery);

                // Since there are no other relationships, we can execute command here.

            }
            else
            {
                handleOneToOneRelationships(obj);
            }
            _connection.ConnectAndOpen();
            foreach (string q in _queries)
            {
                SqlCommand command = _connection.execute(q);
                command.ExecuteNonQuery();
                Console.WriteLine(q);
            }
            // _connection.ConnectAndOpen();
            //SqlCommand command = _connection.execute(insertQuery);
            //command.ExecuteNonQuery();
            _connection.Dispose();

        }

        public void handleOneToOneRelationships(object obj)
        {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            if (oneToOneRelationshipsList.Count != 0)
            {
                foreach (OneToOneRelationship rel in oneToOneRelationshipsList)
                {
                    Console.WriteLine("rel    " + rel.getOwned());
                    if (rel.getOwned() == null)
                    {
                        Console.WriteLine("nnnnnnnnnnnnnn" + columnsAndValuesList);
                    }
                    else
                        handleOneToOneRelationships(rel.getOwned());
                }
            }
            else
            {

            }

            string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
            _queries.Add(createTableQuery);

            string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
            _queries.Add(insertQuery);

            // string updateQuery = query.createUpdateQuery(tableName, columnsAndValuesList, _conditions);
            // _queries.Add(updateQuery);



        }
        public void update(Object obj, List<Tuple<string, object>> conditions)
        {
            _conditions = conditions;
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            if (oneToOneRelationshipsList.Count == 0)
            {
                string updateQuery = query.createUpdateQuery(tableName, columnsAndValuesList, conditions);
                _queries.Add(updateQuery);
                // Since there are no other relationships, we can execute command here.

            }
            else
            {
                handleOneToOneRelationships(obj);
            }
            _connection.ConnectAndOpen();
            foreach (string q in _queries)
            {
                Console.WriteLine(q);
                SqlCommand command = _connection.execute(q);
                command.ExecuteNonQuery();

            }
            // _connection.ConnectAndOpen();
            //SqlCommand command = _connection.execute(insertQuery);
            //command.ExecuteNonQuery();
            _connection.Dispose();
        }

    }
}
