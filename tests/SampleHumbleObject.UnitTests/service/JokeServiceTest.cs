using System.Net;
using Moq;
using Moq.Protected;
using SampleHumbleObject.service;

namespace SampleHumbleObject.UnitTests.service;

public class JokeServiceTest
{
    private static readonly HttpResponseMessage SuccessfulHttpResponseMessage = new()
    {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent("""
                                    {
                                      "id": "joke-id-cow",
                                      "joke": "What kind of magic do cows believe in? MOODOO.",
                                      "status": 200
                                    }
                                    """)
    };

    private static readonly HttpResponseMessage FailedHttpResponseMessage = new()
    {
        StatusCode = HttpStatusCode.InternalServerError
    };

    private readonly JokeService _jokeService;

    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

    public JokeServiceTest()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(JokeService.JokesApiUrl)
        };
        _jokeService = new JokeService(httpClient);
    }

    [Fact]
    public async Task GetRandomJoke_GetsRandomJokeSuccessfully()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(SuccessfulHttpResponseMessage);

        // act
        var joke = await _jokeService.GetRandomJoke();

        // assert
        Assert.Equal("What kind of magic do cows believe in? MOODOO.", joke);
    }

    [Fact]
    public async Task GetRandomJoke_FailsWhenRemoteServerFails()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(FailedHttpResponseMessage);

        // act
        var joke = await _jokeService.GetRandomJoke();

        // assert
        Assert.Null(joke);
    }
}