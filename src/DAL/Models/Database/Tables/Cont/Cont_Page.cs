using System;
using System.Collections.Generic;

namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// The class containing generic page data in the database.
    /// </summary>
    public class Cont_Page
    {
        /// <summary>
        /// Row ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SEO friendly name of the page.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Page type
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// UTC+0 timestamp of creating the page.
        /// </summary>
        public DateTime CreateUtcTimestamp { get; set; }


        public List<Cont_Module> ContModules { get; set; }
    }
}
