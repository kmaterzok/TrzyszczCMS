using System;
using System.Collections.Generic;

#nullable disable

namespace TrzyszczCMS.Core.Infrastructure.Models.Database
{
    public partial class AuthRolePolicyAssign
    {
        public int AuthRoleId { get; set; }
        public int AuthPolicyId { get; set; }

        public virtual AuthPolicy AuthPolicy { get; set; }
        public virtual AuthRole AuthRole { get; set; }
    }
}
