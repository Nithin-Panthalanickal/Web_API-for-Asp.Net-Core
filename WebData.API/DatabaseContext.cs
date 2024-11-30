using MySql.Data.MySqlClient;
using System.Data;

namespace WebData.API
{
    public class DatabaseContext
    {
        private readonly string _connectionString;
        public DatabaseContext(string connectionString)
        {

            _connectionString = connectionString;
        }
        public MySqlConnection GetConnection() {

            return new MySqlConnection(_connectionString);
        
        }
    }
}
