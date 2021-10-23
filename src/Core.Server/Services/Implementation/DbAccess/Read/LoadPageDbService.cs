using Core.Server.Models.Extensions;
using Core.Server.Services.Interfaces.DbAccess.Read;
using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using Core.Shared.Models.SiteContent;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using DAL.Models.Database.Tables;
using Dapper;
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
            string sql = @"SELECT m.""Id"", m.""Type"" FROM ""Cont_Page"" p
                           INNER JOIN ""Cont_Module"" m ON p.""Id"" = m.""Cont_PageId""
                           WHERE p.""Name"" = @Name AND p.""Type"" = @Type;";

            var moduleContents = new List<ModuleContent>();
            string queriedName = type == PageType.HomePage ? string.Empty : name;

            using (var conn = _databaseStrategy.GetDbConnection())
            {
                var moduleInfos = (await conn.QueryAsync(sql, new { Type = (byte)type, Name = queriedName })).ToList();

                foreach(var moduleInfo in moduleInfos)
                {
                    switch ((PageModuleType)(byte)moduleInfo.Type)
                    {
                        case PageModuleType.TextWall:
                            string textWallSql = @"SELECT * FROM ""Cont_TextWallModule"" m WHERE m.""Id"" = @Id;";
                            var textWallModule = await conn.QueryFirstAsync<Cont_TextWallModule>(
                                textWallSql, new { Id = moduleInfo.Id }
                            );

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
