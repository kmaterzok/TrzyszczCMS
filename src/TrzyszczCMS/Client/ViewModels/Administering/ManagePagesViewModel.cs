using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.ViewModels.Shared;
using TrzyszczCMS.Client.Views.Administering;

namespace TrzyszczCMS.Client.ViewModels.Administering
{
    /// <summary>
    /// Viewmodel containing data for displaying in <see cref="ManagePages"/> view.
    /// </summary>
    public class ManagePagesViewModel : ViewModelBase
    {
        #region Fields
        #endregion

        #region Properties
        private List<GridItem<string>> _posts;
        /// <summary>
        /// All posts currently displayed in the posts grid.
        /// </summary>
        public List<GridItem<string>> Posts
        {
            get => _posts;
            set => Set(ref _posts, value, nameof(Posts));
        }
        #endregion

        #region Ctor
        public ManagePagesViewModel()
        {
            this.Posts = new List<GridItem<string>>()
            {
                new GridItem<string>()
                {
                    Data = "Aperiam est sit eos voluptas cum repellat sed. Totam optio nisi animi vel beatae ab quisquam minima. Aspernatur atque quas velit aut magnam in nulla quidem. Itaque enim iure dignissimos.",
                    Checked = true
                },
                new GridItem<string>()
                {
                    Data = "Quia enim aliquam nostrum. Nesciunt aut dolorem libero. Dicta deleniti ut adipisci repudiandae qui consequuntur commodi ea. Facere fuga inventore quo. Consequatur corrupti alias hic ullam ducimus accusamus eum dolore.",
                    Checked = true
                },
                new GridItem<string>()
                {
                    Data = "Aut unde quisquam maiores in fugiat enim quia veniam. Quisquam non repellat rerum ipsam odio pariatur omnis voluptate. Ut maxime aut laboriosam quia nihil sapiente molestiae. Vitae sed sit rerum cumque odit libero nihil et. Minus culpa et blanditiis eum. Et tempora eaque et cum ratione.",
                    Checked = false
                },
                new GridItem<string>()
                {
                    Data = "Ex repellat atque vero mollitia voluptatum. Corrupti aut ut non nobis non aut blanditiis quo. Et perspiciatis ipsam iusto iste quisquam quas voluptatem. Beatae fugit cupiditate mollitia nulla.",
                    Checked = false
                },
                new GridItem<string>()
                {
                    Data = "Distinctio voluptatibus ipsam quae. Officiis repellat est veniam. Ipsam accusamus quo facilis mollitia dolor eos.",
                    Checked = true
                }
            };
        }
        #endregion

        #region Methods
        public void LoadPosts()
        {

        }

        public void LoadArticles()
        {

        }

        public  void OnPublisherSearchTextChanged(ChangeEventArgs changeEventArgs, string columnTitle)
        {

        }
        #endregion
    }
}
