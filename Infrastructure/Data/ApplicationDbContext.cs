using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<McType> McTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<HostingStyle> HostingStyles { get; set; }
        public DbSet<ClientReviewMc> ClientReviewMcs { get; set; }
        public DbSet<McReviewClient> McReviewClients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<IdInfo> IdInfos { get; set; }
        public DbSet<UserIdVerification> UserIdVerifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<McType>().ToTable("mc_type");
            modelBuilder.Entity<Province>().ToTable("province");
            modelBuilder.Entity<HostingStyle>().ToTable("hosting_style");
            modelBuilder.Entity<ClientReviewMc>().ToTable("client_review_mc");
            modelBuilder.Entity<McReviewClient>().ToTable("mc_review_client");
            modelBuilder.Entity<Contract>().ToTable("contract");
            modelBuilder.Entity<Media>().ToTable("media");
            modelBuilder.Entity<Post>().ToTable("post");
            modelBuilder.Entity<Reaction>().ToTable("reaction");
            modelBuilder.Entity<Notification>().ToTable("notification");
            modelBuilder.Entity<IdInfo>().ToTable("id_info");
            modelBuilder.Entity<UserIdVerification>().ToTable("user_id_verification");

            // Configure many-to-many relationship between User and McType
            modelBuilder.Entity<User>()
                .HasMany(u => u.McTypes)
                .WithMany(t => t.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "mc_mc_type",
                    j => j
                        .HasOne<McType>()
                        .WithMany()
                        .HasForeignKey("mc_type_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("mc_id")
                        .OnDelete(DeleteBehavior.Cascade));

            // Configure many-to-many relationship between User and Province
            modelBuilder.Entity<User>()
                .HasMany(u => u.Provinces)
                .WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "mc_province",
                    j => j
                        .HasOne<Province>()
                        .WithMany()
                        .HasForeignKey("province_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("mc_id")
                        .OnDelete(DeleteBehavior.Cascade));

            // Configure many-to-many relationship between User and HostingStyle
            modelBuilder.Entity<User>()
                .HasMany(u => u.HostingStyles)
                .WithMany(h => h.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "mc_hosting_style",
                    j => j
                        .HasOne<HostingStyle>()
                        .WithMany()
                        .HasForeignKey("hosting_style_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("mc_id")
                        .OnDelete(DeleteBehavior.Cascade));

            // Global snake_case naming convention
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.GetColumnName()));
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(ToSnakeCase(key.GetName()));
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(ToSnakeCase(key.GetConstraintName()));
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
                }
            }
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = System.Text.RegularExpressions.Regex.Match(input, @"^_+");
            return startUnderscores + System.Text.RegularExpressions.Regex.Replace(input.Substring(startUnderscores.Length), @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
