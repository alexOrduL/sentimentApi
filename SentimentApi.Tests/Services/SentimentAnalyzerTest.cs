using SentimentApi.Services;
using Xunit;

public class SentimentAnalyzerTests
{
    private readonly SentimentAnalyzer _analyzer = new();

    [Theory]
    [InlineData("Este producto es excelente", "positivo")]
    [InlineData("El servicio fue terrible", "negativo")]
    [InlineData("No tengo una opinión", "neutral")]
    public void Analyze_ReturnsExpectedSentiment(string input, string expected)
    {
        var result = _analyzer.Analyze(input);

        Assert.Equal(expected, result);
    }
}
