namespace MacBookCrawler.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public int ProductId { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Price { get; set; }

        [StringLength(500)]
        public string ImageURL { get; set; }

        [StringLength(500)]
        public string Link { get; set; }

        [StringLength(500)]
        public string Firm { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
