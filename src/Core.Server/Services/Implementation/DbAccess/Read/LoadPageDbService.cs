using Core.Server.Models.Extensions;
using Core.Server.Services.Interfaces.DbAccess.Read;
using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using Core.Shared.Models.PageContent;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using DAL.Models.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string queriedName = type == PageType.HomePage ? string.Empty : name;

            using (var ctx = _databaseStrategy.GetContext())
            {
                var rawValueOfType = (byte)type;
                var moduleInfos = await (from p in ctx.Cont_Page.AsNoTracking()
                                         join m in ctx.Cont_Module.AsNoTracking() on p.Id equals m.Cont_PageId
                                         where p.Name == queriedName &&
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
