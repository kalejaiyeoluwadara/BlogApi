using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

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

        // Create sample data
        modelBuilder.Entity<Post>().HasData(
            new Post
            {
                Id = 1,
                Title = "Getting Started with ASP.NET 9",
                Content = "ASP.NET 9 offers many new features...",
                Summary = "An introduction to ASP.NET 9",
                IsPublished = true
            }
        );

        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "ASP.NET" },
            new Tag { Id = 2, Name = "C#" },
            new Tag { Id = 3, Name = "Web API" }
        );
    }
}