using Data;
using Microsoft.EntityFrameworkCore;
using Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<DataService>();

builder.Services.AddDbContext<PostContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("ContextSQLite")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}

app.MapGet("/api/posts", (DataService service) =>
{
    return service.GetPosts();
});

app.MapGet("/api/posts/{id}", (DataService service, int id) =>
{
    return service.GetPost(id);
});

app.MapPut("/api/posts/{id}/upvote", (DataService Service, int id) =>
{
    return Service.UpvotePost(id);
});
app.MapPut("/api/posts/{id}/downvote", (DataService Service, int id) =>
{
    return Service.DownvotePost(id);
});

app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (DataService Service, int postid, int commentid) =>
{
    return Service.UpvoteComment(postid, commentid);
});

app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (DataService Service, int postid, int commentid) =>
{
    return Service.DownvoteComment(postid, commentid);
});

app.MapPost("api/posts", (DataService Service) => { });
app.MapPost("api/posts/{id}/comments", (DataService Service, int id) => { });

app.Run();
