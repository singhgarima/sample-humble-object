using Spectre.Console.Cli;

namespace SampleHumbleObject;

public class TypeResolver(IServiceProvider serviceProvider) : ITypeResolver
{
    public object? Resolve(Type? type)
    {
        return type == null ? null : serviceProvider.GetService(type);
    }
}