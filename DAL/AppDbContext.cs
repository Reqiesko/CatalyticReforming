
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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                //.UseLazyLoadingProxies()
                .UseSqlite("Data Source=Databases/DataBase.db")
                .EnableSensitiveDataLogging(true)
                .LogTo(s => Debug.WriteLine(s));
        }
    }
}
