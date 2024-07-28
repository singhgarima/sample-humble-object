using Moq;
using SampleHumbleObject.command.joke;
using SampleHumbleObject.service;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace SampleHumbleObject.UnitTests.command.joke;

public class RandomCommandTest
{
    private readonly IRemainingArguments _remainingArgs = Mock.Of<IRemainingArguments>();

    [Fact]
    public void Execute_ShouldPrintADadJoke()
    {
        // arrange
        var jokeService = new Mock<IJokeService>();
        const string aRandomJoke = "To be Frank, I'd have to change my name.";
        jokeService.Setup(x => x.GetRandomJoke())
            .ReturnsAsync(aRandomJoke);

        var console = new TestConsole().EmitAnsiSequences();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new RandomCommand(console, jokeService.Object);

        // act
        var result = command.Execute(context);

        // arrange
        Assert.Contains(aRandomJoke, console.Output);
        Assert.Equal(0, result);
    }

    [Fact]
    public void Execute_ShouldPrintAnErrorWhenNoJokeFound()
    {
        // arrange
        var jokeService = new Mock<IJokeService>();
        jokeService.Setup(x => x.GetRandomJoke())
            .ReturnsAsync((string?)null);

        var console = new TestConsole();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new RandomCommand(console, jokeService.Object);

        // act
        var result = command.Execute(context);

        // arrange
        Assert.Contains("Sorry! No jokes found, try again in a bit.", console.Output);
        Assert.Equal(1, result);
    }
}