using DAL.Models.Database.Tables;
using FluentMigrator;
using System;

namespace DAL.Migrations
{
    /// <summary>
    /// The migration initialising a database to its very first version.
    /// </summary>
    [Migration(1000)]
    public class InitialDatabaseVer1_0 : Migration
    {
        public override void Up()
        {
            #region --- Auth tables ---
            Create.Table(   nameof(Auth_User))
                .WithColumn(nameof(Auth_User.Id))           .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(Auth_User.Username))     .AsString(40).NotNullable()
                .WithColumn(nameof(Auth_User.Description))  .AsString(250).Nullable().WithDefaultValue(null)
                .WithColumn(nameof(Auth_User.PasswordHash)) .AsString(250).NotNullable()
                .WithColumn(nameof(Auth_User.PasswordSalt)) .AsString(30).NotNullable()
                .WithColumn(nameof(Auth_User.Auth_RoleId))  .AsInt32().NotNullable();

            Create.Table(   nameof(Auth_Token))
                .WithColumn(nameof(Auth_Token.Id))          .AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn(nameof(Auth_Token.Auth_UserId)) .AsInt32().NotNullable();

            Create.ForeignKey(ForeignKeys.Current.AUTHTOKEN_AUTHUSER_ASSIGNEDUSERID)
                .FromTable(nameof(Auth_Token))              .ForeignColumn(nameof(Auth_Token.Auth_UserId))
                .ToTable(  nameof(Auth_User))               .PrimaryColumn(nameof(Auth_User.Id));



            Create.Table(   nameof(Auth_Policy))
                .WithColumn(nameof(Auth_Policy.Id))         .AsInt32().NotNullable().PrimaryKey().Identity()
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
