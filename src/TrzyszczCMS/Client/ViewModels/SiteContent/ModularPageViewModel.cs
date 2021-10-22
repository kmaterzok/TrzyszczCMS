using Core.Application.Enums;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.ViewModels.Shared;
using TrzyszczCMS.Client.ViewModels.SiteContent.Modules;
using TrzyszczCMS.Client.Views.SiteContent;

namespace TrzyszczCMS.Client.ViewModels.SiteContent
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
        private List<IModuleViewModelBase> _viewModelsOfModules;
        /// <summary>
        /// List of viewmodels which are used by page modules that display its content
        /// </summary>
        public List<IModuleViewModelBase> ViewModelsForModules
        {
            get => _viewModelsOfModules;
            set => Set(ref _viewModelsOfModules, value, nameof(ViewModelsForModules));
        }
        #endregion


        #region Ctor
        public ModularPageViewModel(ILoadPageService loadPageService)
        {
            this._loadPageService = loadPageService;
            this.ViewModelsForModules = new List<IModuleViewModelBase>();
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
                                                   .CreateViewModels();
        }
        #endregion
    }
}
