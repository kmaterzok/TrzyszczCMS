using DAL.Models.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.Database
{
    public class CmsDbContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth_Policy>()
                .HasKey(c => c.Id);
            
            
            modelBuilder.Entity<Auth_Role>()
                .HasKey(c => c.Id);


            modelBuilder.Entity<Auth_Role_Policy_Assign>()
                .HasKey(c => new { c.Auth_PolicyId, c.Auth_RoleId });
            modelBuilder.Entity<Auth_Role_Policy_Assign>()
                .HasOne(c => c.Auth_Role)
                .WithMany(c => c.Auth_Role_Policy_Assigns)
                .HasForeignKey(c => c.Auth_RoleId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Auth_Role_Policy_Assign>()
                .HasOne(c => c.Auth_Policy)
                .WithMany(c => c.Auth_Role_Policy_Assigns)
                .HasForeignKey(c => c.Auth_PolicyId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Auth_Token>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Auth_Token>()
                .HasOne(c => c.Auth_User)
                .WithMany(c => c.AuthTokens)
                .HasForeignKey(c => c.Auth_UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Auth_User>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Auth_User>()
                .HasOne(c => c.AuthRole)
                .WithMany(c => c.Auth_Users)
                .HasForeignKey(c => c.Auth_RoleId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Cont_Module>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cont_Module>()
                .HasOne(c => c.ContPage)
                .WithMany(c => c.ContModules)
                .HasForeignKey(c => c.Cont_PageId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Cont_Page>()
                .HasKey(c => c.Id);
            
            
            modelBuilder.Entity<Cont_TextWallModule>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Cont_TextWallModule>()
                .HasOne(c => c.ContModule)
                .WithOne(c => c.ContTextWallModule)
                .HasForeignKey<Cont_TextWallModule>(c => c.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
        #endregion

        #region --- Private methods ---

        #endregion

        #region --- DbSets ---
        public DbSet<Auth_Policy> Auth_Policy { get; set; }
        public DbSet<Auth_Role> Auth_Role { get; set; }
        public DbSet<Auth_Role_Policy_Assign> Auth_Role_Policy_Assign { get; set; }
        public DbSet<Auth_Token> Auth_Token { get; set; }
        public DbSet<Auth_User> Auth_User { get; set; }


        public DbSet<Cont_Module> Cont_Module { get; set; }
        public DbSet<Cont_Page> Cont_Page { get; set; }
        public DbSet<Cont_TextWallModule> Cont_TextWallModule { get; set; }
        #endregion
    }
}
