using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseServices();

var app = builder.Build();

app.MapPost("/postsWithLazyLoading", (MyDbContext db) =>
{
    var stopWatch = Stopwatch.StartNew();

    var users = db.Users.ToList();
    foreach (var user in users)
    {
        foreach (var post in user.posts)
        {
            Console.WriteLine($"User: {user.username} has post: {post.message}");
        }
    }

    stopWatch.Stop();
    return Results.Ok($"Posts processed successfully. Time taken: {stopWatch.ElapsedMilliseconds} ms");
});

app.MapPost("/postsWithoutLazyLoading", (MyDbContext db) =>
{
    var stopWatch = Stopwatch.StartNew();

    var users = db.Users.Include(u => u.posts);
    foreach (var user in users)
    {
        foreach (var post in user.posts)
        {
            Console.WriteLine($"User: {user.username} has post: {post.message}");
        }
    }

    stopWatch.Stop();
    return Results.Ok($"Posts processed successfully. Time taken: {stopWatch.ElapsedMilliseconds} ms");
});

app.Run();

