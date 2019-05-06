using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
namespace GokkersApp
{
    public class ServerConnection
    {
        public MySqlConnection connection;
        public string serverName;
        public string databaseName;
        public string userName;
        public string userPass;
        public string portNum;
        public string connectionString;
        public string sslM;
        public string activeUserName;

        public void svLogin(string svName, string dbName, string userID, string pass, string port, string ssl)
        {
            serverName = svName;
            databaseName = dbName;
            userName = userID;
            userPass = pass;
            portNum = port;
            sslM = ssl;
            connectionString = string.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}",
            serverName, port, userName, userPass, databaseName, sslM);
            connection = new MySqlConnection(connectionString);
        }
    }
}
