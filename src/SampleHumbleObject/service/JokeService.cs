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
        return await GetJokeFromResponse(response.Result);
    }


    public async Task<string?> GetAJoke(string id)
    {
        var response = _client.GetAsync(JokesApiUrl + "/j/" + id);
        return await GetJokeFromResponse(response.Result);
    }

    private static async Task<string?> GetJokeFromResponse(HttpResponseMessage message)
    {
        if (!message.IsSuccessStatusCode) return null;

        var data = await message.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(data);
        return jsonDocument.RootElement.GetProperty("joke").GetString();
    }
}

public interface IJokeService
{
    public Task<string?> GetRandomJoke();
    public Task<string?> GetAJoke(string id);
}