using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using playlist_converter.Clients.Extensions;

namespace PlaylistConverter.Client.Spotify;

public class SpotifyClient : ISpotifyClient
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    
    public SpotifyClient(IConfiguration configuration)
    {
        _clientId = configuration["SPOTIFY_CLIENT_ID"] ?? throw new NullReferenceException();
        _clientSecret = configuration["SPOTIFY_CLIENT_SECRET"] ?? throw new NullReferenceException();
        _httpClient = new HttpClient();
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }
    
    public async Task<string> FindMusicByTitle(string title)
    {
        Dictionary<string, string> queryParams = new()
        {
            {"q", title},
            {"type", "track"}
        };
        
        var accessToken = await GetAccessToken();

        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get,
            QueryHelpers.AddQueryString(SpotifyConstants.GetTracksUrl, queryParams!));        
        
        requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
        var response = await _httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
            return "";

        var result = await response.DeserializeContent<SpotifyTrackResponse<SpotifyTrackItem>>(_jsonSerializerOptions);
        return result.Tracks.Items[0].ExternalUrls.Spotify;
    }

    private async Task<string> GetAccessToken()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, SpotifyConstants.GetAccessTokenUrl);
        Dictionary<string, string> form = new() {{"grant_type", "client_credentials"}};
        request.Content = new FormUrlEncodedContent(form);
        
        var auth = System.Convert.ToBase64String(
            ASCIIEncoding.ASCII.GetBytes(_clientId + ":" + _clientSecret));
        
        request.Headers.Add("Authorization", "Basic " + auth);
        request.RequestUri = new Uri(SpotifyConstants.GetAccessTokenUrl);
        
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return "";
        
        var responseBody = await response.DeserializeContent<SpotifyAccountResponse>(_jsonSerializerOptions);
        
        return responseBody!.AccessToken;
    }
}

public static class SpotifyConstants
{
    public static string GetAccessTokenUrl => "https://accounts.spotify.com/api/token";
    
    public static string GetTracksUrl => "https://api.spotify.com/v1/search";
}

public record SpotifyAccountResponse(string AccessToken, string TokenType, int ExpiresIn);

public record SpotifyTrackResponse<T>(SpotifyResponse<T> Tracks);

public class SpotifyResponse<T>
{
    public string Href { get; set; }
    
    public int Limit { get; set; }
    
    public int Offset { get; set; }

    public string Next { get; set; }
    
    public string Previous { get; set; }
    
    public int Total { get; set; }
    
    public List<T> Items { get; set; }
}
//todo: eval the use of ISRC to find titles and tacks
public record SpotifyTrackItem(string Id, SpotifyExternalUrl ExternalUrls);

public record SpotifyExternalUrl(string Spotify);