using Moq;
using SampleHumbleObject.command.joke;
using SampleHumbleObject.service;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace SampleHumbleObject.UnitTests.command.joke;

public class SearchCommandTest
{
    private readonly IRemainingArguments _remainingArgs = Mock.Of<IRemainingArguments>();

    [Fact]
    public void Execute_ShouldReturnJokesForTheSearchTerm()
    {
        // arrange
        var serachTerm = "a search term";

        var jokeService = new Mock<IJokeService>();
        var jokes = new List<string> { "joke1", "joke2" };
        jokeService.Setup(x => x.SearchJokes(It.Is<string>(i => i == serachTerm)))
            .ReturnsAsync(jokes);

        var console = new TestConsole().EmitAnsiSequences();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new SearchCommand(console, jokeService.Object);

        // act
        var result = command.Execute(context, new SearchCommand.Settings
        {
            Term = serachTerm
        });

        // arrange
        Assert.Equal(0, result);
        foreach (var joke in jokes) Assert.Contains(joke, console.Output);
    }

    [Fact]
    public void Execute_ShouldPrintAnErrorWhenNoJokeFound()
    {
        // arrange
        var serachTerm = "a invalid search term";

        var jokeService = new Mock<IJokeService>();
        jokeService.Setup(x => x.SearchJokes(It.Is<string>(i => i == serachTerm)))
            .ReturnsAsync([]);

        var console = new TestConsole();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new SearchCommand(console, jokeService.Object);

        // act
        var result = command.Execute(context, new SearchCommand.Settings
        {
            Term = serachTerm
        });

        // arrange
        Assert.Contains("No jokes found, try again in a bit.", console.Output);
        Assert.Equal(1, result);
    }
}