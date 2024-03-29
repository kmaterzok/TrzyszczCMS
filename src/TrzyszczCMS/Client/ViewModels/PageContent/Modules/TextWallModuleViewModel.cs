﻿using TrzyszczCMS.Core.Shared.Models.PageContent;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.PageContent.Modules
{
    /// <summary>
    /// The viewmodel for text wall page module.
    /// </summary>
    public class TextWallModuleViewModel : ViewModelBase
    {
        #region Properties
        private TextWallModuleContent _ModuleContent;
        /// <summary>
        /// Content of the module which the viewmodel gets data from.
        /// </summary>
        public TextWallModuleContent ModuleContent
        {
            get => _ModuleContent;
            set => Set(ref _ModuleContent, value, nameof(ModuleContent));
        }
        #endregion

        #region Ctor
        public TextWallModuleViewModel(TextWallModuleContent moduleContent) =>
            this.ModuleContent = moduleContent;
        #endregion
    }
}
