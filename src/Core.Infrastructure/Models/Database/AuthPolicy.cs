using System;
using System.Collections.Generic;

#nullable disable

namespace TrzyszczCMS.Core.Infrastructure.Models.Database
{
    public partial class AuthPolicy
    {
        public AuthPolicy()
        {
            AuthRolePolicyAssigns = new HashSet<AuthRolePolicyAssign>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AuthRolePolicyAssign> AuthRolePolicyAssigns { get; set; }
    }
}
