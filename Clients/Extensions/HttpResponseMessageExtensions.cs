using System.Text.Json;

namespace playlist_converter.Clients.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<T> DeserializeContent<T>(this HttpResponseMessage response, JsonSerializerOptions? options = null)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(responseContent, options);
        return result;
    }
}