using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.EntityFrameworkCore;


namespace DAL;

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
            .UseLazyLoadingProxies()
            .UseSqlite("Data Source=DataBase.db")
            .EnableSensitiveDataLogging()
            .LogTo(s => Debug.WriteLine(s));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>()
                    .HasData(new UserRole()
                    {
                        Id = 1,
                        Name = "Admin"
                    });
        modelBuilder.Entity<UserRole>()
                    .HasData(new UserRole()
                    {
                        Id = 2,
                        Name = "User"
                    });
        modelBuilder.Entity<User>()
                    .HasData(new User
                    {
                        Id = 1,
                        Password = "u",
                        Username = "u",
                        RoleId = 2,
                        Access = false,
                    });

        modelBuilder.Entity<User>()
                    .HasData(new User
                    {
                        Id = 2,
                        Password = "a",
                        Username = "a",
                        RoleId = 1,
                        Access = true,
                    });

        modelBuilder.Entity<Question>()
                    .HasData(new Question
                    {
                        Id = 1,
                        Text = "текст вопроса",
                    });

        modelBuilder.Entity<Answer>()
                    .HasData(new Answer
                    {
                        Id = 1,
                        IsCorrect = true,
                        QuestionId = 1,
                        Text = "ответ 1",
                    });

        modelBuilder.Entity<Answer>()
                    .HasData(new Answer
                    {
                        Id = 2,
                        IsCorrect = false,
                        QuestionId = 1,
                        Text = "ответ 2",
                    });
    }
}
