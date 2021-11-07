using Core.Server.Models.Extensions;
using Core.Shared.Enums;
using Core.Shared.Models.PageContent;
using DAL.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Server.Helpers
{
    /// <summary>
    /// It describes assigning <see cref="Type"/> to the module represented by <see cref="Id"/>.
    /// </summary>
    public struct ModuleTypeValueInfo
    {
        /// <summary>
        /// Row ID of the module.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Type of the module.
        /// </summary>
        public byte Type { get; set; }
    }

    /// <summary>
    /// Methods for managing page modules info.
    /// </summary>
    public static class ModuleTypeValueInfoExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleInfos"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static async Task<List<ModuleContent>> GetModuleContentsAsync(this List<ModuleTypeValueInfo> moduleInfos, CmsDbContext dbContext)
        {
            var moduleContents = new List<ModuleContent>();

            foreach (var moduleInfo in moduleInfos)
            {
                switch ((PageModuleType)moduleInfo.Type)
                {
                    case PageModuleType.TextWall:
                        var textWallModule = await dbContext.Cont_TextWallModule.AsNoTracking().FirstAsync(i => i.Id == moduleInfo.Id);
                        moduleContents.Add(textWallModule.ToModuleContent());
                        break;

                    default:
                        throw new ApplicationException($"Cannot process {nameof(PageModuleType)} of type {(byte)moduleInfo.Type}");
                }
            }
            return moduleContents;
        }
    }

}
