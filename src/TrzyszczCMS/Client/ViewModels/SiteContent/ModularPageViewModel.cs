using Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // TODO: DB service
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
        public ModularPageViewModel()
        {
            
        }
        #endregion


        #region Methods
        /// <summary>
        /// Method loading data modules for displaying its content.
        /// </summary>
        /// <param name="type">Type of the requested page</param>
        /// <param name="name">Name of the requested page</param>
        public void PrepareModules(PageType type, string name)
        {
            // TODO: Assignment for development purposes only.
            this.ViewModelsForModules = new List<IModuleViewModelBase>()
            {
                new TextWallModuleViewModel() { SectionMarkDown = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.", SectionWidth = TextWallSectionWidth._1200 },
                new TextWallModuleViewModel() { SectionMarkDown = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.", SectionWidth = TextWallSectionWidth._1000 },
                new TextWallModuleViewModel() { SectionMarkDown = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.", SectionWidth = TextWallSectionWidth._800 },
                new TextWallModuleViewModel() { SectionMarkDown = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.", SectionWidth = TextWallSectionWidth._600 },
            };
        }
        #endregion
    }
}
