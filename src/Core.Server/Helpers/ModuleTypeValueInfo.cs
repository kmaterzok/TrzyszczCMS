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
        public short Type { get; set; }
    }

    /// <summary>
    /// Methods for managing page modules info.
    /// </summary>
    public static class ModuleTypeValueInfoExtensions
    {
        /// <summary>
        /// Get content of page's modules.
        /// </summary>
        /// <param name="moduleInfos">Simple imfo about requested modules</param>
        /// <param name="dbContext">Database context which the data is fetched from1</param>
        /// <returns>List of modules for page</returns>
        public static async Task<List<ModuleContent>> GetModuleContentsAsync(this List<ModuleTypeValueInfo> moduleInfos, CmsDbContext dbContext)
        {
            var moduleContents = new List<ModuleContent>();

            foreach (var moduleInfo in moduleInfos)
            {
                switch ((PageModuleType)moduleInfo.Type)
                {
                    case PageModuleType.TextWall:
                        var textWallModule = await dbContext.ContTextWallModules.AsNoTracking().FirstAsync(i => i.Id == moduleInfo.Id);
                        moduleContents.Add(textWallModule.ToModuleContent());
                        break;

                    case PageModuleType.HeadingBanner:
                        var headingBannerModule = await dbContext.ContHeadingBannerModules.AsNoTracking().FirstAsync(i => i.Id == moduleInfo.Id);
                        moduleContents.Add(await headingBannerModule.ToModuleContentAsync(dbContext));
                        break;

                    default:
                        throw new ApplicationException($"Cannot process {nameof(PageModuleType)} of type {(byte)moduleInfo.Type}");
                }
            }
            return moduleContents;
        }
    }

}
