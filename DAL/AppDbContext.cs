
using System.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                //.UseLazyLoadingProxies()
                .UseSqlite("Data Source=Databases/DataBase.db")
                .EnableSensitiveDataLogging(true)
                .LogTo(s => Debug.WriteLine(s));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>()
                        .HasData(new User()
                        {
                            Id = 1,
                            Password = "u",
                            Username = "u",
                            Role = UserRoles.User,
                            Access = false,
                        });
            modelBuilder.Entity<User>()
                        .HasData(new User()
                        {
                            Id = 2,
                            Password = "a",
                            Username = "a",
                            Role = UserRoles.Admin,
                            Access = true,
                        });
        }
    }
}
