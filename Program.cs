using PlaylistConverter.Client;
using PlaylistConverter.Client.Spotify;
using PlaylistConverter.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IYoutubeClient, YoutubeClient>();
builder.Services.AddScoped<ISpotifyClient, SpotifyClient>();
builder.Services.AddOpenApi();
var app = builder.Build();
app.MapOpenApi();
app.MapConverterEndpoints();

app.Run();
