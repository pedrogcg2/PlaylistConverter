using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using PlaylistConverter.Dtos;
using PlaylistConverter.Dtos.Extensions;
using PlaylistConverter.Dtos.Youtube;

namespace PlaylistConverter.Client;

public class YoutubeClient : IYoutubeClient
{
    private readonly string _key;
    private readonly string _youtubeBase = "https://www.googleapis.com/youtube/v3";
    private readonly string _playlistItemsUrl = "/playlistItems";
    
    private HttpClient _httpClient { get; }

    public YoutubeClient(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _key = configuration["YOUTUBE_API_KEY"] ?? throw new NullReferenceException();
    }

    public async Task<List<PlaylistItem>> GetPlaylistItems(string playlistId)
    {
        List<PlaylistItem> result = [];
        
        Dictionary<string, string> queryParams = new();
        queryParams["playlistId"] = playlistId;
        queryParams["part"] = "snippet";
        queryParams["key"] =  _key;
        
        bool hasNextPage;
        do
        {
            var url = _youtubeBase + _playlistItemsUrl;
            HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(url, queryParams));
            if (!response.IsSuccessStatusCode)
                break;
            
            var responseJson = await response.Content.ReadAsStringAsync(); 
            var dto
                = JsonSerializer.Deserialize<YoutubeListResponse<YoutubePlaylistItem>>(responseJson, 
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
            
            result.AddRange(dto!.Items.Select(i => i.ToPlaylistItem()));
            hasNextPage = false;
            if (!string.IsNullOrEmpty(dto.NextPageToken))
            {
                hasNextPage = true;
                queryParams["pageToken"] = dto.NextPageToken;
            }
            
        } 
        while (hasNextPage);

        return result;
    }
    
}