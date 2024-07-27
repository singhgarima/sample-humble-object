namespace SampleHumbleObject.UnitTests;

public class TypeRegistrarTest
{
    [Fact]
    public void ShouldInitialise()
    {
        var registrar = new TypeRegistrar();

        Assert.NotNull(registrar);
        Assert.NotNull(registrar.Services);
    }

    [Fact]
    public void Register_ShouldAddClassToServices()
    {
        var registrar = new TypeRegistrar();

        registrar.Register(typeof(ISampleInterface), typeof(SampleClass));

        Assert.NotNull(registrar.Services);
        Assert.NotEmpty(registrar.Services);
        Assert.Equal("SampleHumbleObject.UnitTests.SampleClass", registrar.Services[0].ImplementationType?.ToString());
    }

    [Fact]
    public void RegisterInstance_ShouldAddInstanceToServices()
    {
        var registrar = new TypeRegistrar();

        registrar.RegisterInstance(typeof(ISampleInterface), new SampleClass());

        Assert.NotNull(registrar.Services);
        Assert.NotEmpty(registrar.Services);
        Assert.IsType<SampleClass>(registrar.Services[0].ImplementationInstance);
    }

    [Fact]
    public void RegisterLazy_ShouldAddLazyToServices()
    {
        var registrar = new TypeRegistrar();

        registrar.RegisterLazy(typeof(ISampleInterface), () => new SampleClass());

        Assert.NotNull(registrar.Services);
        Assert.NotEmpty(registrar.Services);
        Assert.IsType<SampleClass>(registrar.Services[0].ImplementationFactory?.Invoke(null!));
    }

    [Fact]
    public void Build_ShouldBuildTypeResolver()
    {
        var registrar = new TypeRegistrar();
        registrar.Register(typeof(ISampleInterface), typeof(SampleClass));

        var resolver = registrar.Build();

        Assert.NotNull(resolver);
    }
}

internal interface ISampleInterface
{
}

internal class SampleClass : ISampleInterface
{
}