using System;
using System.Collections.Generic;

#nullable disable

namespace TrzyszczCMS.Core.Infrastructure.Models.Database
{
    public partial class ContFile
    {
        public ContFile()
        {
            ContHeadingBannerModules = new HashSet<ContHeadingBannerModule>();
            InverseParentFile = new HashSet<ContFile>();
        }

        public int Id { get; set; }
        public int? ParentFileId { get; set; }
        public bool IsDirectory { get; set; }
        public DateTime CreationUtcTimestamp { get; set; }
        public string Name { get; set; }
        public Guid AccessGuid { get; set; }
        public string MimeType { get; set; }

        public virtual ContFile ParentFile { get; set; }
        public virtual ICollection<ContHeadingBannerModule> ContHeadingBannerModules { get; set; }
        public virtual ICollection<ContFile> InverseParentFile { get; set; }
    }
}
