using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace SampleHumbleObject;

public class TypeRegistrar : ITypeRegistrar
{
    public ServiceCollection Services { get; } = [];

    public void Register(Type service, Type implementation)
    {
        Services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object instance)
    {
        Services.AddSingleton(service, instance);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        Services.AddSingleton(service, _ => factory());
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(Services.BuildServiceProvider());
    }
}