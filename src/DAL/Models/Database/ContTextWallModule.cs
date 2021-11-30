using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models.Database
{
    public partial class ContTextWallModule
    {
        public int Id { get; set; }
        public string LeftAsideContent { get; set; }
        public string RightAsideContent { get; set; }
        public string SectionContent { get; set; }
        public short SectionWidth { get; set; }

        public virtual ContModule IdNavigation { get; set; }
    }
}
