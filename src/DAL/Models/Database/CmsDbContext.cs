using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DAL.Models.Database
{
    public partial class CmsDbContext : DbContext
    {
        public CmsDbContext()
        {
        }

        public CmsDbContext(DbContextOptions<CmsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuthPolicy> AuthPolicies { get; set; }
        public virtual DbSet<AuthRole> AuthRoles { get; set; }
        public virtual DbSet<AuthRolePolicyAssign> AuthRolePolicyAssigns { get; set; }
        public virtual DbSet<AuthToken> AuthTokens { get; set; }
        public virtual DbSet<AuthUser> AuthUsers { get; set; }
        public virtual DbSet<ContFile> ContFiles { get; set; }
        public virtual DbSet<ContModule> ContModules { get; set; }
        public virtual DbSet<ContPage> ContPages { get; set; }
        public virtual DbSet<ContTextWallModule> ContTextWallModules { get; set; }
        public virtual DbSet<VersionInfo> VersionInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_Poland.1250");

            modelBuilder.Entity<AuthPolicy>(entity =>
            {
                entity.ToTable("AuthPolicy");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AuthRole>(entity =>
            {
                entity.ToTable("AuthRole");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AuthRolePolicyAssign>(entity =>
            {
                entity.HasKey(e => new { e.AuthRoleId, e.AuthPolicyId });

                entity.ToTable("AuthRolePolicyAssign");

                entity.HasOne(d => d.AuthPolicy)
                    .WithMany(p => p.AuthRolePolicyAssigns)
                    .HasForeignKey(d => d.AuthPolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AuthRolePolicyAssign_AuthPolicy_AssignedPolicyId");

                entity.HasOne(d => d.AuthRole)
                    .WithMany(p => p.AuthRolePolicyAssigns)
                    .HasForeignKey(d => d.AuthRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AuthRolePolicyAssign_AuthRole_AssignedRoleId");
            });

            modelBuilder.Entity<AuthToken>(entity =>
            {
                entity.ToTable("AuthToken");

                entity.Property(e => e.HashedToken).IsRequired();

                entity.HasOne(d => d.AuthUser)
                    .WithMany(p => p.AuthTokens)
                    .HasForeignKey(d => d.AuthUserId)
                    .HasConstraintName("AuthToken_AuthUser_AssignedUserId");
            });

            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.ToTable("AuthUser");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.HasOne(d => d.AuthRole)
                    .WithMany(p => p.AuthUsers)
                    .HasForeignKey(d => d.AuthRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AuthUser_AuthRole_AssignedRoleId");
            });

            modelBuilder.Entity<ContFile>(entity =>
            {
                entity.ToTable("ContFile");

                entity.Property(e => e.MimeType).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.ParentFile)
                    .WithMany(p => p.InverseParentFile)
                    .HasForeignKey(d => d.ParentFileId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ContFile_ContFile_ParentFileId");
            });

            modelBuilder.Entity<ContModule>(entity =>
            {
                entity.ToTable("ContModule");

                entity.HasOne(d => d.ContPage)
                    .WithMany(p => p.ContModules)
                    .HasForeignKey(d => d.ContPageId)
                    .HasConstraintName("ContModule_Page_AssignedId");
            });

            modelBuilder.Entity<ContPage>(entity =>
            {
                entity.ToTable("ContPage");

                entity.HasIndex(e => e.Title, "IX_ContPage_Title")
                    .IsUnique();

                entity.HasIndex(e => e.UriName, "IX_ContPage_UriName")
                    .IsUnique();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Type).HasDefaultValueSql("3");

                entity.Property(e => e.UriName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ContTextWallModule>(entity =>
            {
                entity.ToTable("ContTextWallModule");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.SectionWidth).HasDefaultValueSql("800");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ContTextWallModule)
                    .HasForeignKey<ContTextWallModule>(d => d.Id)
                    .HasConstraintName("ContTextWallModuleId_ContModuleId_AssignedModuleId");
            });

            modelBuilder.Entity<VersionInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("VersionInfo");

                entity.HasIndex(e => e.Version, "UC_Version")
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1024);
            });
        }
    }
}
