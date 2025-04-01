using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogApi.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure many-to-many relationship between Post and Tag
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Tags)
            .WithMany(t => t.Posts)
            .UsingEntity(j => j.ToTable("PostTags"));

        // Create sample data with fixed dates instead of DateTime.UtcNow
        modelBuilder.Entity<Post>().HasData(
            new Post
            {
                Id = 1,
                Title = "Getting Started with ASP.NET 9",
                Content = "ASP.NET 9 offers many new features...",
                Summary = "An introduction to ASP.NET 9",
                IsPublished = true,
                CreatedAt = new DateTime(2025, 4, 1, 12, 0, 0, DateTimeKind.Utc) // Fixed date
            }
        );

        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "ASP.NET" },
            new Tag { Id = 2, Name = "C#" },
            new Tag { Id = 3, Name = "Web API" }
        );
    }

    // Add this method to suppress the warning if desired
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));

        base.OnConfiguring(optionsBuilder);
    }
}