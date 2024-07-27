using System.Text.Json;

namespace SampleHumbleObject.service;

public class JokeService(HttpClient client) : IJokeService
{
    public const string JokesApiUrl = "https://icanhazdadjoke.com/";

    public async Task<string?> GetRandomJoke()
    {
        var response = client.GetAsync(JokesApiUrl);
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