using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models.Database
{
    public partial class ContPage
    {
        public ContPage()
        {
            ContModules = new HashSet<ContModule>();
        }

        public int Id { get; set; }
        public string UriName { get; set; }
        public string Title { get; set; }
        public short Type { get; set; }
        public DateTime CreateUtcTimestamp { get; set; }
        public DateTime PublishUtcTimestamp { get; set; }
        public string AuthorsInfo { get; set; }

        public virtual ICollection<ContModule> ContModules { get; set; }
    }
}
