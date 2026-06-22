namespace Bot1
{
    public class CybersecurityBot
    {
        public string InvalidMessage()
        {
            return
        @"I didn't quite understand your question.

You can ask me about:

• Password Safety
• Phishing
• Safe Browsing
• Malware
• Privacy

You can also type:

• REMEMBER
• TELL ME MORE
• EXIT";
        }

        public string GoodbyeMessage(string userName)
        {
            return
        $@"Goodbye, {userName}!

Thank you for using the Cybersecurity Awareness Bot.

Stay safe online and remember:
• Use strong passwords
• Avoid suspicious links
• Protect your personal information
• Keep your software updated";
        }
    }
}