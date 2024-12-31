using PlaylistConverter.Dtos;

namespace PlaylistConverter.Client;

public interface IYoutubeClient
{
    Task<List<PlaylistItem>> GetPlaylistItems(string playlistId);
}