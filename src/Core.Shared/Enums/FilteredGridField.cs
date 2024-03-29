﻿namespace TrzyszczCMS.Core.Shared.Enums
{
    /// <summary>
    /// Contains all grids' field names that can be used for filtering content.
    /// </summary>
    public enum FilteredGridField
    {
        #region ManagePages
        ManagePages_Posts_Title,
        ManagePages_Posts_Created,
        ManagePages_Posts_Published,

        ManagePages_Articles_Title,
        ManagePages_Articles_Created,
        ManagePages_Articles_Published,
        #endregion

        #region ManageUsers
        ManageUsers_UserName,
        ManageUsers_Description,
        ManageUsers_Role,
        #endregion

        #region ManageFiles
        ManageFiles_Name,
        ManageFiles_Created
        #endregion
    }
}
