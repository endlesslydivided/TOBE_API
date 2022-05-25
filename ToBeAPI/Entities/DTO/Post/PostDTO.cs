namespace ToBeApi.Data.DTO
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }

    }
}
