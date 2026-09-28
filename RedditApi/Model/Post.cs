namespace Model;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTime Date { get; set; }
    public required User User { get; set; }
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
}