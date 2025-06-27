public class SentimentAnalyzer
{
    public string Analyze(string text)
    {
        if (text.Contains("love", StringComparison.OrdinalIgnoreCase))
            return "positive";
        if (text.Contains("hate", StringComparison.OrdinalIgnoreCase))
            return "negative";
        return "neutral";
    }
}
