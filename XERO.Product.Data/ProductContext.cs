using Microsoft.EntityFrameworkCore;

namespace Xero.Product.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductData>()
                .HasMany(b => b.Options)
                .WithOne(p => p.Product)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<ProductData> Product { get; set; }
        public DbSet<ProductOption> ProductOption { get; set; }
    }
}
