using System.ComponentModel.DataAnnotations;

namespace PostHubAPI.Dtos.Comment;

public class CreateCommentDto
{
    [Required]
    [StringLength(80)]
    public string Body { get; set; } = string.Empty;
}