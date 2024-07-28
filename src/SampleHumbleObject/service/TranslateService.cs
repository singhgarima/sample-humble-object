using System.Text.Json;

namespace SampleHumbleObject.service;

public class TranslateService : ITranslateService
{
    public const string ApiUrl = "https://api.funtranslations.com";
    private readonly HttpClient _client;

    public TranslateService(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<string?> TranslateToYoda(string text)
    {
        var response = _client.GetAsync(ApiUrl + "/translate/yoda.json?text=" + text);
        return await GetTranslationFromResponse(response);
    }


    public async Task<string?> TranslateToGroot(string text)
    {
        var response = _client.GetAsync(ApiUrl + "/translate/groot.json?text=" + text);
        return await GetTranslationFromResponse(response);
    }

    private static async Task<string?> GetTranslationFromResponse(Task<HttpResponseMessage> response)
    {
        var message = response.Result;
        if (!message.IsSuccessStatusCode)
        {
            return null;
        }

        var data = await message.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(data);
        jsonDocument.RootElement.GetProperty("contents").GetProperty("translated").GetString();
        return jsonDocument.RootElement.GetProperty("contents").GetProperty("translated").GetString();
    }
}

public interface ITranslateService
{
    public Task<string?> TranslateToYoda(string text);
    public Task<string?> TranslateToGroot(string text);
}