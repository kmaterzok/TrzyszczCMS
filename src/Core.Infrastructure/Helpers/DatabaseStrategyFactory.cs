using TrzyszczCMS.Core.Infrastructure.Enums;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using TrzyszczCMS.Core.Infrastructure.Models.Config;
using Microsoft.Extensions.Options;

namespace TrzyszczCMS.Core.Infrastructure.Helpers
{
    public class DatabaseStrategyFactory : IDatabaseStrategyFactory
    {
        #region Fields
        private readonly ConnectionStrings _connectionStrings;
        #endregion

        #region Ctor
        public DatabaseStrategyFactory(IOptions<ConnectionStrings> connectionStrings) =>
            this._connectionStrings = connectionStrings.Value;
        #endregion

        #region Methods
        public IDatabaseStrategy GetStrategy(ConnectionStringDbType dbType) =>
            new PgsqlDatabaseStrategy(this._connectionStrings.GetConnectionString(dbType));
        #endregion
    }
}
