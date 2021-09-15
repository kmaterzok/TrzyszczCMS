using Npgsql;
using System.Data.Common;

namespace DAL.Helpers
{
    /// <summary>
    /// This is a helper allowing to establish a connection with PostgreSQL server and a database.
    /// </summary>
    internal static class DALHelper
    {
        #region Public methods
        /// <summary>
        /// Create a connection object for <see cref="database"/> on a PostgreSQL server.
        /// </summary>
        /// <param name="connectionString">Connection string defining basic connection credentials.</param>
        /// <returns>Connection object for the desireed database</returns>
        public static DbConnection CreatePgsqlDbConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
        #endregion
    }
}
