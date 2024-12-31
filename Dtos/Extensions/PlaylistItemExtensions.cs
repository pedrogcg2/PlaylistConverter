using PlaylistConverter.Dtos.Youtube;

namespace PlaylistConverter.Dtos.Extensions;

public static class PlaylistItemExtensions
{
    public static PlaylistItem ToPlaylistItem(this YoutubePlaylistItem item)
    {
        return new PlaylistItem(item.Snippet.Title, "", "", item.Snippet.Position);
    }
        
}