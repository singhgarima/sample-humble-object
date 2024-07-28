using System.ComponentModel;
using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SampleHumbleObject.command.translate;

public class GrootCommand(IAnsiConsole console, ITranslateService service) : Command<GrootCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[TEXT]")]
        [Description("The text to translate into Groot.")]
        public string Text { get; init; } = string.Empty;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var translation = service.TranslateToGroot(settings.Text).Result;
        if (string.IsNullOrWhiteSpace(translation))
        {
            console.MarkupLine("Sorry! No translation found, try again in a bit.");
            return 1;
        }

        console.MarkupLine($"[bold]Groot[/]: {translation}");
        return 0;
    }
}