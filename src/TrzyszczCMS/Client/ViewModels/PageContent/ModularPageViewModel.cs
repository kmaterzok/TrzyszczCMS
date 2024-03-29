﻿using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Core.Shared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.ViewModels.Shared;
using TrzyszczCMS.Client.Views.PageContent;

namespace TrzyszczCMS.Client.ViewModels.PageContent
{
    /// <summary>
    /// The viewmodel for <see cref="ModularPageComponent"/> displaying various modules (parts) of the page.
    /// </summary>
    public class ModularPageViewModel : ViewModelBase
    {
        #region Fields
        private readonly ILoadPageService _loadPageService;
        #endregion

        #region Properties
        private List<ViewModelBase> _viewModelsOfModules;
        /// <summary>
        /// List of viewmodels which are used by page modules that display its content
        /// </summary>
        public List<ViewModelBase> ViewModelsForModules
        {
            get => _viewModelsOfModules;
            set => Set(ref _viewModelsOfModules, value, nameof(ViewModelsForModules));
        }

        private bool? _contentFound;
        /// <summary>
        /// Expresses if the page's content has actually been found and loaded so bropwser may display it.
        /// </summary>
        public bool? ContentFound
        {
            get => _contentFound;
            set => Set(ref _contentFound, value, nameof(ContentFound));
        }
        #endregion


        #region Ctor
        public ModularPageViewModel(ILoadPageService loadPageService)
        {
            this._loadPageService = loadPageService;
            this.ViewModelsForModules = new List<ViewModelBase>();
            this.ContentFound = null;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Method loading data modules for displaying its content.
        /// </summary>
        /// <param name="type">Type of the requested page</param>
        /// <param name="name">Name of the requested page</param>
        public async Task PrepareModules(PageType type, string name)
        {
            this.ViewModelsForModules = (await this._loadPageService.GetPageContentAsync(type, name))
                                                   .CreateViewModels(this._loadPageService);

            this.ContentFound = this.ViewModelsForModules.Count > 0;
        }
        #endregion
    }
}
