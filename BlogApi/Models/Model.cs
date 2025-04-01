using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models;

public class Post
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }

    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsPublished { get; set; } = false;

    // Navigation properties
    public List<Comment> Comments { get; set; } = new();

    public List<Tag> Tags { get; set; } = new();
}

public class Comment
{
    public int Id { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    [StringLength(50)]
    public required string Author { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key
    public int PostId { get; set; }

    // Navigation property
    public Post? Post { get; set; }
}

public class Tag
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public required string Name { get; set; }

    // Navigation property
    public List<Post> Posts { get; set; } = new();
}