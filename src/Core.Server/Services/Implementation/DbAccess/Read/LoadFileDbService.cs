using Core.Server.Services.Interfaces.DbAccess.Read;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementation.DbAccess.Read
{
    public class LoadFileDbService : ILoadFileDbService
    {
        #region Fields
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public LoadFileDbService(IDatabaseStrategyFactory databaseStrategyFactory) =>
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Read);
        #endregion

        #region Methods
        public async Task<string> GetMimeType(Guid fileGuid)
        {
            using (var ctx = this._databaseStrategy.GetContext())
            {
                return (await ctx.ContFiles.AsNoTracking().SingleAsync(i => i.AccessGuid == fileGuid)).MimeType;
            }
        }
        #endregion
    }
}
