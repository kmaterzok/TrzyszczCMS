﻿using TrzyszczCMS.Core.Server.Helpers.Extensions;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Helpers;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.PageContent;
using TrzyszczCMS.Core.Infrastructure.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Models.Extensions
{
    /// <summary>
    /// Methods extending models functionality.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Create a complementary instance of <see cref="ModuleContent"/> basing on <see cref="ContTextWallModule"/> instance.
        /// </summary>
        /// <param name="dbTextWallModule">Input instance</param>
        /// <returns>Output instance</returns>
        public static ModuleContent ToModuleContent(this ContTextWallModule dbTextWallModule) => new ModuleContent()
        {
            TextWallModuleContent = new TextWallModuleContent()
            {
                SectionWidth              = (TextWallSectionWidth)dbTextWallModule.SectionWidth,
                LeftAsideMarkDownContent  = dbTextWallModule.LeftAsideContent,
                RightAsideMarkDownContent = dbTextWallModule.RightAsideContent,
                SectionMarkDownContent    = dbTextWallModule.SectionContent
            }
        };
        /// <summary>
        /// Create a complementary instance of <see cref="ModuleContent"/> basing on <see cref="ContHeadingBannerModule"/> instance.
        /// </summary>
        /// <param name="dbHeadingBannerModule">Input instance</param>
        /// <param name="context">Database context</param>
        /// <returns>Output instance</returns>
        public static async Task<ModuleContent> ToModuleContentAsync(this ContHeadingBannerModule dbHeadingBannerModule, CmsDbContext context)
        {
            var containingPage = await (from mod in context.ContModules
                                        join page in context.ContPages on mod.ContPageId equals page.Id
                                        where mod.Id == dbHeadingBannerModule.Id
                                        select page).FirstAsync();

            var containingFile = await context.ContFiles.FirstOrDefaultAsync(i => i.Id == dbHeadingBannerModule.BackgroundPictureId);

            return new ModuleContent()
            {
                HeadingBannerModuleContent = new HeadingBannerModuleContent()
                {
                    BackgroundPictureAccessGuid = containingFile?.AccessGuid.ToString(CommonConstants.FILE_ACCESS_ID_FORMAT),
                    ViewportHeight              = (HeadingBannerHeight)dbHeadingBannerModule.ViewportHeight,
                    DisplayDescription          = dbHeadingBannerModule.DisplayDescription.Value,
                    DisplayAuthorsInfo          = dbHeadingBannerModule.DisplayAuthorsInfo.Value,
                    DarkDescription             = dbHeadingBannerModule.DarkDescription,
                    AttachLinkMenu              = dbHeadingBannerModule.AttachLinkMenu.Value,
                    PublishUtcTimestamp         = containingPage.PublishUtcTimestamp,
                    Description                 = containingPage.Description,
                    AuthorsInfo                 = containingPage.AuthorsInfo,
                    Title                       = containingPage.Title,
                    MenuItems                   = context.GetAllMenuItems()
                }
            };
        }
        /// <summary>
        /// Create a complementary instance of <see cref="ModuleContent"/> basing on <see cref="ContPostListingModule"/> instance.
        /// </summary>
        /// <param name="dbTextWallModule">Input instance</param>
        /// <returns>Output instance</returns>
        public static ModuleContent ToModuleContent(this ContPostListingModule dbPostListingModule) => new ModuleContent()
        {
            PostListingModuleContent = new PostListingModuleContent()
            {
                Width = (PostListingWidth)dbPostListingModule.Width
            }
        };
        /// <summary>
        /// Create a complementary instance of <see cref="ContModule"/> basing on <see cref="ModuleContent"/> instance.
        /// </summary>
        /// <param name="source">Input instance</param>
        /// <param name="context">Database context</param>
        /// <returns>Output instance</returns>
        public static ContModule ToContModule(this ModuleContent source, CmsDbContext context)
        {
            var type = source.GetModuleType();
            var desiredModule = new ContModule()
            {
                Type = (byte)type
            };
            switch (type)
            {
                case PageModuleType.TextWall:
                    desiredModule.ContTextWallModule = new ContTextWallModule()
                    {
                        SectionWidth      = (short)source.TextWallModuleContent.SectionWidth,
                        LeftAsideContent  = source.TextWallModuleContent.LeftAsideMarkDownContent,
                        RightAsideContent = source.TextWallModuleContent.RightAsideMarkDownContent,
                        SectionContent    = source.TextWallModuleContent.SectionMarkDownContent
                    };
                    break;
                case PageModuleType.HeadingBanner:
                    int? backgroundPictureId = null;
                    if (!string.IsNullOrEmpty(source.HeadingBannerModuleContent.BackgroundPictureAccessGuid))
                    {
                        var pictureAccessId = Guid.Parse(source.HeadingBannerModuleContent.BackgroundPictureAccessGuid);
                        backgroundPictureId = context.ContFiles.AsNoTracking().First(i => i.AccessGuid == pictureAccessId).Id;
                    }
                    desiredModule.ContHeadingBannerModule = new ContHeadingBannerModule()
                    {
                        BackgroundPictureId = backgroundPictureId,
                        AttachLinkMenu = source.HeadingBannerModuleContent.AttachLinkMenu,
                        DarkDescription = source.HeadingBannerModuleContent.DarkDescription,
                        DisplayDescription = source.HeadingBannerModuleContent.DisplayDescription,
                        DisplayAuthorsInfo = source.HeadingBannerModuleContent.DisplayAuthorsInfo,
                        ViewportHeight = (short)source.HeadingBannerModuleContent.ViewportHeight
                    };
                    break;
                case PageModuleType.PostListing:
                    desiredModule.ContPostListingModule = new ContPostListingModule()
                    {
                        Width = (short)source.PostListingModuleContent.Width
                    };
                    break;

                default:
                    throw ExceptionMaker.Argument.Unsupported(type, $"{nameof(source)}.{nameof(source.GetModuleType)}()");
            }
            return desiredModule;
        }

        /// <summary>
        /// Create <see cref="List{Cont_Module}"/> instance by mapping data.
        /// </summary>
        /// <param name="source">Data to be mapped to desired list</param>
        /// <param name="context">Database context</param>
        /// <returns>Remapped list</returns>
        public static List<ContModule> ToContModulesList(this IEnumerable<ModuleContent> source, CmsDbContext context) =>
            source?.Select(i => i.ToContModule(context)).ToList() ?? new List<ContModule>();

        /// <summary>
        /// Get all menu items.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <returns>Menu items in a recursive list</returns>
        public static List<DisplayedMenuItem> GetAllMenuItems(this CmsDbContext context)
        {
            var subLists = context.ContMenuItems.AsNoTracking()
                                                .OrderBy(i => i.OrderNumber)
                                                .AsEnumerable()
                                                .GroupBy(i => i.ParentItemId ?? -1)
                                                .ToDictionary(i => i.Key, i => i.Select(
                                                    e => new
                                                    {
                                                        Id = e.Id,
                                                        Item = new DisplayedMenuItem()
                                                        {
                                                            Name = e.Name,
                                                            Uri = e.Uri,
                                                            SubItems = null
                                                        }
                                                    }).AsEnumerable()
                                                );
            
            if (subLists.ContainsKey(-1))
            {
                return subLists[-1].Select(i => new DisplayedMenuItem()
                {
                    Name     = i.Item.Name,
                    Uri      = i.Item.Uri,
                    SubItems = subLists.ContainsKey(i.Id) ?
                        subLists[i.Id].Select(i => i.Item).ToList() :
                        null
                }).ToList();
            }
            return new List<DisplayedMenuItem>();
        }
    }
}
