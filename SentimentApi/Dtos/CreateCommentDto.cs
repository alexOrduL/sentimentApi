namespace SentimentApi.Dtos
{
    public class CreateCommentDto
    {
        public string ProductId { get; set; } = null!;
        public string CommentText { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
