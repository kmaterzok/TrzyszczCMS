using TrzyszczCMS.Core.Infrastructure.Enums;

namespace TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces
{
    /// <summary>
    /// Interface for factory returning a proper database strategy for connection with a database.
    /// </summary>
    public interface IDatabaseStrategyFactory
    {
        /// <summary>
        /// Return reference on the instance of the strategy that handles the desired database.
        /// </summary>
        /// <param name="dbType">Desired database to handle</param>
        /// <returns>Desired database strategy</returns>
        IDatabaseStrategy GetStrategy(ConnectionStringDbType dbType);
    }
}
