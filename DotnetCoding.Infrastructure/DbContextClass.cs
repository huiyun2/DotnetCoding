using Microsoft.EntityFrameworkCore;
using DotnetCoding.Core.Models;

namespace DotnetCoding.Infrastructure
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> contextOptions) : base(contextOptions)
        {
        }

        // DbSet for ProductDetails
        public DbSet<ProductDetails> Products { get; set; }

        // DbSet for ApprovalQueue
        public DbSet<ApprovalQueue> ApprovalQueue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring ProductDetails entity
            modelBuilder.Entity<ProductDetails>(entity =>
            {
                entity.HasKey(p => p.Id); // Primary key
                entity.Property(p => p.ProductName).IsRequired().HasMaxLength(255);
                entity.Property(p => p.ProductDescription).HasMaxLength(500);
                entity.Property(p => p.ProductPrice).IsRequired();
                entity.Property(p => p.ProductStatus).IsRequired();
                entity.Property(p => p.PostedDate).IsRequired();
            });

            // Configuring ApprovalQueue entity
            modelBuilder.Entity<ApprovalQueue>(entity =>
            {
                entity.HasKey(a => a.Id); // Primary key
                entity.Property(a => a.RequestReason).HasMaxLength(500);
                entity.Property(a => a.RequestDate).IsRequired();
                entity.Property(a => a.State).IsRequired();
                // Configuring foreign key with ProductDetails
                entity.HasOne<ProductDetails>()
                      .WithMany() // No navigation property required in ProductDetails
                      .HasForeignKey(a => a.ProductId)
                      .OnDelete(DeleteBehavior.Cascade); // Delete approval queue items if the product is deleted
            });
        }
    }
}
