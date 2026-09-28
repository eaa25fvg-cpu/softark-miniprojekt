namespace Model;

public class Comment
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
    public required User User { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public required Post Post { get; set; }
}