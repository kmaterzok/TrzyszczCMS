using DAL.Helpers.Interfaces;
using System;
using System.Data;
using System.Data.Common;

namespace DAL.Helpers
{
    /// <summary>
    /// The strategy defining a basic stuff for executing queries in a PostgreSQL server.
    /// </summary>
    public class PgsqlDatabaseStrategy : IDatabaseStrategy
    {
        #region Fields
        /// <summary>
        /// A factory methods for generating <see cref="DbConnection"/> objects.
        /// </summary>
        private readonly Func<DbConnection> _dbConnectionFactoryMethod;
        #endregion

        #region Ctor
        /// <summary>
        /// A standard constructor for handling databases in a PostgreSQL server.
        /// </summary>
        /// <param name="connectionString">Connection string for database access.</param>
        public PgsqlDatabaseStrategy(string connectionString)
        {
            this._dbConnectionFactoryMethod = () => DALHelper.CreatePgsqlDbConnection(connectionString);
        }
        #endregion

        #region Public methods
        public IDbConnection GetDbConnection()
        {
            return this._dbConnectionFactoryMethod.Invoke();
        }
        #endregion
    }
}
