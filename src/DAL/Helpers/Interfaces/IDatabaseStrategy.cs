using System.Data;
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
        /// <param name="openConnection">Open connection just before returning the object reference</param>
        /// <returns>DB connection object instance fopr connection with a concrete database.</returns>
        IDbConnection GetDbConnection(bool openConnection = true);
    }
}
