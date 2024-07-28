using System.ComponentModel;
using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SampleHumbleObject.command.joke;

public class GetCommand(IAnsiConsole console, IJokeService jokeService) : Command<GetCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        var joke = jokeService.GetAJoke(settings.Id).Result;

        if (string.IsNullOrWhiteSpace(joke))
        {
            console.MarkupLine("[red]Sorry![/] No jokes found, try again in a bit.");
            return 1;
        }

        console.WriteLine(joke);
        return 0;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[ID]")]
        [Description("The ID of the joke to get.")]
        public required string Id { get; init; }


        public override ValidationResult Validate()
        {
            return string.IsNullOrWhiteSpace(Id)
                ? ValidationResult.Error("The joke ID is required.")
                : ValidationResult.Success();
        }
    }
}