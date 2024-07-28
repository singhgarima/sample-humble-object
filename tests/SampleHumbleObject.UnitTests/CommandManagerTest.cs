using Moq;
using SampleHumbleObject.service;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SampleHumbleObject.UnitTests;

public class CommandManagerTest
{
    [Fact]
    public void Constructor_ShouldInitialiseCommandApp()
    {
        // arrange and act
        var manager = new CommandManager();

        // assert
        Assert.NotNull(manager.App);
    }

    [Fact]
    public void Constructor_ShouldInitialiseRegistrar()
    {
        // arrange and act
        var manager = new CommandManager();

        // assert
        VerifyInstancesAreRegistered(manager.Registrar);
    }

    [Fact]
    public void Configure_ShouldConfigureSubCommands()
    {
        // arrange
        var commandApp = new Mock<ICommandApp>();
        var mockConfig = CreateMockForConfiguration(commandApp);

        var manager = new CommandManager { App = commandApp.Object };

        // act
        manager.Configure();

        // assert
        VerifyAppIsConfigured(commandApp);
        VerifyJokeCommandConfigured(mockConfig);
        VerifyTranslateCommandConfigured(mockConfig);
    }

    [Fact]
    public void Run_ShouldRunCommandAppWithExpectedArgs()
    {
        // arrange
        var commandApp = new Mock<ICommandApp>();
        var manager = new CommandManager
        {
            App = commandApp.Object
        };

        // act
        manager.Run(["joke", "random"]);

        // assert
        VerifyAppRunWithPassedArguments(commandApp);
    }

    private static void VerifyAppRunWithPassedArguments(Mock<ICommandApp> commandApp)
    {
        commandApp.Verify(app => app.Run(
            new[] { "joke", "random" }
        ), Times.Once);
    }

    private static Mock<IConfigurator> CreateMockForConfiguration(Mock<ICommandApp> commandApp)
    {
        var mockConfig = new Mock<IConfigurator>();
        var mockBranchConfig = new Mock<IBranchConfigurator>();
        mockConfig.Setup(config =>
                config.AddBranch(It.IsAny<string>(), It.IsAny<Action<IConfigurator<CommandSettings>>>()))
            .Returns(mockBranchConfig.Object);
        commandApp.Setup(app => app.Configure(It.IsAny<Action<IConfigurator>>()))
            .Callback<Action<IConfigurator>>(action => action(mockConfig.Object));
        return mockConfig;
    }

    private static void VerifyJokeCommandConfigured(Mock<IConfigurator> mockConfig)
    {
        mockConfig.Verify(
            config => config.AddBranch("joke", It.IsAny<Action<IConfigurator<CommandSettings>>>()),
            Times.Once);
    }

    private static void VerifyTranslateCommandConfigured(Mock<IConfigurator> mockConfig)
    {
        mockConfig.Verify(
            config => config.AddBranch("translate", It.IsAny<Action<IConfigurator<CommandSettings>>>()),
            Times.Once);
    }

    private static void VerifyAppIsConfigured(Mock<ICommandApp> commandApp)
    {
        commandApp.Verify(app => app.Configure(
            It.IsAny<Action<IConfigurator>>()
        ), Times.Once);
    }

    private static void VerifyInstancesAreRegistered(TypeRegistrar typeRegistrar)
    {
        Assert.NotNull(typeRegistrar);
        Assert.Equal(typeof(IAnsiConsole), typeRegistrar.Services[0].ServiceType);
        Assert.Equal(typeof(IJokeService), typeRegistrar.Services[1].ServiceType);
        Assert.Equal(typeof(ITranslateService), typeRegistrar.Services[2].ServiceType);
    }
}