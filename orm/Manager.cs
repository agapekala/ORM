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
using System.Linq;

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
            List<IRelationship> oneToManyRelationshipsList = _relationshipsMapper.findOneToManyRelationships(obj);
           
            QueryBuilder query = new QueryBuilder();
            if (oneToOneRelationshipsList.Count == 0 && oneToManyRelationshipsList.Count == 0)
            {
                string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
                _queries.Add(insertQuery);
                // Since there are no other relationships, we can execute command here.
            }
            else {
                handleOneToManyRelationships(obj, null);
            }
            


            foreach(string q in _queries) { 
                Console.WriteLine(q);
            }

            // _connection.ConnectAndOpen();
            //SqlCommand command = _connection.execute(insertQuery);
            //command.ExecuteNonQuery();
            //_connection.Dispose();

        }
/*        
        public void handleOneToOneRelationships(object obj) {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);
            List<IRelationship> oneToManyRelationshipsList = _relationshipsMapper.findOneToManyRelationships(obj);
 

            QueryBuilder query = new QueryBuilder();
            foreach (OneToOneRelationship rel in oneToOneRelationshipsList){
                handleOneToOneRelationships(rel.getOwned());
            }
            
            foreach (OneToManyRelationship rel in oneToManyRelationshipsList)
            {
                LinkedList<object> listOfObjects = (LinkedList<object>)rel.getOwned();
                foreach (object child in listOfObjects)
                {
                    handleOneToOneRelationships(child);
                    handleOneToManyRelationships(child, obj);
                }
            }

            string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
            _queries.Add(insertQuery);
        }
*/
        public void handleOneToManyRelationships(object obj, object parent)
        {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            // This part is responsible for adding foreign key in child object. (Cat receives person's ID).
            if (parent != null) { 
                string foreignColumn = _propertiesMapper.getTableName(parent); // Gets name of the column, which contains parent's ID.
                foreignColumn += "Id";
                object parentId = _propertiesMapper.findPrimaryKey(parent); // Gets foreign key.
                columnsAndValuesList.Add(new Tuple<string, object>(foreignColumn, parentId));
            }
            
            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);
            List<IRelationship> oneToManyRelationshipsList = _relationshipsMapper.findOneToManyRelationships(obj);


            QueryBuilder query = new QueryBuilder();
            foreach (OneToOneRelationship rel in oneToOneRelationshipsList)
            {
                handleOneToManyRelationships(rel.getOwned(), null);
            }
            foreach (OneToManyRelationship rel in oneToManyRelationshipsList)
            {
                Console.WriteLine("Rel: "+rel.getOwned().ToString());

                Type listType = rel.getOwnedType();
                IEnumerable <object> listOfObjects = (LinkedList<object>)rel.getOwned();
                //listOfObjects = Convert.ChangeType(listOfObjects, rel.getOwned().GetType());

                

                foreach (object child in listOfObjects) {
//                    handleOneToOneRelationships(child, null);

                    handleOneToManyRelationships(child, obj);
                }
            }

            string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
            _queries.Add(insertQuery);

        }



    }
}
