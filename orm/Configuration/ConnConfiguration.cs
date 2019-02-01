using System;
using System.Data.SqlClient;

namespace orm.Configuration
{
    public class ConnConfiguration
    {
        private string _serverName;
        private string _databaseName;


        public string GetServerName()
        {
            return _serverName;
        }

        public string GetDatabaseName()
        {
            return _databaseName;
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
    }
}