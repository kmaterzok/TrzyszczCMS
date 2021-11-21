﻿using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManagePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces.DbAccess.Modify
{
    public interface IManagePageDbService
    {
        /// <summary>
        /// Get basic information about sites.
        /// </summary>
        /// <param name="type">Type of fetched pages</param>
        /// <param name="desiredPageNumber">Number of first page that will be fetched</param>
        /// <param name="filters">Filters applied for results</param>
        /// <returns>Task returning a page of data</returns>
        Task<DataPage<SimplePageInfo>> GetSimplePageInfoPage(PageType type, int desiredPageNumber, Dictionary<FilteredGridField, string> filters);
        /// <summary>
        /// Get detailed information about a single site.
        /// </summary>
        /// <param name="id">Database row ID of the site</param>
        /// <returns>Task returning details about the site</returns>
        Task<DetailedPageInfo> GetDetailedPageInfo(int id);
        /// <summary>
        /// Get detailed information about the homepage.
        /// </summary>
        /// <returns>Task returning details about the homepage</returns>
        Task<DetailedPageInfo> GetDetailedPageInfoOfHomepage();
    }
}