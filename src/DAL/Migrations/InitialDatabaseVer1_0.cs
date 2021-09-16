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
            throw new NotImplementedException();
        }
        #region Unused
        public override void Down()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
