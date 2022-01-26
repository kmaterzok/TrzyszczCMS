using Core.Shared.Enums;

namespace TrzyszczCMS.Client.ViewModels.PageContent.Modules
{
    /// <summary>
    /// The interface for viewmodels that apply for modules of parts of pages.
    /// </summary>
    public interface IModuleViewModel
    {
        /// <summary>
        /// The kind of module which the viewmodel holds data for.
        /// </summary>
        public PageModuleType ModuleType { get; }
    }
}
