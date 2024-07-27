using Moq;

namespace SampleHumbleObject.UnitTests;

public class TypeResolverTest
{
    [Fact]
    public void Constructor_ShouldInitialise()
    {
        //arrange
        var provider = new Mock<IServiceProvider>();

        //act
        var resolver = new TypeResolver(provider.Object);

        //assert
        Assert.NotNull(resolver);
    }

    [Fact]
    public void Resolve_ShouldResolveType()
    {
        //arrange
        var provider = new Mock<IServiceProvider>();
        var sampleObject = new SampleClass();
        provider.Setup(p => p.GetService(typeof(ISampleInterface))).Returns(sampleObject);

        //act
        var resolver = new TypeResolver(provider.Object);
        var result = resolver.Resolve(typeof(ISampleInterface));

        //assert
        Assert.Equal(sampleObject, result);
    }

    [Fact]
    public void Resolve_ShouldResolveNullForTypeNull()
    {
        //arrange
        var provider = new Mock<IServiceProvider>();

        //act
        var resolver = new TypeResolver(provider.Object);
        var result = resolver.Resolve(null);

        //assert
        Assert.Null(result);
    }
}