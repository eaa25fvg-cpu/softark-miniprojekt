using Data;
using Microsoft.EntityFrameworkCore;
using Data;
using Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<PostContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("ContextSQLite")));

var app = builder.Build();

app.MapGet("/api/posts", (DataService service) =>
{
    return service.GetPosts();
});

app.MapGet("/api/posts/{id}", (DataService service, int id) =>
{
    return service.GetPost(id);
});

app.MapPut("/api/posts/{id}/upvote", (DataService Service, int id) => { });
app.MapPut("/api/posts/{id}/downvote", (DataService Service, int id) => { });

app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (DataService Service, int postid, int commentid) => { });
app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (DataService Service, int postid, int commentid) => { });

app.MapPost("api/posts", (DataService Service) => { });
app.MapPost("api/posts/{id}/comments", (DataService Service, int id) => { });