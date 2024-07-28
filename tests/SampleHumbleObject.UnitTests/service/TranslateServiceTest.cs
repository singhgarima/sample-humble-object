using System.Net;
using System.Security.AccessControl;
using Moq;
using Moq.Protected;
using SampleHumbleObject.service;

namespace SampleHumbleObject.UnitTests.service;

public class TranslateServiceTest
{
    private static readonly HttpResponseMessage SuccessfulYogaMsg = new()
    {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent("""
                                    {
                                      "success": {
                                        "total": 1
                                      },
                                      "contents": {
                                        "translated": "Lost a planet, master obiwan has.",
                                        "text": "Master Obiwan has lost a planet.",
                                        "translation": "yoda"
                                      }
                                    }
                                    """)
    };

    private static readonly HttpResponseMessage SuccessfulGrootMsg = new()
    {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent("""
                                    {
                                        "success": {
                                            "total": 1
                                        },
                                        "contents": {
                                            "translated": "I am grooto",
                                            "text": "hello",
                                            "translation": "groot"
                                        }
                                    }
                                    """)
    };

    private static readonly HttpResponseMessage FailedHttpResponseMessage = new()
    {
        StatusCode = HttpStatusCode.BadRequest,
        Content = new StringContent("""
                                    {
                                        "error": {
                                            "code": 400,
                                            "message": "Bad Request: text is missing."
                                        }
                                    }
                                    """)
    };

    private readonly TranslateService _service;

    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

    public TranslateServiceTest()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(JokeService.JokesApiUrl)
        };
        _service = new TranslateService(httpClient);
    }

    [Fact]
    public async Task TranslateToYoda_Successfully()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(SuccessfulYogaMsg);

        // act
        var translation = await _service.TranslateToYoda("Master Obiwan has lost a planet.");

        // assert
        Assert.Equal("Lost a planet, master obiwan has.", translation);
    }

    [Fact]
    public async Task TranslateToYoda_FailsWhenRemoteServerFails()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(FailedHttpResponseMessage);

        // act
        var translation = await _service.TranslateToYoda("Master Obiwan has lost a planet.");

        // assert
        Assert.Null(translation);
    }

    [Fact]
    public async Task TranslateToGroot_Successfully()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(SuccessfulGrootMsg);

        // act
        var translation = await _service.TranslateToGroot("hello");

        // assert
        Assert.Equal("I am grooto", translation);
    }

    [Fact]
    public async Task TranslateToGroot_FailsWhenRemoteServerFails()
    {
        // arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(FailedHttpResponseMessage);

        // act
        var translation = await _service.TranslateToGroot("hello");

        // assert
        Assert.Null(translation);
    }
}