using System;
using System.Collections.Generic;

#nullable disable

namespace TrzyszczCMS.Core.Infrastructure.Models.Database
{
    public partial class VersionInfo
    {
        public long Version { get; set; }
        public DateTime? AppliedOn { get; set; }
        public string Description { get; set; }
    }
}
