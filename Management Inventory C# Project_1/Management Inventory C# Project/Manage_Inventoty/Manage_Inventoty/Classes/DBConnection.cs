using System.Data.SqlClient;

namespace Manage_Inventoty.Classes
{
    public static class DBConnection
    {
        // Your database connection string
        private static readonly string connectionString = @"Server=.\SQLEXPRESS02;Database=StockControler;UID=sa;PWD=sa123456";

        /// <summary>
        /// Provides a new SqlConnection object using the centralized connection string.
        /// </summary>
        /// <returns>A new SqlConnection instance.</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}