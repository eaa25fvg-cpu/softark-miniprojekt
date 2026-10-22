using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using shared.Model;

using Data;
using shared.Model;

namespace Service;


public class DataService
{
    private PostContext db { get; }

    public DataService(PostContext db)
    {
        this.db = db;
    }

    public List<Post> GetPosts()
    {
        return db.Posts
            .Include(p => p.User)
            .Include(p => p.Comments).ThenInclude(c => c.User)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
    }

    public Post GetPost(int id)
    {
        return db.Posts
            .Include(p => p.User)
            .Include(p => p.Comments).ThenInclude(c => c.User)
            .FirstOrDefault(p => p.Id == id);
    }

    public Post UpvotePost(int id)
    {
        Post post = db.Posts.FirstOrDefault(p => p.Id == id);
        
        if (post is null)
            return null;

        post.Upvotes += 1;

        db.SaveChanges();

        return post;
    }
    
    public Post DownvotePost(int id)
    {
        Post post = db.Posts.FirstOrDefault(p => p.Id == id);
        
        if (post is null)
            return null;

        post.Downvotes += 1;

        db.SaveChanges();

        return post;
    }
    
    public Comment UpvoteComment(int postid, int commentid)
    {
        Comment comment = db.Comments.FirstOrDefault(c => c.Id == commentid && c.Post.Id == postid);
        
        if (comment is null)
            return null;

        comment.Upvotes += 1;

        db.SaveChanges();

        return comment;
    }
    
    public Comment DownvoteComment(int postid, int commentid)
    {
        Comment comment = db.Comments.FirstOrDefault(c => c.Id == commentid && c.Post.Id == postid);
        
        if (comment is null)
            return null;

        comment.Downvotes += 1;

        db.SaveChanges();

        return comment;
    }

    // Finder brugeren ud fra navnet, eller opretter en ny hvis den ikke findes.
    private User GetOrCreateUser(string username)
    {
        User user = db.Users.FirstOrDefault(u => u.Username == username);

        if (user is null)
        {
            user = new User(username);
            db.Users.Add(user);
        }

        return user;
    }

    public Post CreatePost(string title, string content, string url, string author)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
            return null;

        // En tråd er enten et link eller tekst
        if (string.IsNullOrWhiteSpace(url) == string.IsNullOrWhiteSpace(content))
            return null;

        User user = GetOrCreateUser(author);

        Post post = new Post(user, title, content ?? "", string.IsNullOrWhiteSpace(url) ? null : url);
        db.Posts.Add(post);

        db.SaveChanges();

        return post;
    }

    public Comment CreateComment(int postId, string content, string author)
    {
        Post post = db.Posts.FirstOrDefault(p => p.Id == postId);

        if (post is null || string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(author))
            return null;

        User user = GetOrCreateUser(author);

        Comment comment = new Comment(content, user: user);
        comment.Post = post;
        db.Comments.Add(comment);

        db.SaveChanges();

        return comment;
    }

    public void SeedData()
    {

        User user = db.Users.FirstOrDefault()!;
        // derfor list skal sættes!
        // user.Posts.Add(new Post { Title = "??", User = user });
        if (user == null)
        {
            user = new User { Username = "Kristian" };
            db.Users.Add(user);
            db.Users.Add(new User { Username = "Søren" });
            db.Users.Add(new User { Username = "Mette" });
        }

        Post post = db.Posts.FirstOrDefault()!;
        if (post == null)
        {
            post = new Post
            {
                Title = "Harry Potter", Content = "Harry potter er en god filmserie", User = user,
                Downvotes = 10, Upvotes = 1
            };
            db.Posts.Add(post);
            db.Posts.Add(new Post
            {
                Title = "Ringenes Herre", Content = "Ringenes Herre ved jeg ikke hvad er", User = user,
                Downvotes = 10, Upvotes = 1
            });
            db.Posts.Add(new Post
            {
                Title = "True detective", Content = "Bedste sæson TV nogensinde", User = user, 
                Downvotes = 10, Upvotes = 1
            });
        }
        
        Comment comment = db.Comments.FirstOrDefault()!;
        if (comment == null)
        {
            db.Comments.Add(new Comment
            {
                Content = "Dette er en kommentar", User = user, 
                Downvotes = 10, Upvotes = 1, Post = post
            });
            db.Comments.Add(new Comment
            {
                Content = "I like big men", User = user, 
                Downvotes = 10, Upvotes = 1, Post = post
            });
            db.Comments.Add(new Comment
            {
                Content = "Waddup", User = user, 
                Downvotes = 10, Upvotes = 1, Post = post
            });
        }
        
        db.SaveChanges();
    }
}
