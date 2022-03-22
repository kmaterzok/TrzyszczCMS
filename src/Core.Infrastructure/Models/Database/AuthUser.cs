using System;
using System.Collections.Generic;

#nullable disable

namespace TrzyszczCMS.Core.Infrastructure.Models.Database
{
    public partial class AuthUser
    {
        public AuthUser()
        {
            AuthTokens = new HashSet<AuthToken>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Argon2Iterations { get; set; }
        public int Argon2MemoryCost { get; set; }
        public int Argon2Parallelism { get; set; }
        public int AuthRoleId { get; set; }

        public virtual AuthRole AuthRole { get; set; }
        public virtual ICollection<AuthToken> AuthTokens { get; set; }
    }
}
