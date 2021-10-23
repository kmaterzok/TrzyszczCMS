namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// The class representing Text Wall module.
    /// </summary>
    public class Cont_TextWallModule
    {
        /// <summary>
        ///  Row ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The content for the left aside block.
        /// </summary>
        public string LeftAsideContent { get; set; }
        /// <summary>
        /// The content for the main block displaying crucial text.
        /// </summary>
        public string RightAsideContent { get; set; }
        /// <summary>
        /// The content for the right aside block.
        /// </summary>
        public string SectionContent { get; set; }
        /// <summary>
        /// Main content section width. Expressed as enum's value.
        /// </summary>
        public short SectionWidth { get; set; }
    }
}
