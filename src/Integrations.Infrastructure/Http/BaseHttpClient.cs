namespace Integrations.Infrastructure.Http;

public abstract class BaseHttpClient(HttpClient client)
{
    public async Task<T> SendAsync<T>(HttpRequestMessage request)
    {
        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request to Sentry API failed. Status Code: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        
        return System.Text.Json.JsonSerializer.Deserialize<T>(content)
               ?? throw new InvalidOperationException("Failed to deserialize response from Sentry API");
    }
}