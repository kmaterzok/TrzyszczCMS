namespace TrzyszczCMS.Core.Infrastructure.Shared.Data
{
    /// <summary>
    /// The names of policies used in the CMS.
    /// </summary>
    public static class UserPolicies
    {
        public const string HOMEPAGE_EDITING       = "PolicyHomePageEditing";

        public const string BLOG_POST_CREATING     = "PolicyBlogPostCreating";
        public const string BLOG_POST_EDITING      = "PolicyBlogPostEditing";
        public const string BLOG_POST_DELETING     = "PolicyBlogPostDeleting";

        public const string ARTICLE_CREATING       = "PolicyArticleCreating";
        public const string ARTICLE_EDITING        = "PolicyArticleEditing";
        public const string ARTICLE_DELETING       = "PolicyArticleDeleting";

        public const string ANY_USER_CREATING      = "PolicyAnyUserCreating";
        public const string ANY_USER_EDITING       = "PolicyAnyUserEditing";
        public const string ANY_USER_DELETING      = "PolicyAnyUserDeleting";

        public const string MANAGE_NAVIGATION_BAR  = "PolicyManageNavigationBar";

        public const string FILE_ADDING            = "PolicyFileAdding";
        public const string FILE_DELETING          = "PolicyFileDeleting";
    }
}
