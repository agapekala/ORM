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
using orm.Criterias;

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
            object primarKey = _propertiesMapper.findPrimaryKey(obj); //TO DO: exception that henadles when trying to add id that already exists
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


        public IEnumerable select(Type type, List<Criteria> listOfCriteria) {
            List<object> result = new List<object>();

            QueryBuilder queryBuilder = new QueryBuilder();
            object obj = Activator.CreateInstance(type);

            string tableName = _propertiesMapper.getTableName(obj);
            string primaryKeyName = _propertiesMapper.findPrimaryKeyFieldName(obj);

            String query = queryBuilder.createSelectQuery(tableName, listOfCriteria);
            Console.WriteLine(query);


            _connection.ConnectAndOpen();
            SqlCommand command = _connection.execute(query);
            //command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();

            // Getting all elements that fit the criteria and putting their primary keys into listOfIds.
            LinkedList<object> listOfIds = new LinkedList<object>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).ToString() == primaryKeyName) {
                        listOfIds.AddLast(reader[i]);
                    }
                    string formatString = "{0}";
                    Console.Write(String.Format(formatString, reader[i]));
                    Console.WriteLine(String.Format(formatString, reader.GetName(i)));
                }
            }
            reader.Close();
            _connection.Dispose();


            // Triggering selectById for every single object that is present in listOfIds.
            foreach (object id in listOfIds) {
                result.Add(selectById(type, id));
            }

            return result; 
        }


        // Selects single object from database basing on its id (primary key).
        public object selectById(Type type, object id){
            QueryBuilder queryBuilder = new QueryBuilder();
            object obj = Activator.CreateInstance(type);

            string tableName = _propertiesMapper.getTableName(obj);
            string primaryKeyName = _propertiesMapper.findPrimaryKeyFieldName(obj);

            String query = queryBuilder.createSelectByIdQuery(tableName, id, primaryKeyName);
            Console.WriteLine(query);

            List<IRelationship> oneToOneRelationshipsList = _relationshipsMapper.findOneToOneRelationships(obj);
            List<IRelationship> oneToManyRelationshipsList = _relationshipsMapper.findOneToManyRelationships(obj);
            
            if (oneToOneRelationshipsList.Count == 0 && oneToManyRelationshipsList.Count == 0)
            {

                // Wywołanie pojedyncznego selecta i podpisanie wszystkich typów generycznych.
                // TO-DO: Przerzucić connection.
                _connection.ConnectAndOpen();
                SqlCommand command = _connection.execute(query);
                //command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                var mappedObject = _propertiesMapper.mapTableIntoObject(obj, reader);
                _connection.Dispose();
            }
            else{
                // Iteracja po wszystkich polach 

                PropertyInfo[] props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (PropertyInfo prp in props)
                {
                    // OneToOne
                    MethodInfo strGetter = prp.GetGetMethod(nonPublic: true); //get id()  (column)
                    object[] attOneToOne = prp.GetCustomAttributes(typeof(OneToOneAttribute), false);
                    var val = strGetter.Invoke(obj, null);
                    if (attOneToOne.Length == 0)
                    {
                        continue;
                    }
                    _connection.ConnectAndOpen();
                    SqlCommand commandTmp = _connection.execute(query);
                    SqlDataReader readerTmp = commandTmp.ExecuteReader();
                    
                    object foreignKeyId = _propertiesMapper.getValueOfForeignKey(prp, readerTmp); // readerTmp is closed inside of this function.
                    _connection.Dispose();
                    // Handles case, when object doesn't exists in database.
                    if (foreignKeyId == null) {
                        return null;
                    }
                    // Handles case, when object is a null in database.
                    if (foreignKeyId == DBNull.Value) {
                        obj = _propertiesMapper.setCertainField(obj, null, prp);
                        continue;
                    }

                    object mappedObject = selectById(prp.PropertyType, foreignKeyId);
                    obj = _propertiesMapper.setCertainField(obj, mappedObject, prp);

                }
                _connection.ConnectAndOpen();
                SqlCommand command = _connection.execute(query);
                //command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                obj = _propertiesMapper.mapTableIntoObject(obj, reader); // Reader is closed inside of this function.
                _connection.Dispose();
            }

            // Executing single select query. 
            return obj;
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
