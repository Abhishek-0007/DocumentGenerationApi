using DocumentGenerationApi.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DocumentGenerationApi.DAL.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("userTbl");

            modelBuilder.Entity<Document>()
                .ToTable("DocumentTemplate");

            modelBuilder.Entity<SaveDocument>()
                .ToTable("documentTbl");
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<SaveDocument> SavedDocuments { get; set; } = null!;

    }
}
