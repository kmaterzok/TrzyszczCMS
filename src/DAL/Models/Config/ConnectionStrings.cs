using DAL.Enums;
using System;

namespace DAL.Models.Config
{
    /// <summary>
    /// COnnection string for database instances used for data handling.
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// Connection string for database that allows only read operations like SELECT.
        /// </summary>
        public string ReadDbSqlConnection   { get; set; }
        /// <summary>
        /// Connection string for database that allows to execute all operations.
        /// </summary>
        public string ModifyDbSqlConnection { get; set; }

        /// <summary>
        /// Return the connection string depending on the desired type.
        /// </summary>
        /// <param name="dbType">Desired database to handle</param>
        /// <returns>Desired connection string</returns>
        public string GetConnectionString(ConnectionStringDbType dbType) => dbType switch
        {
            ConnectionStringDbType.Modify => ModifyDbSqlConnection,
            ConnectionStringDbType.Read   => ReadDbSqlConnection,

            _ => throw new ArgumentException($"The type of {dbType} is not supported within this method.")
        };
        
    }
}
