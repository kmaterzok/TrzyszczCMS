using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using TrzyszczCMS.Core.Infrastructure.Models.Database;
using System;

namespace TrzyszczCMS.Core.Infrastructure.Helpers
{
    /// <summary>
    /// The strategy defining a basic stuff for executing queries in a PostgreSQL server.
    /// </summary>
    public class PgsqlDatabaseStrategy : IDatabaseStrategy
    {
        #region Fields
        /// <summary>
        /// A factory methods for generating <see cref="CmsDbContext"/> objects.
        /// </summary>
        private readonly Func<CmsDbContext> _dbContextFactoryMethod;
        #endregion

        #region Ctor
        /// <summary>
        /// A standard constructor for handling databases in a PostgreSQL server.
        /// </summary>
        /// <param name="connectionString">Connection string for database access.</param>
        public PgsqlDatabaseStrategy(string connectionString) =>
            this._dbContextFactoryMethod = () => new CmsDbContext(connectionString);
        #endregion

        #region Public methods
        public CmsDbContext GetContext() =>
            this._dbContextFactoryMethod.Invoke();
        #endregion
    }
}
