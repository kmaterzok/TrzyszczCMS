using System;
using System.Collections.Generic;

#nullable disable

namespace TrzyszczCMS.Core.Infrastructure.Models.Database
{
    public partial class AuthRole
    {
        public AuthRole()
        {
            AuthRolePolicyAssigns = new HashSet<AuthRolePolicyAssign>();
            AuthUsers = new HashSet<AuthUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool FactoryRole { get; set; }

        public virtual ICollection<AuthRolePolicyAssign> AuthRolePolicyAssigns { get; set; }
        public virtual ICollection<AuthUser> AuthUsers { get; set; }
    }
}
