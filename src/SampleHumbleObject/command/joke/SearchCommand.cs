using System.ComponentModel;
using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SampleHumbleObject.command.joke;

public class SearchCommand(IAnsiConsole console, IJokeService jokeService) : Command<SearchCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        var jokes = jokeService.SearchJokes(settings.Term).Result;

        if (jokes.Count == 0)
        {
            console.MarkupLine("[red]Sorry![/] No jokes found, try again in a bit.");
            return 1;
        }

        console.MarkupLine($"[green]Found {jokes.Count} jokes for '{settings.Term}':[/]");
        foreach (var joke in jokes) console.MarkupLine(" - " + joke);

        return 0;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[TERM]")]
        [Description("The search term to use.")]
        public required string Term { get; init; }


        public override ValidationResult Validate()
        {
            return string.IsNullOrWhiteSpace(Term)
                ? ValidationResult.Error("The search term is required.")
                : ValidationResult.Success();
        }
    }
}