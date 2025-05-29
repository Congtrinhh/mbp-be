using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Admin.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure default schema
            modelBuilder.HasDefaultSchema("public");

            // Configure base entity properties for all entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Configure base entity properties with snake_case column names
                    modelBuilder.Entity(entityType.ClrType).Property("Id").HasColumnName("id");
                    modelBuilder.Entity(entityType.ClrType).Property("IsActive").HasColumnName("is_active");
                    modelBuilder.Entity(entityType.ClrType).Property("CreatedAt").HasColumnName("created_at");
                    modelBuilder.Entity(entityType.ClrType).Property("ModifiedAt").HasColumnName("modified_at");
                    modelBuilder.Entity(entityType.ClrType).Property("CreatedBy").HasColumnName("created_by");
                    modelBuilder.Entity(entityType.ClrType).Property("ModifiedBy").HasColumnName("modified_by");
                }
            }

            // User configuration
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("user");
                builder.HasKey(e => e.Id);
                
                builder.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(100).IsRequired();
                builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
                builder.HasIndex(e => e.Email).IsUnique();
                builder.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                builder.Property(e => e.IsMc).HasColumnName("is_mc").IsRequired();
                builder.Property(e => e.Age).HasColumnName("age");
                builder.Property(e => e.NickName).HasColumnName("nick_name").HasMaxLength(50);
                builder.Property(e => e.Gender).HasColumnName("gender").IsRequired();
                builder.Property(e => e.IsNewbie).HasColumnName("is_newbie").IsRequired();
                builder.Property(e => e.WorkingArea).HasColumnName("working_area").HasMaxLength(200);
                builder.Property(e => e.IsVerified).HasColumnName("is_verified").IsRequired();
                builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(1000);
                builder.Property(e => e.Education).HasColumnName("education").HasMaxLength(200);
                builder.Property(e => e.Height).HasColumnName("height").HasPrecision(5, 2);
                builder.Property(e => e.Weight).HasColumnName("weight").HasPrecision(5, 2);
                builder.Property(e => e.AvatarUrl).HasColumnName("avatar_url").HasMaxLength(500);
                builder.Property(e => e.Facebook).HasColumnName("facebook").HasMaxLength(200);
                builder.Property(e => e.Zalo).HasColumnName("zalo").HasMaxLength(20);
            });

            // Post configuration
            modelBuilder.Entity<Post>(builder =>
            {
                builder.ToTable("post");
                builder.HasKey(e => e.Id);

                builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                builder.Property(e => e.Caption).HasColumnName("caption").HasMaxLength(500).IsRequired(false);
                builder.Property(e => e.PostGroup).HasColumnName("post_group").IsRequired();
                builder.Property(e => e.Place).HasColumnName("place").HasMaxLength(200).IsRequired();
                builder.Property(e => e.EventName).HasColumnName("event_name").HasMaxLength(200).IsRequired();
                builder.Property(e => e.EventStart).HasColumnName("event_start").IsRequired();
                builder.Property(e => e.EventEnd).HasColumnName("event_end").IsRequired();
                builder.Property(e => e.PriceFrom).HasColumnName("price_from").HasPrecision(10, 2);
                builder.Property(e => e.PriceTo).HasColumnName("price_to").HasPrecision(10, 2);
                builder.Property(e => e.McRequirement).HasColumnName("mc_requirement").HasMaxLength(1000).IsRequired();
                builder.Property(e => e.DetailDesc).HasColumnName("detail_desc").HasMaxLength(2000).IsRequired(false);

                // Configure foreign key relationship with User
                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}