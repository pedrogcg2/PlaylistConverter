namespace PlaylistConverter.Client.Spotify;

public interface ISpotifyClient
{
    Task<string> FindMusicByTitle(string title);
}