using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using shared.Model;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "";

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        this.configuration = configuration;
        this.baseAPI = configuration["base_api"];
    }

    public async Task<Post[]> GetPosts()
    {
        string url = $"{baseAPI}posts/";
        return await http.GetFromJsonAsync<Post[]>(url);
    }

    public async Task<Post> GetPost(int id)
    {
        string url = $"{baseAPI}posts/{id}/";
        return await http.GetFromJsonAsync<Post>(url);
    }

    public async Task<Post> CreatePost(string title, string? content, string? postUrl, string author)
    {
        string url = $"{baseAPI}posts";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { title, content, url = postUrl, author });

        // Deserialize the response to a Post object
        return await ReadJson<Post>(msg);
    }

    public async Task<Comment> CreateComment(string content, int postId, string author)
    {
        string url = $"{baseAPI}posts/{postId}/comments";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { content, author });

        // Deserialize the response to a Comment object
        return await ReadJson<Comment>(msg);
    }

    public async Task<Post> UpvotePost(int id)
    {
        HttpResponseMessage msg = await http.PutAsJsonAsync($"{baseAPI}posts/{id}/upvote/", "");
        return await ReadJson<Post>(msg);
    }

    public async Task<Post> DownvotePost(int id)
    {
        HttpResponseMessage msg = await http.PutAsJsonAsync($"{baseAPI}posts/{id}/downvote/", "");
        return await ReadJson<Post>(msg);
    }

    public async Task<Comment> UpvoteComment(int postId, int commentId)
    {
        HttpResponseMessage msg = await http.PutAsJsonAsync($"{baseAPI}posts/{postId}/comments/{commentId}/upvote/", "");
        return await ReadJson<Comment>(msg);
    }

    public async Task<Comment> DownvoteComment(int postId, int commentId)
    {
        HttpResponseMessage msg = await http.PutAsJsonAsync($"{baseAPI}posts/{postId}/comments/{commentId}/downvote/", "");
        return await ReadJson<Comment>(msg);
    }

    private static async Task<T?> ReadJson<T>(HttpResponseMessage msg)
    {
        // Get the JSON string from the response
        string json = await msg.Content.ReadAsStringAsync();
        if (!msg.IsSuccessStatusCode || string.IsNullOrWhiteSpace(json))
            return default;

        // Deserialize the JSON string to an object
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });
    }
}
