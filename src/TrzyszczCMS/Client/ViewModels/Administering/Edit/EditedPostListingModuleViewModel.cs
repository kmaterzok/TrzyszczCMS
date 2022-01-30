using Core.Shared.Enums;
using Core.Shared.Models.PageContent;
using System;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    public class EditedPostListingModuleViewModel : ViewModelBase
    {
        #region Fields
        /// <summary>
        /// Reference on an object containing edited data.
        /// </summary>
        private PostListingModuleContent _postListingModuleContent;
        #endregion

        #region Properties :: General
        /// <summary>
        /// Fired when all data is proper and a user wants to exit heading banner editor.
        /// </summary>
        public Action OnExitingEditor { get; set; }

        /// <summary>
        /// Fired when any displayed data are modified.
        /// </summary>
        public EventHandler OnDataSet { get; set; }

        /// <summary>
        /// Width of the displayed element on the page
        /// </summary>
        public PostListingWidth PostListingWidth
        {
            get => this._postListingModuleContent.Width;
            set
            {
                this._postListingModuleContent.Width = value;
                PropertyValueChanged(nameof(PostListingWidth));
            }
        }
        #endregion

        #region Ctor
        public EditedPostListingModuleViewModel(PostListingModuleContent postListingModuleContent) =>
            this._postListingModuleContent = postListingModuleContent;
        #endregion

        #region Methods
        public void Exit() => this.OnExitingEditor?.Invoke();
        #endregion

        #region Helper methods
        /// <summary>
        /// Notify UI thread about change of the property
        /// and invoke action responsible for saving changes periodically.
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        private void PropertyValueChanged(string propertyName)
        {
            NotifyPropertyChanged(propertyName);
            OnDataSet?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
