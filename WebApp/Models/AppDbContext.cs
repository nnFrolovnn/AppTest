using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApp.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext()
            : base("WebAppDb", throwIfV1Schema: false)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // item - category many to many
            modelBuilder.Entity<ItemCategory>()
                .HasKey(x => new { x.ItemId, x.CategoryId });

            modelBuilder.Entity<ItemCategory>()
                .HasRequired(x => x.Item)
                .WithMany(m => m.Categories)
                .HasForeignKey(x => x.ItemId);

            modelBuilder.Entity<ItemCategory>()
                .HasRequired(x => x.Category)
                .WithMany(m => m.Items)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}