using BOTY.Models.Tables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BOTY.Models
{
    public class MyContext : DbContext
    {
        public DbSet<Address> addresses { get; set; }
        public DbSet<Category> catergories { get; set; }
        public DbSet<Color> colors { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<logins> logins { get; set; }
        public DbSet<Manufacturer> manufacturers { get; set; }
        public DbSet<Material> materials { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<Size> sizes { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<Variant> variants { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<logins>().HasNoKey().ToView("logins");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=4b1_mrazfilip_db2;user=mrazfilip;password=123456;SslMode=none");
        }
    }
}
