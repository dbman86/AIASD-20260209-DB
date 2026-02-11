using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostHubAPI.Dtos.Comment;
using PostHubAPI.Exceptions;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CommentController(ICommentService commentService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(int id)
    {
        try
        {
            var comment = await commentService.GetCommentAsync(id);
            return Ok(comment);
        }
        catch (NotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost("{postId}")]
    public async Task<IActionResult> CreateNewComment(int postId, [FromBody] CreateCommentDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var newCommentId = await commentService.CreateNewCommnentAsync(postId, dto);
                var locationUri = $"{Request.Scheme}://{Request.Host}/api/Comment/{newCommentId}";
                return Created(locationUri, newCommentId);
            }

            return BadRequest(ModelState);
        }
        catch (NotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditComment(int id, [FromBody] EditCommentDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var editedComment = await commentService.EditCommentAsync(id, dto);
                return Ok(editedComment);
            }

            return BadRequest(ModelState);
        }
        catch (NotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        try
        {
            await commentService.DeleteCommentAsync(id);
            return NoContent();
        }
        catch (NotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}