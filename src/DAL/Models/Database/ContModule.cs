using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models.Database
{
    public partial class ContModule
    {
        public int Id { get; set; }
        public short Type { get; set; }
        public int ContPageId { get; set; }

        public virtual ContPage ContPage { get; set; }
        public virtual ContTextWallModule ContTextWallModule { get; set; }
    }
}
