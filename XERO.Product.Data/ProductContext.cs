using Microsoft.EntityFrameworkCore;

namespace Xero.Product.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
           : base(options)
        {
        }
        public DbSet<ProductData> Product { get; set; }
        public DbSet<ProductOption> ProductOption { get; set; }



    }
}
