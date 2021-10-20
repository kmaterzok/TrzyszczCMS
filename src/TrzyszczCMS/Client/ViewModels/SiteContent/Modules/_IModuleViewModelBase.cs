using Core.Application.Enums;

namespace TrzyszczCMS.Client.ViewModels.SiteContent.Modules
{
    /// <summary>
    /// The interface for viewmodels that apply for modules of parts of pages.
    /// </summary>
    public interface IModuleViewModelBase
    {
        /// <summary>
        /// The kind of module which the viewmodel holds data for.
        /// </summary>
        public PageModuleType ModuleType { get; }
    }
}
