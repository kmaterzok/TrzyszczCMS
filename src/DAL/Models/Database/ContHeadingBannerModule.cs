using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models.Database
{
    public partial class ContHeadingBannerModule
    {
        public int Id { get; set; }
        public bool? DisplayDescription { get; set; }
        public bool? DisplayAuthorsInfo { get; set; }
        public bool DarkDescription { get; set; }
        public short ViewportHeight { get; set; }
        public bool? AttachLinkMenu { get; set; }
        public int? BackgroundPictureId { get; set; }

        public virtual ContFile BackgroundPicture { get; set; }
        public virtual ContModule IdNavigation { get; set; }
    }
}
