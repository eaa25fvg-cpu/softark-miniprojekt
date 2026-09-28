namespace Model;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public required User User { get; set; }
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
}