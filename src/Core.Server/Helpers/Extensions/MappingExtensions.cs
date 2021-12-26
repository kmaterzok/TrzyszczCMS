using Core.Shared.Models.ManageFiles;
using Core.Shared.Models.ManageUser;
using DAL.Models.Database;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace Core.Server.Helpers.Extensions
{
    /// <summary>
    /// The class of methods easing mapping of data between EF Core models and endpoints' models.
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Get <see cref="IQueryable{SimpleRoleInfo}"/> from <see cref="IQueryable{AuthRole}"/> by mapping data.
        /// </summary>
        /// <param name="source">Queryable source of data</param>
        /// <returns>Remapped Queryable</returns>
        public static IQueryable<SimpleRoleInfo> ToSimpleRoleInfo(this IQueryable<AuthRole> source) => source.Select(i => new SimpleRoleInfo()
        {
            Id = i.Id,
            Name = i.Name
        });

        /// <summary>
        /// Get <see cref="SimpleFileInfo"/> from <see cref="EntityEntry{ContFile}"/> by mapping data.
        /// </summary>
        /// <param name="source">Database entity class instance</param>
        /// <returns>Mapped object</returns>
        public static SimpleFileInfo ToSimpleFileInfo(this EntityEntry<ContFile> source) => new SimpleFileInfo()
        {
            Id                   = source.Entity.Id,
            AccessGuid           = source.Entity.AccessGuid,
            CreationUtcTimestamp = source.Entity.CreationUtcTimestamp,
            IsDirectory          = source.Entity.IsDirectory,
            Name                 = source.Entity.Name,
            ParentFileId         = source.Entity.ParentFileId
        };
    }
}
