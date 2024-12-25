using PlaylistConverter.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IYoutubeClient, YoutubeClient>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/youtube-playlist/{id}", async (string id, IYoutubeClient client) =>
{
    var result = await client.GetPlaylistItems(id);
    
    if (result.Count == 0)
        return Results.NotFound();
    
    return Results.Ok(result);
});

app.Run();
