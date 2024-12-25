namespace PlaylistConverter.Dtos.Youtube;

public class YoutubeListResponse<T>
{
    public List<T> Items { get; set; }
    public string Kind { get; set; }   
    public string Etag { get; set; }
    public string NextPageToken { get; set; }
}