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
using System.Collections;

namespace orm
{
    class Manager
    {
        private MSSqlConnection _connection;
        private PropertiesMapper _propertiesMapper;
        private RelationshipsMapper _relationshipsMapper;
        private List<string> _queries;
        private List<Tuple<string, object>> _conditions;
        private String operation;
        public Manager(MSSqlConnection connection)
        {
            _connection = connection;
            _propertiesMapper = new PropertiesMapper();
            _relationshipsMapper = new RelationshipsMapper();
            _queries = new List<string>();
        }

        public void insert(Object obj)
        {
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);
            object primarKey = columnsAndValuesList[0].Item2; //TO DO: exception that henadles when trying to add id that already exists
            Console.WriteLine(("pk:   " + primarKey));


            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);
            List<IRelationship> oneToManyRelationshipsList = _relationshipsMapper.findOneToManyRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            if (oneToOneRelationshipsList.Count == 0 && oneToManyRelationshipsList.Count == 0)
            {
                string tableName = _propertiesMapper.getTableName(obj);
                List<string> ColumnList = _propertiesMapper.getColumnName(obj);
                //                    string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
                string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
                //                    _queries.Add(createTableQuery);
                _queries.Add(insertQuery);
            }
            else
            {
                //                string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
                //                string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
                //                _queries.Add(createTableQuery);
                //                _queries.Add(insertQuery); 
                operation = "insert";
                handleOneToManyRelationships(obj, null);
                //    

            }
            //_connection.ConnectAndOpen();
            foreach (string q in _queries)
            {
                Console.WriteLine(q);
               // SqlCommand command = _connection.execute(q);
               // command.ExecuteNonQuery();

            }
            //_connection.Dispose();
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
            //            _queries.Add(createTableQuery);
            switch (operation)
            {
                case "insert":
                    string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
                    _queries.Add(createTableQuery);
                    string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
                    _queries.Add(insertQuery);
                    break;
                case "create":
                    break;
                case "update":
                    string updateQuery = query.createUpdateQuery(tableName, columnsAndValuesList, _conditions);
                    _queries.Add(updateQuery);
                    break;
                case "delete":
                    string deleteQuery = query.createDeleteQuery(tableName, columnsAndValuesList);
                    _queries.Add(deleteQuery);
                    break;

            }
        }

        public object select(Type type, int id)
        {
            object obj = Activator.CreateInstance(type);
            QueryBuilder queryBuilder = new QueryBuilder();
            object tableName = _propertiesMapper.getTableName(obj);
            //String query = queryBuilder.createSelectQuery(tableName, id);
            return null;
        }

        public void update(Object obj, List<Tuple<string, object>> conditions)
        {
            _conditions = conditions;

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            //            if (oneToOneRelationshipsList.Count == 0)
            //            {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            string updateQuery = query.createUpdateQuery(tableName, columnsAndValuesList, conditions);
            _queries.Add(updateQuery);
            //            }
            //            else
            //            {
            //                operation = "update";
            //                handleOneToOneRelationships(obj);
            //
            //            }
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

        public void delete(Object obj)
        {

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);

            QueryBuilder query = new QueryBuilder();
            //            if (oneToOneRelationshipsList.Count == 0)
            //            {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);
            string deleteQuery = query.createDeleteQuery(tableName, columnsAndValuesList);
            _queries.Add(deleteQuery);
            //            }
            //            else
            //            {
            //                operation = "delete";
            //                handleOneToOneRelationships(obj);
            //               }
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



        public void handleOneToManyRelationships(object obj, object parent)
        {
            string tableName = _propertiesMapper.getTableName(obj);
            List<string> ColumnList = _propertiesMapper.getColumnName(obj);
            List<Tuple<string, object>> columnsAndValuesList = _propertiesMapper.getColumnAndValue(obj);

            // This part is responsible for adding foreign key in child object. (Cat receives person's ID).
            if (parent != null)
            {
                // Gets name of the column, which contains parent's ID.
                // string foreignColumn = _propertiesMapper.getTableName(parent); 
                var nameOfBaseClass = parent.GetType().BaseType;
                var nameOfBaseClassWithoutNamespaces = _propertiesMapper.convertObjectNameToString(nameOfBaseClass);
                string foreignColumn = nameOfBaseClassWithoutNamespaces;

                Console.WriteLine("value="+ nameOfBaseClassWithoutNamespaces);
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

                Type listType = rel.getOwnedType();

                IEnumerable e = rel.getOwned() as IEnumerable;

                foreach (object child in e)
                {
                    handleOneToManyRelationships(child, obj);
                }
            }

            switch (operation)
            {
                case "insert":
                    // string createTableQuery = query.createCreateTableQuery(tableName, columnsAndValuesList);
                    // _queries.Add(createTableQuery);
                    string insertQuery = query.createInsertQuery(tableName, columnsAndValuesList);
                    _queries.Add(insertQuery);
                    break;
                case "create":
                    break;
                case "update":
                    string updateQuery = query.createUpdateQuery(tableName, columnsAndValuesList, _conditions);
                    _queries.Add(updateQuery);
                    break;
                case "delete":
                    string deleteQuery = query.createDeleteQuery(tableName, columnsAndValuesList);
                    _queries.Add(deleteQuery);
                    break;
            }

        }

        public static object ConvertList(List<Object> value, Type type)
        {
            var containedType = type.GenericTypeArguments.First();
            return value.Select(item => Convert.ChangeType(item, containedType)).ToList();
        }



    }
}
