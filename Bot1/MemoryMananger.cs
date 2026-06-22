using System.Collections.Generic;
using System.IO;

namespace Bot1
{
    public class MemoryManager
    {
        public string UserName { get; set; }

        public string FavouriteTopic { get; set; }

        public List<string> ConversationHistory { get; set; }

        private readonly string filePath = "memory.txt";

        public MemoryManager()
        {
            ConversationHistory = new List<string>();
            LoadMemory();
        }

        public void SaveMemory()
        {
            List<string> lines = new List<string>();

            lines.Add(UserName ?? "");
            lines.Add(FavouriteTopic ?? "");

            lines.AddRange(ConversationHistory);

            File.WriteAllLines(filePath, lines);
        }

        public void LoadMemory()
        {
            if (!File.Exists(filePath))
                return;

            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length > 0)
                UserName = lines[0];

            if (lines.Length > 1)
                FavouriteTopic = lines[1];

            for (int i = 2; i < lines.Length; i++)
            {
                ConversationHistory.Add(lines[i]);
            }
        }

        public void AddConversation(string text)
        {
            ConversationHistory.Add(text);
            SaveMemory();
        }
    }
}