using DAL.Models.Database;
using DAL.Shared.Data;
using FluentMigrator;
using System;
using System.Linq;

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
                .WithColumn(nameof(AuthUser.Username))          .AsString(Constraints.AuthUser.USERNAME).NotNullable().Unique()
                .WithColumn(nameof(AuthUser.Description))       .AsString(Constraints.AuthUser.DESCRIPTION).Nullable().WithDefaultValue(null)
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

            
            Insert.IntoTable(nameof(AuthPolicy)).Row(new { Id =  1, Name = UserPolicies.HOMEPAGE_EDITING      })
                                                .Row(new { Id =  2, Name = UserPolicies.BLOG_POST_CREATING    })
                                                .Row(new { Id =  3, Name = UserPolicies.BLOG_POST_EDITING     })
                                                .Row(new { Id =  4, Name = UserPolicies.BLOG_POST_DELETING    })
                                                .Row(new { Id =  5, Name = UserPolicies.ARTICLE_CREATING         })
                                                .Row(new { Id =  6, Name = UserPolicies.ARTICLE_EDITING          })
                                                .Row(new { Id =  7, Name = UserPolicies.ARTICLE_DELETING         })
                                                .Row(new { Id =  8, Name = UserPolicies.ANY_USER_CREATING     })
                                                .Row(new { Id =  9, Name = UserPolicies.ANY_USER_EDITING      })
                                                .Row(new { Id = 10, Name = UserPolicies.ANY_USER_DELETING     })
                                                .Row(new { Id = 11, Name = UserPolicies.MANAGE_NAVIGATION_BAR })
                                                .Row(new { Id = 12, Name = UserPolicies.FILE_ADDING           })
                                                .Row(new { Id = 13, Name = UserPolicies.FILE_DELETING         });


            Insert.IntoTable(nameof(AuthRole)).Row(new { FactoryRole = true, Name = "Admin" });
            for (int i=1; i <= 13; ++i)
                { Insert.IntoTable(nameof(AuthRolePolicyAssign)).Row(new { AuthRoleId = 1, AuthPolicyId = i }); }


            Insert.IntoTable(nameof(AuthRole)).Row(new { FactoryRole = true, Name = "Editor" });
            for (int i = 1; i <= 7; ++i)
                { Insert.IntoTable(nameof(AuthRolePolicyAssign)).Row(new { AuthRoleId = 2, AuthPolicyId = i }); }
            for (int i = 11; i <= 13; ++i)
                { Insert.IntoTable(nameof(AuthRolePolicyAssign)).Row(new { AuthRoleId = 2, AuthPolicyId = i }); }


            Insert.IntoTable(nameof(AuthRole)).Row(new { FactoryRole = true, Name = "Writer" });
            foreach (var i in Enumerable.Range(2, 5).Where(v => v != 4))
                { Insert.IntoTable(nameof(AuthRolePolicyAssign)).Row(new { AuthRoleId = 3, AuthPolicyId = i }); }
            Insert.IntoTable(nameof(AuthRolePolicyAssign)).Row(new { AuthRoleId = 3, AuthPolicyId = 12 });


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


            #region --- Menu ---
            Create.Table(nameof(ContMenuItem))
                .WithColumn(nameof(ContMenuItem.Id))           .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(ContMenuItem.ParentItemId)) .AsInt32().Nullable()
                .WithColumn(nameof(ContMenuItem.Name))         .AsString(Constraints.ContMenuItem.NAME).NotNullable()
                .WithColumn(nameof(ContMenuItem.Uri))          .AsString(Constraints.ContMenuItem.URI).Nullable()
                .WithColumn(nameof(ContMenuItem.OrderNumber))  .AsInt32().NotNullable();

            Create.ForeignKey(ForeignKeys.Current.CONTMENUITEM_CONTMENUITEM_PARENTITEMID)
                .FromTable(nameof(ContMenuItem))               .ForeignColumn(nameof(ContMenuItem.ParentItemId))
                .ToTable(  nameof(ContMenuItem))               .PrimaryColumn(nameof(ContMenuItem.Id))
                .OnDelete(System.Data.Rule.Cascade);
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

            Create.ForeignKey(ForeignKeys.Current.CONTTEXTWALLMODULE_CONTMODULE_ASSIGNEDMODULEID)
                .FromTable(nameof(ContTextWallModule))       .ForeignColumn(nameof(ContTextWallModule.Id))
                .ToTable(  nameof(ContModule))               .PrimaryColumn(nameof(ContModule.Id))
                .OnDelete(System.Data.Rule.Cascade);

            Create.Table(   nameof(ContPage))
                .WithColumn(nameof(ContPage.Id))                          .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(ContPage.UriName))                     .AsString(Constraints.ContPage.URI_NAME).NotNullable().Unique()
                .WithColumn(nameof(ContPage.Title))                       .AsString(Constraints.ContPage.TITLE).NotNullable()
                .WithColumn(nameof(ContPage.Type))                        .AsByte().NotNullable().WithDefaultValue(3)
                .WithColumn(nameof(ContPage.CreateUtcTimestamp))          .AsDateTime().NotNullable()
                .WithColumn(nameof(ContPage.PublishUtcTimestamp))         .AsDateTime().NotNullable()
                .WithColumn(nameof(ContPage.AuthorsInfo))                 .AsString(Constraints.ContPage.AUTHORS_INFO).Nullable().WithDefaultValue(null)
                .WithColumn(nameof(ContPage.Description))                 .AsString(Constraints.ContPage.DESCRIPTION).Nullable().WithDefaultValue(null);

            Create.ForeignKey(ForeignKeys.Current.CONTMODULE_PAGE_ASSIGNEDID)
                .FromTable(nameof(ContModule))   .ForeignColumn(nameof(ContModule.ContPageId))
                .ToTable(  nameof(ContPage))     .PrimaryColumn(nameof(ContPage.Id))
                .OnDelete(System.Data.Rule.Cascade);

            Create.Table(   nameof(ContHeadingBannerModule))
                .WithColumn(nameof(ContHeadingBannerModule.Id))                        .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(ContHeadingBannerModule.DisplayDescription))        .AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn(nameof(ContHeadingBannerModule.DisplayAuthorsInfo))        .AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn(nameof(ContHeadingBannerModule.DarkDescription))           .AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn(nameof(ContHeadingBannerModule.ViewportHeight))            .AsByte().NotNullable().WithDefaultValue(40)
                .WithColumn(nameof(ContHeadingBannerModule.AttachLinkMenu))            .AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn(nameof(ContHeadingBannerModule.BackgroundPictureId))       .AsInt32().Nullable().WithDefaultValue(null);

            Create.ForeignKey(ForeignKeys.Current.CONTHEADINGBANNERMODULE_CONTMODULE_ASSIGNEDMODULEID)
                .FromTable(nameof(ContHeadingBannerModule))       .ForeignColumn(nameof(ContHeadingBannerModule.Id))
                .ToTable(  nameof(ContModule))                    .PrimaryColumn(nameof(ContModule.Id))
                .OnDelete(System.Data.Rule.Cascade);

            Create.Table(   nameof(ContPostListingModule))
                .WithColumn(nameof(ContPostListingModule.Id))                          .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(ContPostListingModule.Width))                       .AsInt16().NotNullable().WithDefaultValue(800);

            Create.ForeignKey(ForeignKeys.Current.CONTPOSTLISTINGMODULE_CONTMODULE_ASSIGNEDMODULEID)
                .FromTable(nameof(ContPostListingModule))         .ForeignColumn(nameof(ContPostListingModule.Id))
                .ToTable(  nameof(ContModule))                    .PrimaryColumn(nameof(ContModule.Id))
                .OnDelete(System.Data.Rule.Cascade);



            Insert.IntoTable(nameof(ContPage)).Row(new
            {
                Id = 1,
                UriName = "homepage",
                Title = "Homepage",
                Type = 1,
                CreateUtcTimestamp  = new DateTime(2021, 10, 1, 18, 15, 0),
                PublishUtcTimestamp = new DateTime(2021, 10, 1, 18, 25, 0),
                AuthorsInfo = (string)null
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
                PublishUtcTimestamp = new DateTime(2021, 10, 1, 18, 40, 0),
                AuthorsInfo = "Someone who wrote it ;)"
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
                .WithColumn(nameof(ContFile.Name))                 .AsString(Constraints.ContFile.NAME).NotNullable()
                .WithColumn(nameof(ContFile.AccessGuid))           .AsGuid().NotNullable().Unique()
                .WithColumn(nameof(ContFile.MimeType))             .AsString(100).Nullable();

            Create.ForeignKey(ForeignKeys.Current.CONTFILE_CONTFILE_PARENTFILEID)
                .FromTable(nameof(ContFile))                       .ForeignColumn(nameof(ContFile.ParentFileId))
                .ToTable(  nameof(ContFile))                       .PrimaryColumn(nameof(ContFile.Id))
                .OnDelete(System.Data.Rule.Cascade);

            Create.ForeignKey(ForeignKeys.Current.CONTHEADINGBANNERMODULE_CONTFILE_ASSIGNEDPICTUREFILEID)
                .FromTable(nameof(ContHeadingBannerModule))        .ForeignColumn(nameof(ContHeadingBannerModule.BackgroundPictureId))
                .ToTable(  nameof(ContFile))                       .PrimaryColumn(nameof(ContFile.Id))
                .OnDelete(System.Data.Rule.SetNull);
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
