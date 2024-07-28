using SampleHumbleObject.command.joke;

namespace SampleHumbleObject.UnitTests.command.joke;

public class SearchCommandSettingsTest
{
    [Theory]
    [InlineData("a-valid-term", true, null)]
    [InlineData("", false, "The search term is required.")]
    [InlineData("   ", false, "The search term is required.")]
    public void Validate_ShouldValidateSettings(string term, bool expected, string? expectedMessage)
    {
        // arrange
        var settings = new SearchCommand.Settings
        {
            Term = term
        };

        // act
        var result = settings.Validate();

        // assert
        Assert.Equal(expected, result.Successful);
        Assert.Equal(expectedMessage, result.Message);
    }
}