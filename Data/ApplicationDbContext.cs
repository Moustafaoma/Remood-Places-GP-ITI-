using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Remood_Place.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Remood_Place.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure UserName property as required
            modelBuilder.Entity<AppUser>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256); // Optionally, you can set max length

            // Configure Email property as required and with email address validation
            modelBuilder.Entity<AppUser>()
                .Property(u => u.Email)
                .IsRequired()
                .HasAnnotation("EmailAddress", true);

            //// Configure PhoneNumber property as required
            //modelBuilder.Entity<AppUser>()
            //    .Property(u => u.PhoneNumber)
            //    .IsRequired();

            // Configure PasswordHash property as required
            modelBuilder.Entity<AppUser>()
                .Property(u => u.PasswordHash)
                .IsRequired();



            //---------------

            // Your model configuration here
            modelBuilder.Entity<Rate>().HasKey(key => new { key.PostId, key.UserId });
            modelBuilder.Entity<Favorite>().HasKey(key => new { key.PostId, key.UserId });
            modelBuilder.Entity<Like>().HasKey(key => new { key.PostId, key.UserId });

            //modelBuilder.Entity<Post_images>().HasKey(key => new { key.PostId, key.Image });
            //new----------
            modelBuilder.Entity<PackagePosts>().HasKey(key => new { key.PostId, key.PackageId,key.orderposts });
            modelBuilder.Entity<PackageComment>();
            modelBuilder.Entity<PackageFavourit>().HasKey(key => new { key.PackageId, key.UserId });
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Likes)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>();
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Rates)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Rates)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Favorites)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Favorites)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Comments)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //new--------------------------------
            modelBuilder.Entity<Package>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.packages)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Package>()
                .HasMany(p => p.packageFavourits)
                .WithOne(r => r.Package)
                .HasForeignKey(r => r.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.PackageFavourit)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Package>()
                .HasMany(p => p.packageComments)
                .WithOne(r => r.Package)
                .HasForeignKey(r => r.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.PackageComment)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
    .HasOne(p => p.AppUser)
    .WithMany(u => u.posts)
    .HasForeignKey(p => p.UserId)
    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Favorites)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Favorites)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Comments)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                },
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                },
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "user",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                });


            base.OnModelCreating(modelBuilder);
        }

        internal object UsersWhere(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        // DbSets for your entities
        public virtual DbSet<Post> Posts { get; set; }
        //public virtual DbSet<Post_images> PostImages { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Like> Likes { get; set; }

        //new----------
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<PackagePosts> PackagePosts { get; set; }
        public virtual DbSet<PackageComment> PackageComment { get; set; }
        public virtual DbSet<PackageFavourit> PackageFavourits { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }


    }
}
