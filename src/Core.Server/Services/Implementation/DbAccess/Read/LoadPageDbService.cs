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
            var moduleContents = new List<ModuleContent>();
            string queriedName = type == PageType.HomePage ? string.Empty : name;

            using (var ctx = _databaseStrategy.GetContext())
            {
                var rawValueOfType = (byte)type;
                var moduleInfos = await (from p in ctx.Cont_Page.AsNoTracking()
                                         join m in ctx.Cont_Module.AsNoTracking() on p.Id equals m.Cont_PageId
                                         where p.Name == queriedName &&
                                            p.Type == rawValueOfType &&
                                            p.PublishUtcTimestamp <= DateTime.UtcNow
                                         select new
                                         {
                                             Id = m.Id,
                                             Type = m.Type
                                         }).ToListAsync();

                foreach (var moduleInfo in moduleInfos)
                {
                    switch ((PageModuleType)(byte)moduleInfo.Type)
                    {
                        case PageModuleType.TextWall:
                            var textWallModule = await ctx.Cont_TextWallModule.AsNoTracking().FirstAsync(i => i.Id == moduleInfo.Id);
                            moduleContents.Add(textWallModule.ToModuleContent());
                            break;

                        default:
                            throw new ApplicationException($"Cannot process PageType of type {(byte)moduleInfo.Type}");
                    }
                }

                return new ModularPageContentResponse()
                {
                    ModuleContents = moduleContents
                };
            }
        }
        #endregion
    }
}
