namespace PlaylistConverter.Dtos.Youtube;

public record YoutubePlaylistSnippet(DateTime PublishedAt, string Title, string Description, uint Position);