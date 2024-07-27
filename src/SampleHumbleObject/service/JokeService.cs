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

    public async Task<List<string>> SearchJokes(string term)
    {
        var response = _client.GetAsync(JokesApiUrl + "/search?page=1&limit=5&term=" + term);
        var message = response.Result;
        if (!message.IsSuccessStatusCode) return [];

        var data = await message.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(data);

        var results = jsonDocument.RootElement.GetProperty("results");
        var jokes = new List<string>();
        for (var i = 0; i < results.GetArrayLength(); i++)
        {
            var joke = results[i].GetProperty("joke").GetString();
            if (joke == null) continue;
            jokes.Add(joke);
        }

        return jokes;
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
    public Task<List<string>> SearchJokes(string term);
}