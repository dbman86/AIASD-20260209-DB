namespace PostHubAPI.Dtos.Comment;

public class ReadCommentDto
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; } = DateTime.Now;
}