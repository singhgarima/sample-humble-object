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
        StatusCode = HttpStatusCode.NotFound
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


    [Fact]
    public async Task GetAJoke_GetsAJokeSuccessfully()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(SuccessfulHttpResponseMessage);

        // act
        var joke = await _jokeService.GetAJoke("a-specific-id");

        // assert
        Assert.Equal("What kind of magic do cows believe in? MOODOO.", joke);
    }

    [Fact]
    public async Task GetAJoke_FailsWhenRemoteServerFails()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(FailedHttpResponseMessage);

        // act
        var joke = await _jokeService.GetAJoke("invalid-id");

        // assert
        Assert.Null(joke);
    }

    [Fact]
    public async Task SearchJokes_GetsAJokeSuccessfully()
    {
        // arrange
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("""
                                        {
                                            "current_page": 1,
                                            "limit": 5,
                                            "next_page": 2,
                                            "previous_page": 1,
                                            "results": [
                                              {
                                                "id": "M7wPC5wPKBd",
                                                "joke": "Joke 1."
                                              },
                                              {
                                                "id": "MRZ0LJtHQCd",
                                                "joke": "Joke 2."
                                              },
                                              {
                                                "id": "M7wPC5w2KBd",
                                                "joke": "Joke 3."
                                              },
                                              {
                                                "id": "MRZ0LJ1HQCd",
                                                "joke": "Joke 4."
                                              },
                                              {
                                                "id": "M7wPCawPKBd",
                                                "joke": "Joke 5."
                                              }
                                            ]
                                        }
                                        """)
        };
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // act
        var jokes = await _jokeService.SearchJokes("a-term");

        // assert
        Assert.Equal(5, jokes.Count);
        Assert.Equal("Joke 1.", jokes[0]);
        Assert.Equal("Joke 2.", jokes[1]);
        Assert.Equal("Joke 3.", jokes[2]);
        Assert.Equal("Joke 4.", jokes[3]);
        Assert.Equal("Joke 5.", jokes[4]);
    }

    [Fact]
    public async Task SearchJokes_FailsWhenRemoteServerFails()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(FailedHttpResponseMessage);

        // act
        var jokes = await _jokeService.SearchJokes("invalid-id");

        // assert
        Assert.Equal(0, jokes.Count);
    }
}