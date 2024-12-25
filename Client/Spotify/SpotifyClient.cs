namespace playlist_converter.Client.Spotify;

public class SpotifyClient : ISpotifyClient
{
    private readonly HttpClient _httpClient;


    public Task<string> FindMusicByTitle(string title)
    {
        throw new NotImplementedException();
    }
}