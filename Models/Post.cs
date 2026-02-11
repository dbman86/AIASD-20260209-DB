using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PostHubAPI.Models;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreationTime { get; } = DateTime.Now;
    public int Likes { get; set; } = 0;

    public IList<Comment>? Comments { get; set; }
}