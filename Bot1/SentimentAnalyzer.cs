namespace Bot1
{
    public class SentimentAnalyzer
    {
        public string GetSentimentResponse(string message)
        {
            string lower = message.ToLower();

            if (lower.Contains("sad") ||
                lower.Contains("worried") ||
                lower.Contains("confused"))
            {
                return "Don't worry. I'll help you learn cybersecurity step by step.";
            }

            if (lower.Contains("thanks") ||
                lower.Contains("awesome") ||
                lower.Contains("great"))
            {
                return "I'm glad I could help!";
            }

            if (lower.Contains("angry") ||
                lower.Contains("frustrated"))
            {
                return "I understand. Let's work through it together.";
            }

            return "";
        }
    }
}