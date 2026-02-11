using PostHubAPI.Dtos.Comment;

namespace PostHubAPI.Dtos.Post;

public class ReadPostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
    public int Likes { get; set; }

    public IList<ReadCommentDto>? Comments { get; set; }
}