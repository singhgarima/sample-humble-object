using SampleHumbleObject;
using SampleHumbleObject.command.joke;
using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

var typeRegistrar = new TypeRegistrar();
typeRegistrar.RegisterInstance(typeof(IAnsiConsole), AnsiConsole.Console);
var jokeService = new JokeService(new HttpClient());
typeRegistrar.RegisterInstance(typeof(IJokeService), jokeService);

var app = new CommandApp(typeRegistrar);
app.Configure(config =>
{
    config.AddBranch("joke", jokeConfig =>
    {
        jokeConfig.AddCommand<RandomCommand>("random");
        jokeConfig.AddCommand<GetCommand>("get");
        jokeConfig.AddCommand<SearchCommand>("search");
    });
    config.SetApplicationVersion("1.0.0");
});
app.Run(args);