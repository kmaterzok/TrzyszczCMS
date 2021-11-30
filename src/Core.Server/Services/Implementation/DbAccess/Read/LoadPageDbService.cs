using Core.Server.Services.Interfaces.DbAccess.Read;
using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Server.Helpers;

namespace Core.Server.Services.Implementation.DbAccess.Read
{
    public class LoadPageDbService : ILoadPageDbService
    {
        #region Fields
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public LoadPageDbService(IDatabaseStrategyFactory databaseStrategyFactory)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Read);
        }
        #endregion

        #region Methods
        public async Task<ModularPageContentResponse> GetPageContentAsync(PageType type, string name)
        {
            string queriedName = type == PageType.HomePage ? "homepage" : name;

            using (var ctx = _databaseStrategy.GetContext())
            {
                var rawValueOfType = (byte)type;
                var moduleInfos = await (from p in ctx.ContPages.AsNoTracking()
                                         join m in ctx.ContModules.AsNoTracking() on p.Id equals m.ContPageId
                                         where p.UriName == queriedName &&
                                            p.Type == rawValueOfType &&
                                            p.PublishUtcTimestamp <= DateTime.UtcNow
                                         select new ModuleTypeValueInfo()
                                         {
                                             Id = m.Id,
                                             Type = m.Type
                                         }).ToListAsync();

                return new ModularPageContentResponse()
                {
                    ModuleContents = await moduleInfos.GetModuleContentsAsync(ctx)
                };
            }
        }
        #endregion
    }
}
