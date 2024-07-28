using Moq;
using SampleHumbleObject.command.joke;
using SampleHumbleObject.service;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace SampleHumbleObject.UnitTests.command.joke;

public class GetCommandTest
{
    private readonly IRemainingArguments _remainingArgs = Mock.Of<IRemainingArguments>();

    [Fact]
    public void Execute_ShouldReturnJokeForTheJokeId()
    {
        // arrange
        var jokeId = "a-very-specific-id";

        var jokeService = new Mock<IJokeService>();
        const string aRandomJoke = "What do I look like? A JOKE MACHINE!?";
        jokeService.Setup(x => x.GetAJoke(It.Is<string>(i => i == jokeId)))
            .ReturnsAsync(aRandomJoke);

        var console = new TestConsole().EmitAnsiSequences();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new GetCommand(console, jokeService.Object);

        // act
        var result = command.Execute(context, new GetCommand.Settings
        {
            Id = jokeId
        });

        // arrange
        Assert.Contains(aRandomJoke, console.Output);
        Assert.Equal(0, result);
    }

    [Fact]
    public void Execute_ShouldPrintAnErrorWhenNoJokeFound()
    {
        // arrange
        var jokeId = "a-invalid-id";

        var jokeService = new Mock<IJokeService>();
        jokeService.Setup(x => x.GetAJoke(It.Is<string>(i => i == jokeId)))
            .ReturnsAsync((string?)null);

        var console = new TestConsole();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new GetCommand(console, jokeService.Object);

        // act
        var result = command.Execute(context, new GetCommand.Settings
        {
            Id = jokeId
        });

        // arrange
        Assert.Contains("No jokes found, try again in a bit.", console.Output);
        Assert.Equal(1, result);
    }
}