using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            db.Posts.Add(new Post
            {
                Title = "Harry Potter", Content = "Harry potter er en god filmserie", User = user,
                Downvotes = 10, Upvotes = 1
            });
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
    }
}