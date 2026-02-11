using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostHubAPI.Models;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; } = DateTime.Now;

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
}