using TrzyszczCMS.Core.Infrastructure.Models.Database;

namespace TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces
{
    /// <summary>
    /// An interface for basic database operations and handling.
    /// </summary>
    public interface IDatabaseStrategy
    {
        /// <summary>
        /// Get an instance of <see cref="CmsDbContext"/> used for connection with database and commiting EF Core based queries.
        /// </summary>
        /// <returns>DFatabase context</returns>
        CmsDbContext GetContext();
    }
}
