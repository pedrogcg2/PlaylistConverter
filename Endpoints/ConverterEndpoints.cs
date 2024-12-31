using PlaylistConverter.Client;
using PlaylistConverter.Client.Spotify;

namespace PlaylistConverter.Endpoints;

public static class ConverterEndpoints
{
    public static WebApplication MapConverterEndpoints(this WebApplication app)
    {
        app.MapGet("/youtube-playlist/{id}", GetPlaylistFromYoutube);
        app.MapGet("/spotify-track/{title}", GetSpotifyTrackByTitle);
        return app;
    }
    
    public static async Task<IResult> GetPlaylistFromYoutube(string id, IYoutubeClient client)
    {
        var result = await client.GetPlaylistItems(id);
    
        if (result.Count == 0)
            return Results.NotFound();
    
        return Results.Ok(result);
    }

    public static async Task<IResult> GetSpotifyTrackByTitle(string title, ISpotifyClient client)
    {
        var result = await client.FindMusicByTitle(title);

        if (string.IsNullOrEmpty(result)) 
            return Results.NotFound();
        
        return Results.Ok(result);
    }
}