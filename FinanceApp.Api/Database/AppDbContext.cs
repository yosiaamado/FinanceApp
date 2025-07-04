using FinanceApp.Api.Helper;
using FinanceApp.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get;set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<TempItem> TempItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "admin@senco.com".ToLower(), //bisa dommo
                Password = EncryptionHelper.Hash("admin123"),
                Username = "Admin",
                Role = "Admin"
            });

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TempItem>()
                .HasOne(i => i.Category)
                .WithMany(c => c.TempItems)
                .HasForeignKey(i => i.CategoryId);

            modelBuilder.Entity<Transaction>()
                .HasOne(i => i.Item)
                .WithMany(c => c.Transactions)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
