using SampleHumbleObject;
using SampleHumbleObject.command;
using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

var typeRegistrar = new TypeRegistrar();
typeRegistrar.RegisterInstance(typeof(IAnsiConsole), AnsiConsole.Console);
var jokeService = new JokeService(new HttpClient());
typeRegistrar.RegisterInstance(typeof(IJokeService), jokeService);

var app = new CommandApp<JokeCommand>(typeRegistrar);
app.Configure(config =>
{
    config.SetApplicationVersion("1.0.0");
    config.AddCommand<GetCommand>("get");
    config.AddCommand<SearchCommand>("search");
});
app.Run(args);