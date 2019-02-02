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

        public Manager(MSSqlConnection connection)
        {
            _connection=connection;
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
                string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
                _queries.Add(insertQuery);
                // Since there are no other relationships, we can execute command here.
            }
            else { 
                handleOneToOneRelationships(obj);
            }

            foreach(string q in _queries) { 
                Console.WriteLine(q);
            }

            // _connection.ConnectAndOpen();
            //SqlCommand command = _connection.execute(insertQuery);
            //command.ExecuteNonQuery();
            //_connection.Dispose();

        }

        public void handleOneToOneRelationships(object obj) {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            foreach (OneToOneRelationship rel in oneToOneRelationshipsList){
                    handleOneToOneRelationships(rel.getOwned());
            }
            
            string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
            _queries.Add(insertQuery);
            
        }
    }
}
