namespace DAL.Migrations
{
    /// <summary>
    /// This class contains all foreign keys that have evver existed.
    /// </summary>
    internal static class ForeignKeys
    {
        /// <summary>
        /// Names of all currently used foreign keys.
        /// Keys that are going to be unused must be moved to <see cref="Obsolete"/> class.
        /// </summary>
        public static class Current
        {
            public const string AUTHTOKEN_AUTHUSER_ASSIGNEDUSERID
                = "AuthToken_AuthUser_AssignedUserId";

            public const string AUTHROLEPOLICYASSIGN_AUTHROLE_ASSIGNEDROLEID
                = "AuthRolePolicyAssign_AuthRole_AssignedRoleId";

            public const string AUTHROLEPOLICYASSIGN_AUTHPOLICY_ASSIGNEDPOLICYID
                = "AuthRolePolicyAssign_AuthPolicy_AssignedPolicyId";

            public const string AUTHUSER_AUTHROLE_ASSIGNEDROLEID
                = "AuthUser_AuthRole_AssignedRoleId";

            public const string CONTTEXTWALLMODULEID_CONTMODULEID_ASSIGNEDMODULEID
                = "ContTextWallModuleId_ContModuleId_AssignedModuleId";

            public const string CONTMODULE_PAGE_ASSIGNEDID
                = "ContModule_Page_AssignedId";

            public const string CONTFILEID_CONTFILEID_PARENTFILEID
                = "ContFile_ContFile_ParentFileId";

            public const string CONTMENUITEMID_CONTMENUITEMID_PARENTITEMID
                = "ContMenuItem_ContMenuItem_ParentItemId";
        }
        /// <summary>
        /// Names of all obsolete (unused) foreign keys.
        /// </summary>
        public static class Obsolete
        {
            // There has been no obsolete foreign key thus far.
        }
    }
}
