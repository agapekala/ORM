using System;
using System.Data.SqlClient;

namespace orm.Configuration
{
    public class ConnConfiguration
    {
        private string _serverName;
        private string _databaseName;
        private string _userId;
        private string _password;


        public string GetServerName()
        {
            return _serverName;
        }

        public string GetDatabaseName()
        {
            return _databaseName;
        }


        public ConnConfiguration(String server, String db, String user, String pass)
        {
            _serverName = server;
            _databaseName = db;
            _userId = user;
            _password = pass;
        }

        public ConnConfiguration(String server, String db)
        {
            _serverName = server;
            _databaseName = db;
        }

        public ConnConfiguration ServerName(string serverName)
        {
            this._serverName = serverName;
            return this;
        }

        public ConnConfiguration DatabaseName(string databaseName)
        {
            this._databaseName = databaseName;
            return this;
        }


        private void Build()
        {
            _serverName = GetServerName();
            _databaseName = GetDatabaseName();
        }

        public String creteConnectionString()
        {
            return "Data Source=" + _serverName + ";Initial Catalog=" + _databaseName + ";Integrated Security=True";

        }
        
        /*
        public String creteConnectionString()
        {
            return "Server=" + _serverName + ";Database=" + _databaseName + ";User Id=" +
                   _userId + ";Password=" + _password + ";MultipleActiveResultSets=true;";
        }
        */
    }
}