using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/posts/{postId}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly BlogDbContext _context;

    public CommentsController(BlogDbContext context)
    {
        _context = context;
    }

    // GET: api/posts/5/comments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int postId)
    {
        if (!await PostExists(postId))
        {
            return NotFound();
        }

        return await _context.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    // GET: api/posts/5/comments/3
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(int postId, int id)
    {
        if (!await PostExists(postId))
        {
            return NotFound();
        }

        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.PostId == postId && c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        return comment;
    }

    // POST: api/posts/5/comments
    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment(int postId, Comment comment)
    {
        if (!await PostExists(postId))
        {
            return NotFound();
        }

        comment.PostId = postId;
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComment),
            new { postId = postId, id = comment.Id },
            comment);
    }

    // PUT: api/posts/5/comments/3
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int postId, int id, Comment comment)
    {
        if (id != comment.Id || postId != comment.PostId)
        {
            return BadRequest();
        }

        _context.Entry(comment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CommentExists(postId, id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/posts/5/comments/3
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int postId, int id)
    {
        if (!await PostExists(postId))
        {
            return NotFound();
        }

        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.PostId == postId && c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private Task<bool> PostExists(int id)
    {
        return _context.Posts.AnyAsync(p => p.Id == id);
    }

    private Task<bool> CommentExists(int postId, int id)
    {
        return _context.Comments.AnyAsync(c => c.PostId == postId && c.Id == id);
    }
}