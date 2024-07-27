using Moq;
using SampleHumbleObject.command;
using SampleHumbleObject.service;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace SampleHumbleObject.UnitTests.command;

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
}