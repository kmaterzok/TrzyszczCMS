using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models.Database
{
    public partial class AuthToken
    {
        public int Id { get; set; }
        public int AuthUserId { get; set; }
        public byte[] HashedToken { get; set; }
        public DateTime UtcCreateTime { get; set; }
        public DateTime UtcExpiryTime { get; set; }

        public virtual AuthUser AuthUser { get; set; }
    }
}
