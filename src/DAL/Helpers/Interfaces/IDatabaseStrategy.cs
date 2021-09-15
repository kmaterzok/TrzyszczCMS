using System.Data.Common;

namespace DAL.Helpers.Interfaces
{
    /// <summary>
    /// An interface for basic database operations and handling.
    /// </summary>
    public interface IDatabaseStrategy
    {
        /// <summary>
        /// Get an instance of <see cref="DbConnection"/> object for executing queries in the DB server.
        /// </summary>
        /// <returns>DB connection object instance fopr connection with a concrete database.</returns>
        DbConnection GetDbConnection();
    }
}
