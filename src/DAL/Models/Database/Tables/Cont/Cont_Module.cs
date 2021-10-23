namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// The class describing a single module that is placed on the page.
    /// </summary>
    public class Cont_Module
    {
        /// <summary>
        /// Row ID and the type of module ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Type of the module
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Row ID expressing which page it is referring to.
        /// </summary>
        public int Cont_PageId { get; set; }
    }
}
