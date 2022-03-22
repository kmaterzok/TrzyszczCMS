using System;
using System.Collections.Generic;

#nullable disable

namespace TrzyszczCMS.Core.Infrastructure.Models.Database
{
    public partial class ContPostListingModule
    {
        public int Id { get; set; }
        public short Width { get; set; }

        public virtual ContModule IdNavigation { get; set; }
    }
}
