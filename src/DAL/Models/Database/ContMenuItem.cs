using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models.Database
{
    public partial class ContMenuItem
    {
        public ContMenuItem()
        {
            InverseParentItem = new HashSet<ContMenuItem>();
        }

        public int Id { get; set; }
        public int? ParentItemId { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public int OrderNumber { get; set; }

        public virtual ContMenuItem ParentItem { get; set; }
        public virtual ICollection<ContMenuItem> InverseParentItem { get; set; }
    }
}
