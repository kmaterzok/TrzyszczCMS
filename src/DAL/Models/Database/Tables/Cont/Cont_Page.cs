﻿using System;
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
        /// SEO friendly name of the page used in its URI.
        /// </summary>
        public string UriName { get; set; }
        /// <summary>
        /// Proper name of the page for display.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Page type
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// UTC+0 timestamp of creating the page.
        /// </summary>
        public DateTime CreateUtcTimestamp { get; set; }
        /// <summary>
        /// UTC+0 timestamp of the official publication on the website.
        /// </summary>
        public DateTime PublishUtcTimestamp { get; set; }



        public List<Cont_Module> ContModules { get; set; }
    }
}