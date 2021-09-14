using Npgsql;
using System.Data.Common;

namespace DAL.Helpers
{
    /// <summary>
    /// This is a helper allowing to establish a connection with PostgreSQL server and a database.
    /// </summary>
    internal static class DALHelper
    {
        #region Constants
        /// <summary>
        /// The standard connection string used for establishing a connection with the PostgreSQL database.
        /// </summary>
        public const string PGSQL_CONNECTION_STRING_TEMPLATE =
            "Host={0};Username={1};Password={2};Database={3}";
        #endregion

        #region Public methods
        /// <summary>
        /// Create a connection object for <see cref="database"/> on a PostgreSQL server.
        /// </summary>
        /// <param name="host">IP address of the connected server</param>
        /// <param name="username">Login for the user connection to the database</param>
        /// <param name="password">Connecting user's password</param>
        /// <param name="database">Connected database name</param>
        /// <returns>Connection object for the desireed database</returns>
        public static DbConnection CreatePgsqlDbConnection(string host, string username, string password, string database)
        {
            return new NpgsqlConnection(
                string.Format(PGSQL_CONNECTION_STRING_TEMPLATE, host, username, password, database)
            );
        }
        #endregion
    }
}
