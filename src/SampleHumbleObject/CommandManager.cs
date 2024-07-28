using SampleHumbleObject.command.joke;
using SampleHumbleObject.command.translate;
using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SampleHumbleObject;

public class CommandManager
{
    public ICommandApp App { get; set; }
    public TypeRegistrar Registrar { get; set; }

    public CommandManager()
    {
        Registrar = new TypeRegistrar();
        RegisterDependencies();

        App = new CommandApp(Registrar);
    }

    private void RegisterDependencies()
    {
        Registrar.RegisterInstance(typeof(IAnsiConsole), AnsiConsole.Console);

        var jokeService = new JokeService(new HttpClient());
        Registrar.RegisterInstance(typeof(IJokeService), jokeService);

        var translateService = new TranslateService(new HttpClient());
        Registrar.RegisterInstance(typeof(ITranslateService), translateService);
    }

    public void Configure()
    {
        App.Configure(config =>
        {
            config.AddBranch("joke", jokeConfig =>
            {
                jokeConfig.AddCommand<RandomCommand>("random");
                jokeConfig.AddCommand<GetCommand>("get");
                jokeConfig.AddCommand<SearchCommand>("search");
            });
            config.AddBranch("translate", translateConfig =>
            {
                translateConfig.AddCommand<YodaCommand>("yoda");
                translateConfig.AddCommand<GrootCommand>("groot");
            });
        });
    }

    public int Run(string[] args)
    {
        return App.Run(args);
    }
}