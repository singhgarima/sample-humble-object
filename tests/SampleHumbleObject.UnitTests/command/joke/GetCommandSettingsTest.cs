using SampleHumbleObject.command.joke;

namespace SampleHumbleObject.UnitTests.command.joke;

public class GetCommandSettingsTest
{
    [Theory]
    [InlineData("a-valid-id", true, null)]
    [InlineData("", false, "The joke ID is required.")]
    [InlineData("   ", false, "The joke ID is required.")]
    public void Validate_ShouldValidateSettings(string id, bool expected, string? expectedMessage)
    {
        // arrange
        var settings = new GetCommand.Settings
        {
            Id = id
        };

        // act
        var result = settings.Validate();

        // assert
        Assert.Equal(expected, result.Successful);
        Assert.Equal(expectedMessage, result.Message);
    }
}