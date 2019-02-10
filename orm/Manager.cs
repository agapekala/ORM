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
            object primarKey=columnsAndValuesList[0].Item2; //TO DO: exception that henadles when trying to add id that already exists
            Console.WriteLine(("pk:   "+primarKey));
            
            
            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            if (oneToOneRelationshipsList.Count == 0)
            {
//                    string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
                    string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
//                    _queries.Add(createTableQuery);
                    _queries.Add(insertQuery);             
            }
            else
            {
                handleOneToOneRelationships(obj);
//                string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
                string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
//                _queries.Add(createTableQuery);
                _queries.Add(insertQuery);   

            }
            _connection.ConnectAndOpen();
            foreach (string q in _queries)
            {
                Console.WriteLine(q);
                SqlCommand command = _connection.execute(q);
                command.ExecuteNonQuery();
                
            }
            _connection.Dispose();
            _queries.Clear();

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
                    if (rel.getOwned() != null)
                    {
                       handleOneToOneRelationships(rel.getOwned());
                    }
                }
            }
//            string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
//            string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
////            _queries.Add(createTableQuery);
//            _queries.Add(insertQuery);     

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
                string updateQuery = query.createUpdateQuery(tableName, columnsAndValuesList, conditions);
                _queries.Add(updateQuery);
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
        
        public void delete(Object obj, List<Tuple<string, object>> conditions)
        {
            _conditions = conditions;
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            if (oneToOneRelationshipsList.Count == 0)
            {
                string deleteQuery = query.createDeleteQuery(tableName, columnsAndValuesList);
                _queries.Add(deleteQuery);
                // Since there are no other relationships, we can execute command here.

            }
            else
            {
                handleOneToOneRelationships(obj);
                string deleteQuery = query.createDeleteQuery(tableName, columnsAndValuesList);
                _queries.Add(deleteQuery);
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
