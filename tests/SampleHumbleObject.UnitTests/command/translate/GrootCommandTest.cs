using Moq;
using SampleHumbleObject.command.joke;
using SampleHumbleObject.command.translate;
using SampleHumbleObject.service;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace SampleHumbleObject.UnitTests.command.translate;

public class GrootCommandTest
{
    private readonly IRemainingArguments _remainingArgs = Mock.Of<IRemainingArguments>();

    [Fact]
    public void Execute_ShouldPrintTranslation()
    {
        // arrange
        var service = new Mock<ITranslateService>();
        const string englishText = "bye";
        var translatedText = "I am groote";
        service.Setup(x => x.TranslateToGroot(englishText))
            .ReturnsAsync(translatedText);

        var console = new TestConsole().EmitAnsiSequences();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new GrootCommand(console, service.Object);

        // act
        var result = command.Execute(context, new GrootCommand.Settings
        {
            Text = englishText
        });

        // arrange
        Assert.Contains(translatedText, console.Output);
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void Execute_ShouldPrintAnErrorWhenNoTranslationFound()
    {
        // arrange
        var service = new Mock<ITranslateService>();
        const string englishText = "invalid text";
        service.Setup(x => x.TranslateToGroot(englishText))
            .ReturnsAsync((string?)null);

        var console = new TestConsole();

        var context = new CommandContext([], _remainingArgs, "", null);
        var command = new GrootCommand(console, service.Object);

        // act
        var result = command.Execute(context, new GrootCommand.Settings
        {
            Text = englishText
        });

        // arrange
        Assert.Contains("Sorry! No translation found, try again in a bit.", console.Output);
        Assert.Equal(1, result);
    }

}