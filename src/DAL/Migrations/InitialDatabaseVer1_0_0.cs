using DAL.Models.Database.Tables;
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
            Create.Table(   nameof(Auth_User))
                .WithColumn(nameof(Auth_User.Id))                .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(Auth_User.Username))          .AsString(40).NotNullable()
                .WithColumn(nameof(Auth_User.Description))       .AsString(250).Nullable().WithDefaultValue(null)
                .WithColumn(nameof(Auth_User.PasswordHash))      .AsBinary(128).NotNullable()
                .WithColumn(nameof(Auth_User.PasswordSalt))      .AsBinary(32).NotNullable()
                .WithColumn(nameof(Auth_User.Argon2Iterations))  .AsInt32().NotNullable()
                .WithColumn(nameof(Auth_User.Argon2MemoryCost))  .AsInt32().NotNullable()
                .WithColumn(nameof(Auth_User.Argon2Parallelism)) .AsInt32().NotNullable()
                .WithColumn(nameof(Auth_User.Auth_RoleId))       .AsInt32().NotNullable();

            Create.Table(   nameof(Auth_Token))
                .WithColumn(nameof(Auth_Token.Id))            .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(Auth_Token.Auth_UserId))   .AsInt32().NotNullable()
                .WithColumn(nameof(Auth_Token.HashedToken))   .AsBinary(128).NotNullable()
                .WithColumn(nameof(Auth_Token.UtcExpiryTime)) .AsDateTime().NotNullable();

            Create.ForeignKey(ForeignKeys.Current.AUTHTOKEN_AUTHUSER_ASSIGNEDUSERID)
                .FromTable(nameof(Auth_Token))              .ForeignColumn(nameof(Auth_Token.Auth_UserId))
                .ToTable(  nameof(Auth_User))               .PrimaryColumn(nameof(Auth_User.Id));

            Create.Table(   nameof(Auth_Policy))
                .WithColumn(nameof(Auth_Policy.Id))         .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(Auth_Policy.Name))       .AsString(50).NotNullable();

            Create.Table(   nameof(Auth_Role))
                .WithColumn(nameof(Auth_Role.Id))           .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(Auth_Role.Name))         .AsString(50).NotNullable()
                .WithColumn(nameof(Auth_Role.FactoryRole))  .AsBoolean().NotNullable().WithDefaultValue(false);

            Create.Table(   nameof(Auth_Role_Policy_Assign))
                .WithColumn(nameof(Auth_Role_Policy_Assign.Auth_RoleId))   .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(Auth_Role_Policy_Assign.Auth_PolicyId)) .AsInt32().NotNullable().PrimaryKey();

            Create.ForeignKey(ForeignKeys.Current.AUTHROLEPOLICYASSIGN_AUTHROLE_ASSIGNEDROLEID)
                .FromTable(nameof(Auth_Role_Policy_Assign)) .ForeignColumn(nameof(Auth_Role_Policy_Assign.Auth_RoleId))
                .ToTable(  nameof(Auth_Role))               .PrimaryColumn(nameof(Auth_Role.Id));

            Create.ForeignKey(ForeignKeys.Current.AUTHROLEPOLICYASSIGN_AUTHPOLICY_ASSIGNEDPOLICYID)
                .FromTable(nameof(Auth_Role_Policy_Assign)) .ForeignColumn(nameof(Auth_Role_Policy_Assign.Auth_PolicyId))
                .ToTable(  nameof(Auth_Policy))             .PrimaryColumn(nameof(Auth_Policy.Id));

            Create.ForeignKey(ForeignKeys.Current.AUTHUSER_AUTHROLE_ASSIGNEDROLEID)
                .FromTable(nameof(Auth_User))               .ForeignColumn(nameof(Auth_User.Auth_RoleId))
                .ToTable(  nameof(Auth_Role))               .PrimaryColumn(nameof(Auth_Role.Id));

            
            Insert.IntoTable(nameof(Auth_Policy)).Row(new { Id =  1, Name = "CreateBlogPost" })
                                                 .Row(new { Id =  2, Name = "EditBlogPost"   })
                                                 .Row(new { Id =  3, Name = "DeleteBlogPost" })
                                                 .Row(new { Id =  4, Name = "DeleteBlogPostArchivedRevision" })
                                                 .Row(new { Id =  5, Name = "CreatePage" })
                                                 .Row(new { Id =  6, Name = "EditPage"   })
                                                 .Row(new { Id =  7, Name = "DeletePage" })
                                                 .Row(new { Id =  8, Name = "DeletePageArchivedRevision" })
                                                 .Row(new { Id =  9, Name = "ApplySiteTheme"  })
                                                 .Row(new { Id = 10, Name = "CreateSiteTheme" })
                                                 .Row(new { Id = 11, Name = "EditSiteTheme"   })
                                                 .Row(new { Id = 12, Name = "DeleteSiteTheme" })
                                                 .Row(new { Id = 13, Name = "CreateAnyUser"   })
                                                 .Row(new { Id = 14, Name = "EditAnyUser"     })
                                                 .Row(new { Id = 15, Name = "DeleteAnyUser"   })
                                                 .Row(new { Id = 16, Name = "PromoteAnyUser"  })
                                                 .Row(new { Id = 17, Name = "ChangeOwnUsername" }); // TODO: Add more policies / permissions.

            Insert.IntoTable(nameof(Auth_Role)).Row(new { FactoryRole = true, Name = "Admin" });
            for (int i=1; i <= 17; ++i)
            {
                Insert.IntoTable(nameof(Auth_Role_Policy_Assign)).Row(new { Auth_RoleId = 1, Auth_PolicyId = i });
            }

            Insert.IntoTable(nameof(Auth_User)).Row(new
            {
                Auth_RoleId  = 1,
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
            Create.Table(   nameof(Cont_Module))
                .WithColumn(nameof(Cont_Module.Id))           .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(Cont_Module.Type))         .AsByte().NotNullable()
                .WithColumn(nameof(Cont_Module.Cont_PageId))  .AsInt32().NotNullable();

            Create.Table(   nameof(Cont_TextWallModule))
                .WithColumn(nameof(Cont_TextWallModule.Id))                .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(Cont_TextWallModule.LeftAsideContent))  .AsString().Nullable().WithDefaultValue(null)
                .WithColumn(nameof(Cont_TextWallModule.RightAsideContent)) .AsString().Nullable().WithDefaultValue(null)
                .WithColumn(nameof(Cont_TextWallModule.SectionContent))    .AsString().Nullable().WithDefaultValue(null)
                .WithColumn(nameof(Cont_TextWallModule.SectionWidth))      .AsInt16().NotNullable().WithDefaultValue(800);

            Create.ForeignKey(ForeignKeys.Current.CONTTEXTWALLMODULEID_CONTMODULEID_ASSIGNEDMODULEID)
                .FromTable(nameof(Cont_TextWallModule))       .ForeignColumn(nameof(Cont_TextWallModule.Id))
                .ToTable(  nameof(Cont_Module))               .PrimaryColumn(nameof(Cont_Module.Id));

            Create.Table(   nameof(Cont_Page))
                .WithColumn(nameof(Cont_Page.Id))                          .AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(Cont_Page.Name))                        .AsString(255).NotNullable().Unique()
                .WithColumn(nameof(Cont_Page.Type))                        .AsByte().NotNullable().WithDefaultValue(3)
                .WithColumn(nameof(Cont_Page.CreateUtcTimestamp))          .AsDateTime().NotNullable()
                .WithColumn(nameof(Cont_Page.PublishUtcTimestamp))         .AsDateTime().NotNullable();

            Create.ForeignKey(ForeignKeys.Current.CONTMODULE_PAGE_ASSIGNEDID)
                .FromTable(nameof(Cont_Module))   .ForeignColumn(nameof(Cont_Module.Cont_PageId))
                .ToTable(  nameof(Cont_Page))     .PrimaryColumn(nameof(Cont_Page.Id));


            Insert.IntoTable(nameof(Cont_Page)).Row(new
            {
                Id = 1,
                Name = string.Empty,
                Type = 1,
                CreateUtcTimestamp = new DateTime(2021, 10, 1, 18, 15, 0),
                PublishUtcTimestamp = new DateTime(2021, 10, 1, 18, 25, 0)
            });
            for (int i = 1; i <= 5; ++i)
            {
                Insert.IntoTable(nameof(Cont_Module)).Row(new
                {
                    Id = i,
                    Type = 1,
                    Cont_PageId = 1
                });
                Insert.IntoTable(nameof(Cont_TextWallModule)).Row(new
                {
                    Id = i,
                    LeftAsideContent = (string)null,
                    RightAsideContent = (string)null,
                    SectionWidth = 600,
                    SectionContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur."
                });
            }


            Insert.IntoTable(nameof(Cont_Page)).Row(new
            {
                Id = 2,
                Name = "simple-article",
                Type = 2,
                CreateUtcTimestamp = new DateTime(2021, 10, 1, 18, 30, 0),
                PublishUtcTimestamp = new DateTime(2021, 10, 1, 18, 40, 0)
            });
            Insert.IntoTable(nameof(Cont_Module)).Row(new
            {
                Id = 6,
                Type = 1,
                Cont_PageId = 2
            });
            Insert.IntoTable(nameof(Cont_TextWallModule)).Row(new
            {
                Id = 6,
                LeftAsideContent = (string)null,
                RightAsideContent = (string)null,
                SectionWidth = 800,
                SectionContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur."
            });
            
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
