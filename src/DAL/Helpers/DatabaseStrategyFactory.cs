using DAL.Enums;
using DAL.Helpers.Interfaces;
using DAL.Models.Config;
using Microsoft.Extensions.Options;

namespace DAL.Helpers
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
