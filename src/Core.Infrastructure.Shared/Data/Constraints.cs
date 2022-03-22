namespace TrzyszczCMS.Core.Infrastructure.Shared.Data
{
    /// <summary>
    /// This class contains all currently used constraints
    /// deployed in the database alongside the frontend.
    /// </summary>
    public static class Constraints
    {
        public static class AuthUser
        {
            public const int USERNAME    =  40;
            public const int DESCRIPTION = 250;
        }
        public static class ContMenuItem
        {
            public const int NAME = 250;
            public const int URI  = 250;
        }
        public static class ContPage
        {
            public const int URI_NAME     = 255;
            public const int TITLE        = 255;
            public const int AUTHORS_INFO = 255;
            public const int DESCRIPTION  = 255;
        }
        public static class ContFile
        {
            public const int NAME = 250;
        }
    }
}
