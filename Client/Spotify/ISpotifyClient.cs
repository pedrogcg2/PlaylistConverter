namespace playlist_converter.Client.Spotify;

public interface ISpotifyClient
{
    Task<string> FindMusicByTitle(string title);
}