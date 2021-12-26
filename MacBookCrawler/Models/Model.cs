using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MacBookCrawler.Models
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ImageURL)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Link)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Firm)
                .IsUnicode(false);
        }
    }
}
