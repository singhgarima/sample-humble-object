using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SampleHumbleObject.command;

public class JokeCommand(IAnsiConsole console, IJokeService jokeService) : Command
{
    public override int Execute(CommandContext context)
    {
        var joke = jokeService.GetRandomJoke().Result;

        if (string.IsNullOrWhiteSpace(joke))
        {
            console.MarkupLine("[red]Sorry![/] No jokes found, try again in a bit.");
            return 1;
        }

        console.WriteLine(joke);
        return 0;
    }
}