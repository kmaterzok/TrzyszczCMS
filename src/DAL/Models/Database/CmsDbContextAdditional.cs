using Microsoft.EntityFrameworkCore;

namespace DAL.Models.Database
{
    public partial class CmsDbContext : DbContext
    {
        #region --- Fields ---
        /// <summary>
        /// The connection string used for connecting with a specific database.
        /// </summary>
        private readonly string _connectionString;
        #endregion

        #region --- Ctor ---
        public CmsDbContext(string connectionString)
        {
            this._connectionString = connectionString;
        }
        #endregion

        #region --- Protected methods ---
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(this._connectionString);
        }

        #endregion
    }
}
