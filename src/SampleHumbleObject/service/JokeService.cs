using System.Text.Json;

namespace SampleHumbleObject.service;

public class JokeService : IJokeService
{
    public const string JokesApiUrl = "https://icanhazdadjoke.com/";
    private readonly HttpClient _client;

    public JokeService(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<string?> GetRandomJoke()
    {
        var response = _client.GetAsync(JokesApiUrl);
        var message = response.Result;

        if (!message.IsSuccessStatusCode) return null;

        var data = await message.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(data);
        return jsonDocument.RootElement.GetProperty("joke").GetString();
    }
}

public interface IJokeService
{
    public Task<string?> GetRandomJoke();
}