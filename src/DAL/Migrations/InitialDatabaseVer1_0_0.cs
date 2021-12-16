using DAL.Models.Database;
using FluentMigrator;
using System;

namespace DAL.Migrations
{
    /// <summary>
    /// The migration initialising a database to its very first version.
    /// </summary>
    [Migration(1000000)]
    public class InitialDatabaseVer1_0_0 : Migration
    {
        public override void Up()
        {

            #region --- Auth tables ---
            Create.Table(   nameof(AuthUser))
                .WithColumn(nameof(AuthUser.Id))                .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(AuthUser.Username))          .AsString(40).NotNullable()
                .WithColumn(nameof(AuthUser.Description))       .AsString(250).Nullable().WithDefaultValue(null)
                .WithColumn(nameof(AuthUser.PasswordHash))      .AsBinary(128).NotNullable()
                .WithColumn(nameof(AuthUser.PasswordSalt))      .AsBinary(32).NotNullable()
                .WithColumn(nameof(AuthUser.Argon2Iterations))  .AsInt32().NotNullable()
                .WithColumn(nameof(AuthUser.Argon2MemoryCost))  .AsInt32().NotNullable()
                .WithColumn(nameof(AuthUser.Argon2Parallelism)) .AsInt32().NotNullable()
                .WithColumn(nameof(AuthUser.AuthRoleId))        .AsInt32().NotNullable();

            Create.Table(   nameof(AuthToken))
                .WithColumn(nameof(AuthToken.Id))            .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(AuthToken.AuthUserId))    .AsInt32().NotNullable()
                .WithColumn(nameof(AuthToken.HashedToken))   .AsBinary(128).NotNullable()
                .WithColumn(nameof(AuthToken.UtcCreateTime)) .AsDateTime().NotNullable()
                .WithColumn(nameof(AuthToken.UtcExpiryTime)) .AsDateTime().NotNullable();

            Create.ForeignKey(ForeignKeys.Current.AUTHTOKEN_AUTHUSER_ASSIGNEDUSERID)
                .FromTable(nameof(AuthToken))              .ForeignColumn(nameof(AuthToken.AuthUserId))
                .ToTable(  nameof(AuthUser))               .PrimaryColumn(nameof(AuthUser.Id))
                .OnDelete(System.Data.Rule.Cascade);

            Create.Table(   nameof(AuthPolicy))
                .WithColumn(nameof(AuthPolicy.Id))         .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(AuthPolicy.Name))       .AsString(50).NotNullable();

            Create.Table(   nameof(AuthRole))
                .WithColumn(nameof(AuthRole.Id))           .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(AuthRole.Name))         .AsString(50).NotNullable()
                .WithColumn(nameof(AuthRole.FactoryRole))  .AsBoolean().NotNullable().WithDefaultValue(false);

            Create.Table(   nameof(AuthRolePolicyAssign))
                .WithColumn(nameof(AuthRolePolicyAssign.AuthRoleId))   .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(AuthRolePolicyAssign.AuthPolicyId)) .AsInt32().NotNullable().PrimaryKey();

            Create.ForeignKey(ForeignKeys.Current.AUTHROLEPOLICYASSIGN_AUTHROLE_ASSIGNEDROLEID)
                .FromTable(nameof(AuthRolePolicyAssign)) .ForeignColumn(nameof(AuthRolePolicyAssign.AuthRoleId))
                .ToTable(  nameof(AuthRole))             .PrimaryColumn(nameof(AuthRole.Id));

            Create.ForeignKey(ForeignKeys.Current.AUTHROLEPOLICYASSIGN_AUTHPOLICY_ASSIGNEDPOLICYID)
                .FromTable(nameof(AuthRolePolicyAssign)) .ForeignColumn(nameof(AuthRolePolicyAssign.AuthPolicyId))
                .ToTable(  nameof(AuthPolicy))           .PrimaryColumn(nameof(AuthPolicy.Id));

            Create.ForeignKey(ForeignKeys.Current.AUTHUSER_AUTHROLE_ASSIGNEDROLEID)
                .FromTable(nameof(AuthUser))             .ForeignColumn(nameof(AuthUser.AuthRoleId))
                .ToTable(  nameof(AuthRole))             .PrimaryColumn(nameof(AuthRole.Id));

            
            Insert.IntoTable(nameof(AuthPolicy)).Row(new { Id = 1, Name = "CreateBlogPost" })
                                                .Row(new { Id = 2, Name = "EditBlogPost"   })
                                                .Row(new { Id = 3, Name = "DeleteBlogPost" })
                                                .Row(new { Id = 4, Name = "CreatePage" })
                                                .Row(new { Id = 5, Name = "EditPage"   })
                                                .Row(new { Id = 6, Name = "DeletePage" })
                                                .Row(new { Id = 7, Name = "CreateAnyUser"   })
                                                .Row(new { Id = 8, Name = "EditAnyUser"     })
                                                .Row(new { Id = 9, Name = "DeleteAnyUser"   });
                                                // TODO: Add more policies / permissions.

            Insert.IntoTable(nameof(AuthRole)).Row(new { FactoryRole = true, Name = "Admin" });
            for (int i=1; i <= 9; ++i)
            {
                Insert.IntoTable(nameof(AuthRolePolicyAssign)).Row(new { AuthRoleId = 1, AuthPolicyId = i });
            }

            Insert.IntoTable(nameof(AuthUser)).Row(new
            {
                AuthRoleId  = 1,
                Description  = "Default administrator",
                Username     = "admin",
                PasswordSalt = Convert.FromBase64String(@"yxOUhd3MwieTjhJjYJ8j0g=="), // Password for admin: Testing123$
                PasswordHash = Convert.FromBase64String(@"/8YcqyYE3901ny4MVWNwKPMGuDwo4Fb78rdMCCMALZgKjlTmlavHYPkCh7pmHV/iQr51Bc/wbWL0OIatYU8hCw=="),
                Argon2Iterations  = 16,
                Argon2MemoryCost  = 64,
                Argon2Parallelism =  2
            });

            #endregion


            #region --- Content tables ---
            Create.Table(   nameof(ContModule))
                .WithColumn(nameof(ContModule.Id))           .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(ContModule.Type))         .AsByte().NotNullable()
                .WithColumn(nameof(ContModule.ContPageId))   .AsInt32().NotNullable();

            Create.Table(   nameof(ContTextWallModule))
                .WithColumn(nameof(ContTextWallModule.Id))                .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(ContTextWallModule.LeftAsideContent))  .AsString().Nullable().WithDefaultValue(null)
                .WithColumn(nameof(ContTextWallModule.RightAsideContent)) .AsString().Nullable().WithDefaultValue(null)
                .WithColumn(nameof(ContTextWallModule.SectionContent))    .AsString().Nullable().WithDefaultValue(null)
                .WithColumn(nameof(ContTextWallModule.SectionWidth))      .AsInt16().NotNullable().WithDefaultValue(800);

            Create.ForeignKey(ForeignKeys.Current.CONTTEXTWALLMODULEID_CONTMODULEID_ASSIGNEDMODULEID)
                .FromTable(nameof(ContTextWallModule))       .ForeignColumn(nameof(ContTextWallModule.Id))
                .ToTable(  nameof(ContModule))               .PrimaryColumn(nameof(ContModule.Id))
                .OnDelete(System.Data.Rule.Cascade);

            Create.Table(   nameof(ContPage))
                .WithColumn(nameof(ContPage.Id))                          .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(ContPage.UriName))                     .AsString(255).NotNullable().Unique()
                .WithColumn(nameof(ContPage.Title))                       .AsString(255).NotNullable().Unique()
                .WithColumn(nameof(ContPage.Type))                        .AsByte().NotNullable().WithDefaultValue(3)
                .WithColumn(nameof(ContPage.CreateUtcTimestamp))          .AsDateTime().NotNullable()
                .WithColumn(nameof(ContPage.PublishUtcTimestamp))         .AsDateTime().NotNullable();

            Create.ForeignKey(ForeignKeys.Current.CONTMODULE_PAGE_ASSIGNEDID)
                .FromTable(nameof(ContModule))   .ForeignColumn(nameof(ContModule.ContPageId))
                .ToTable(  nameof(ContPage))     .PrimaryColumn(nameof(ContPage.Id))
                .OnDelete(System.Data.Rule.Cascade);


            Insert.IntoTable(nameof(ContPage)).Row(new
            {
                Id = 1,
                UriName = "homepage",
                Title = "Homepage",
                Type = 1,
                CreateUtcTimestamp  = new DateTime(2021, 10, 1, 18, 15, 0),
                PublishUtcTimestamp = new DateTime(2021, 10, 1, 18, 25, 0)
            });
            for (int i = 1; i <= 5; ++i)
            {
                Insert.IntoTable(nameof(ContModule)).Row(new
                {
                    Id = i,
                    Type = 1,
                    ContPageId = 1
                });
                Insert.IntoTable(nameof(ContTextWallModule)).Row(new
                {
                    Id = i,
                    LeftAsideContent  = (string)null,
                    RightAsideContent = (string)null,
                    SectionWidth = 600,
                    SectionContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur."
                });
            }


            Insert.IntoTable(nameof(ContPage)).Row(new
            {
                Id = 2,
                UriName = "simple-article",
                Title = "A simple article",
                Type = 2,
                CreateUtcTimestamp  = new DateTime(2021, 10, 1, 18, 30, 0),
                PublishUtcTimestamp = new DateTime(2021, 10, 1, 18, 40, 0)
            });
            Insert.IntoTable(nameof(ContModule)).Row(new
            {
                Id = 6,
                Type = 1,
                ContPageId = 2
            });
            Insert.IntoTable(nameof(ContTextWallModule)).Row(new
            {
                Id = 6,
                LeftAsideContent  = (string)null,
                RightAsideContent = (string)null,
                SectionWidth = 800,
                SectionContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur."
            });

            #endregion


            #region --- Files ---
            Create.Table(nameof(ContFile))
                .WithColumn(nameof(ContFile.Id))                   .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(ContFile.ParentFileId))         .AsInt32().Nullable()
                .WithColumn(nameof(ContFile.IsDirectory))          .AsBoolean().NotNullable()
                .WithColumn(nameof(ContFile.CreationUtcTimestamp)) .AsDateTime().NotNullable()
                .WithColumn(nameof(ContFile.Name))                 .AsString(250).NotNullable()
                .WithColumn(nameof(ContFile.AccessGuid))           .AsGuid().NotNullable();

            Create.ForeignKey(ForeignKeys.Current.CONTFILEID_CONTFILEID_PARENTFILEID)
                .FromTable(nameof(ContFile))                       .ForeignColumn(nameof(ContFile.ParentFileId))
                .ToTable(  nameof(ContFile))                       .PrimaryColumn(nameof(ContFile.Id))
                .OnDelete(System.Data.Rule.Cascade);
            #endregion

        }

        #region Unused
        public override void Down()
        {
            // As there is no lower version
            // no action is required here.
        }
        #endregion
    }
}
