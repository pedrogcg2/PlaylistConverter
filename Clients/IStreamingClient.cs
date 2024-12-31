using PlaylistConverter.Dtos;
using PlaylistConverter.Utils;

namespace PlaylistConverter.Clients;

public interface IStreamingClient
{
    Task<Result<List<PlaylistItem>>> GetPlaylistItems(string playlistId);
    
    Task<Result<string>> FindMusicByTitle(string title);
}