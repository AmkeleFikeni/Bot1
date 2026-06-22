using Bot1;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Linq;
using System.IO;

namespace Bot1
{
    public partial class MainWindow : Window
    {
        private void InitializeReminderSystem()
        {
            reminderTimer = new DispatcherTimer();
            reminderTimer.Interval = TimeSpan.FromMinutes(1);
            reminderTimer.Tick += ReminderTimer_Tick;
            reminderTimer.Start();
        }
        private readonly CybersecurityBot bot = new CybersecurityBot();
        private readonly Menu menu = new Menu();
        private readonly Sound sound = new Sound();
        private readonly MemoryManager memory = new MemoryManager();
        private readonly SentimentAnalyzer sentiment = new SentimentAnalyzer();
        private readonly Logo logo = new Logo();
        private readonly DatabaseManager db = new DatabaseManager();
        private readonly Random random = new Random();
        private string lastTopic = "";
        private Dictionary<string, string[]> keywordResponses;
        private DispatcherTimer? reminderTimer;
        private List<TaskItem> reminderTasks = new();
        private string userName = "";
        private readonly List<TaskItem> tasks =
    new List<TaskItem>();

        private readonly List<string> activityLog =
            new List<string>();
        private readonly string logFile = "activitylog.txt";

        private bool quizMode = false;

        private int currentQuestion = 0;

        private int score = 0;

        private int taskCounter = 1;

        private string rememberedFact = "";

        private readonly List<(string Question,
                        string Answer,
                        string Explanation)> quizQuestions =
 new()
 {
    (
        "What does MFA stand for?",
        "multi factor authentication",
        "MFA adds an extra layer of security by requiring more than one form of verification."
    ),

    (
        "True or False: Password123 is secure?",
        "false",
        "Password123 is considered a weak password because it is easy to guess."
    ),

    (
        "What attack uses fake emails to steal information?",
        "phishing",
        "Phishing attacks trick users into revealing sensitive information."
    ),

    (
        "What type of malware locks files and demands payment?",
        "ransomware",
        "Ransomware encrypts files and demands money to restore access."
    ),

    (
        "What should you do before clicking a link in an email?",
        "verify",
        "Always verify the sender and inspect the link before clicking."
    ),

    (
        "True or False: You should use the same password for all accounts.",
        "false",
        "Each account should have a unique password."
    ),

    (
        "What software helps protect your computer from malicious programs?",
        "antivirus",
        "Antivirus software detects and removes threats."
    ),

    (
        "What does VPN stand for?",
        "virtual private network",
        "A VPN encrypts internet traffic and improves privacy."
    ),

    (
        "What type of attack attempts to guess passwords automatically?",
        "brute force",
        "Brute-force attacks repeatedly try different password combinations."
    ),

    (
        "True or False: Public Wi-Fi is always safe.",
        "false",
        "Public Wi-Fi can expose users to cyber threats."
    ),

    (
        "What is the practice of keeping software up to date called?",
        "patching",
        "Patching fixes security vulnerabilities."
    ),

    (
        "What should you do if you receive a suspicious attachment?",
        "delete",
        "Delete or report suspicious attachments instead of opening them."
    ),

    (
        "What is social engineering?",
        "manipulation",
        "Social engineering manipulates people into revealing confidential information."
    ),

    (
        "What security feature requires a second verification step after entering a password?",
        "mfa",
        "Multi-Factor Authentication provides additional protection."
    ),

    (
        "What should you create to protect important files from loss?",
        "backup",
        "Regular backups help recover data after accidents or cyberattacks."
    )
 };
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(logFile))
            {
                activityLog.AddRange(File.ReadAllLines(logFile));
            }

            try
            {
                DatabaseManager testDb = new DatabaseManager();

                MessageBox.Show(
                    "DatabaseManager loaded successfully.",
                    "Database Test",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            LoadBotLogo();

            sound.PlayWelcomeSound();

            try
            {
                LoadTasksFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }


            keywordResponses = new Dictionary<string, string[]>
{
    { "password", menu.PasswordResponses },
    { "phishing", menu.PhishingResponses },
    { "browser", menu.BrowsingResponses },
    { "virus", menu.MalwareResponses },
    { "malware", menu.MalwareResponses },
    { "privacy", menu.PrivacyResponses },
};
            InitializeReminderSystem();
            AddBotMessage(
                logo.GetAsciiBot() +
                "\n" +
                logo.WelcomeMessage() +
                "\n\n" +
                logo.AskName()
            );

        }
        private void LoadTasksFromDatabase()
        {
            TaskListBox.Items.Clear();

            tasks.Clear();

            reminderTasks.Clear();

            foreach (TaskItem task in db.GetTasks())
            {
                tasks.Add(task);

                if (!task.Completed)
                    reminderTasks.Add(task);

                TaskListBox.Items.Add(task);
            }

            taskCounter =
                tasks.Count > 0
                ? tasks.Max(t => t.Id) + 1
                : 1;
        }
        /// <summary>
        /// Loads the chatbot logo.
        /// </summary>
        private void LoadBotLogo()
        {
            try
            {
                BotLogoImage.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Images/botlogo.png")
                );
            }
            catch
            {
                AddBotMessage("Logo failed to load.");
            }
        }
        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }
        /// <summary>
        /// Processes all user input.
        /// </summary>
        private void ProcessUserInput()
        {
            string message = UserInputTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
                return;

            AddUserMessage(message);
            if (quizMode)
            {
                CheckQuizAnswer(message);
                UserInputTextBox.Clear();
                return;
            }

            UserInputTextBox.Clear();

            // First input = username
            if (string.IsNullOrEmpty(userName))
            {
                userName = message;

                memory.UserName = userName;
                memory.SaveMemory();

                AddBotMessage(
                    $"Welcome {userName}!\n\n" +
                    "I can help you learn about:\n\n" +
                    "• Password Safety\n" +
                    "• Phishing\n" +
                    "• Safe Browsing\n" +
                    "• Malware\n" +
                    "• Data Privacy\n\n" +
                    "Type a topic to begin.\n\n" +
                    "Type REMEMBER to view your favourite topic.\n" +
                    "Type EXIT to close the application."
                );

                return;
            }

            if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                AddBotMessage(bot.GoodbyeMessage(userName));

                Application.Current.Shutdown();

                return;
            }

            HandleSentiment(message);

            StoreUserInterest(message);

            ProcessNaturalLanguageCommand(message);

            string lower = message.ToLower();

            if (lower == "remember")
            {
                RecallUserInterest();
                activityLog.Add("Memory Recalled");
                return;
            }

            if (lower == "tell me more")
            {
                GiveMoreInformation();
                return;
            }

            if (HandleKeywords(message))
            {
                return;
            }

            AddBotMessage(bot.InvalidMessage());
        }
        /// <summary>
        /// Adds user messages to the chat.
        /// </summary>
        private void AddUserMessage(string message)
        {
            memory.AddConversation($"User: {message}");

            Border border = new Border
            {
                Background = Brushes.DodgerBlue,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 500
            };

            TextBlock text = new TextBlock
            {
                Text = $"[{DateTime.Now:HH:mm}] You:\n{message}",
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = text;

            ChatPanel.Children.Add(border);

            ChatScrollViewer.ScrollToEnd();
        }

        /// <summary>
        /// Adds bot messages to the chat.
        /// </summary>
        private void AddBotMessage(string message)
        {
            memory.AddConversation($"Bot: {message}");

            Border border = new Border
            {
                Background = Brushes.DimGray,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 600
            };

            TextBlock text = new TextBlock
            {
                Text = $"[{DateTime.Now:HH:mm}] Bot:\n{message}",
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = text;

            ChatPanel.Children.Add(border);

            ChatScrollViewer.ScrollToEnd();
        }

        /// <summary>
        /// Responds to emotional keywords.
        /// </summary>
        private void HandleSentiment(string message)
        {
            string response = sentiment.GetSentimentResponse(message);

            if (!string.IsNullOrEmpty(response))
            {
                AddBotMessage(response);
            }
        }

        /// <summary>
        /// Stores favourite cybersecurity topics.
        /// </summary>
        private void StoreUserInterest(string message)
        {
            string lower = message.ToLower();

            if (lower.Contains("password"))
                memory.FavouriteTopic = "Password Safety";

            else if (lower.Contains("phishing"))
                memory.FavouriteTopic = "Phishing";

            else if (lower.Contains("privacy"))
                memory.FavouriteTopic = "Privacy";

            else if (lower.Contains("virus") || lower.Contains("malware"))
                memory.FavouriteTopic = "Malware";

            else if (lower.Contains("browser"))
                memory.FavouriteTopic = "Safe Browsing";

            memory.SaveMemory();
        }

        /// <summary>
        /// Recalls the user's favourite topic.
        /// </summary>
        private void RecallUserInterest()
        {
            if (string.IsNullOrEmpty(memory.FavouriteTopic))
            {
                AddBotMessage(
                    "I do not have a favourite topic saved for you yet."
                );

                return;
            }

            AddBotMessage(
                $"I remember your favourite topic is:\n\n{memory.FavouriteTopic}"
            );
        }

        /// <summary>
        /// Handles cybersecurity keywords.
        /// </summary>
        private bool HandleKeywords(string message)
        {
            string lower = message.ToLower();

            foreach (var item in keywordResponses)
            {
                if (lower.Contains(item.Key))
                {
                    lastTopic = item.Key;

                    string response =
                        item.Value[random.Next(item.Value.Length)];

                    switch (item.Key)
                    {
                        case "password":
                            response +=
                                "\n\nType 'Tell me more' if you would like additional password security advice.";
                            break;

                        case "phishing":
                            response +=
                                "\n\nType 'Tell me more' if you would like to learn how to spot phishing attempts.";
                            break;

                        case "browser":
                            response +=
                                "\n\nType 'Tell me more' if you would like additional safe browsing tips.";
                            break;

                        case "virus":
                        case "malware":
                            response +=
                                "\n\nType 'Tell me more' if you would like to learn how malware spreads.";
                            break;

                        case "privacy":
                            response +=
                                "\n\nType 'Tell me more' if you would like additional online privacy advice.";
                            break;
                    }

                    AddBotMessage(response);

                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Provides additional information based on the
        /// most recently discussed cybersecurity topic.
        /// </summary>
        private void GiveMoreInformation()
        {
            switch (lastTopic)
            {
                case "password":

                    AddBotMessage(
                        @"Another password tip:

Avoid reusing passwords across multiple accounts.

If one website is hacked, attackers often try the same password on banking, email, and social media accounts.

A password manager can help you create and store unique passwords safely."
                    );

                    break;

                case "phishing":

                    AddBotMessage(
                        @"Phishing attacks often imitate trusted companies such as banks, Microsoft, Netflix, or PayPal.

Before entering personal information:

• Check the sender's email address
• Look for spelling mistakes
• Verify the website URL
• Be cautious of urgent messages"
                    );

                    break;

                case "browser":

                    AddBotMessage(
                        @"Safe browsing habits include:

• Keeping your browser updated
• Avoiding suspicious downloads
• Clearing browsing data regularly
• Using trusted websites
• Avoiding unknown pop-ups

These practices help reduce cybersecurity risks."
                    );

                    break;

                case "virus":
                case "malware":

                    AddBotMessage(
                        @"Many malware infections occur when users:

• Open suspicious attachments
• Download pirated software
• Click malicious advertisements
• Visit unsafe websites

Always download software from official sources and keep antivirus software updated."
                    );

                    break;

                case "privacy":

                    AddBotMessage(
                        @"To improve your online privacy:

• Review social media privacy settings
• Avoid sharing sensitive information publicly
• Use strong passwords
• Enable Multi-Factor Authentication
• Be selective about app permissions"
                    );

                    break;

                default:

                    AddBotMessage(
                        @"Please ask about one of these cybersecurity topics first:

• Password Safety
• Phishing
• Safe Browsing
• Malware
• Privacy"
                    );

                    break;
            }
        }
        // ================= BUTTON CLICK HANDLERS =================

        private void RememberButton_Click(object sender, RoutedEventArgs e)
        {
            RecallUserInterest();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTextBox.Text))
            {
                MessageBox.Show(
                    "Please enter a task.",
                    "Task Assistant",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            TaskItem task = new TaskItem
            {
                Id = taskCounter++,
                Title = TaskTextBox.Text,
                Description = TaskTextBox.Text,
                ReminderDate = ReminderDatePicker.SelectedDate
                               ?? DateTime.Now,
                Completed = false
            };

            tasks.Add(task);

            db.AddTask(task);

            reminderTasks.Add(task);

            TaskListBox.Items.Add(task);

            activityLog.Add(
                $"Task Added: {task.Title}"
            );

            AddBotMessage(
                $"Task '{task.Title}' added successfully."
            );

            TaskTextBox.Clear();
        }
        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogListBox.Items.Clear();

            if (activityLog.Count == 0)
            {
                AddBotMessage("No activity has been recorded yet.");
                return;
            }

            foreach (string item in activityLog.TakeLast(10))
            {
                ActivityLogListBox.Items.Add(item);
            }

            AddBotMessage("Activity log displayed.");
        }
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            SaveLog();
            quizMode = true;
            currentQuestion = 0;
            score = 0;

            activityLog.Add("Quiz Started");

            QuizScoreText.Text = $"0 / {quizQuestions.Count}";

            AddBotMessage("Cybersecurity Quiz Started!");

            AskNextQuestion();
        }
        private void AskNextQuestion()
        {
            if (currentQuestion < quizQuestions.Count)
            {
                AddBotMessage(
                    quizQuestions[currentQuestion].Question
                );
            }
            else
            {
                double percentage =
                    ((double)score / quizQuestions.Count) * 100;

                string feedback =
                    percentage >= 80 ? "Excellent! You have strong cybersecurity knowledge." :
                    percentage >= 60 ? "Good job! Your cybersecurity awareness is improving." :
                    percentage >= 40 ? "Fair attempt. Continue learning cybersecurity best practices." :
                    "You need more cybersecurity training. Review the topics and try again.";

                MessageBox.Show(
                    $"Quiz Complete!\n\nScore: {score}/{quizQuestions.Count}\n\n{feedback}",
                    "Quiz Finished",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // LOG ONLY ONCE
                activityLog.Add($"Quiz Completed: {score}/{quizQuestions.Count}");
                SaveLog();

                QuizScoreText.Text = $"{score} / {quizQuestions.Count}";

                quizMode = false;
            }
        }
        private void CheckQuizAnswer(string answer)
        {
            string correctAnswer =
                quizQuestions[currentQuestion]
                .Answer
                .ToLower();

            if (answer.Trim().ToLower() == correctAnswer)
            {
                score++;

                AddBotMessage(
                    "Correct!\n\n" +
                    quizQuestions[currentQuestion]
                    .Explanation
                );
            }
            else
            {
                AddBotMessage(
                    $"Incorrect.\n\nCorrect answer: {correctAnswer}\n\n" +
                    quizQuestions[currentQuestion]
                    .Explanation
                );
            }

            currentQuestion++;

            QuizScoreText.Text =
                $"{score} / {quizQuestions.Count}";

            AskNextQuestion();
        }
        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem task)
            {
                task.Completed = true;
                db.UpdateTask(task);
                activityLog.Add(
                    $"[{DateTime.Now:HH:mm}] Task Completed: {task.Title}"
                );

                TaskListBox.Items.Refresh();

                AddBotMessage(
                    $"Task '{task.Title}' marked as completed."
                );
            }
        }
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem task)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this task?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;

                tasks.Remove(task);
                TaskListBox.Items.Remove(task);
                db.DeleteTask(task.Id);

                activityLog.Add($"Task Deleted: {task.Title}");
                SaveLog();

                AddBotMessage($"Task '{task.Title}' deleted.");
            }
        }
        private void TellMoreButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cybersecurity helps protect computers, networks, and data.");
        }
        private void ReminderTimer_Tick(object? sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            foreach (var task in reminderTasks.ToList())
            {
                if (!task.Completed &&
                    task.ReminderDate <= now)
                {
                    MessageBox.Show(
                        $"Reminder!\n\nTask: {task.Title}",
                        "Cybersecurity Reminder",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    reminderTasks.Remove(task);
                }
            }
        }
        private void ProcessNaturalLanguageCommand(string message)
        {
            string input = message.ToLower();

            // ================= REMINDER COMMAND =================
            if (input.Contains("remind me") ||
                input.Contains("set reminder") ||
                input.Contains("create reminder"))
            {
                AddBotMessage("Use the Task Assistant panel to create reminders with date and time.");
                activityLog.Add($"[{DateTime.Now:HH:mm}] NLP: Reminder Request");
                SaveLog();
                return;
            }

            // ================= ADD TASK =================
            if (input.StartsWith("add task") ||
                input.StartsWith("create task") ||
                input.StartsWith("new task"))
            {
                string taskTitle = message
                    .Replace("add task", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("create task", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("new task", "", StringComparison.OrdinalIgnoreCase)
                    .Trim();

                if (!string.IsNullOrWhiteSpace(taskTitle))
                {
                    TaskItem task = new TaskItem
                    {
                        Id = taskCounter++,
                        Title = taskTitle,
                        Description = taskTitle,
                        ReminderDate = DateTime.Now.AddDays(1),
                        Completed = false
                    };

                    tasks.Add(task);
                    reminderTasks.Add(task);
                    db.AddTask(task);
                    TaskListBox.Items.Add(task);

                    AddBotMessage($"Task created: {task.Title}");

                    activityLog.Add($"[{DateTime.Now:HH:mm}] NLP Task Created: {task.Title}");
                    SaveLog();
                }
                else
                {
                    AddBotMessage("Please provide a task name.");
                }

                return;
            }

            // ================= COMPLETE TASK (NLP) =================
            if (input.Contains("complete task"))
            {
                string taskName = message.Replace("complete task", "", StringComparison.OrdinalIgnoreCase).Trim();

                var task = tasks.FirstOrDefault(t => t.Title.ToLower().Contains(taskName.ToLower()));

                if (task != null)
                {
                    task.Completed = true;
                    db.UpdateTask(task);

                    LoadTasksFromDatabase();

                    AddBotMessage($"Task marked complete: {task.Title}");

                    activityLog.Add($"[{DateTime.Now:HH:mm}] NLP Completed Task: {task.Title}");
                    SaveLog();
                }
                else
                {
                    AddBotMessage("Task not found.");
                }

                return;
            }

            // ================= DELETE TASK (NLP) =================
            if (input.Contains("delete task"))
            {
                string taskName = message.Replace("delete task", "", StringComparison.OrdinalIgnoreCase).Trim();

                var task = tasks.FirstOrDefault(t => t.Title.ToLower().Contains(taskName.ToLower()));

                if (task != null)
                {
                    tasks.Remove(task);
                    db.DeleteTask(task.Id);

                    LoadTasksFromDatabase();

                    AddBotMessage($"Task deleted: {task.Title}");

                    activityLog.Add($"[{DateTime.Now:HH:mm}] NLP Deleted Task: {task.Title}");
                    SaveLog();
                }
                else
                {
                    AddBotMessage("Task not found.");
                }

                return;
            }

            // ================= PASSWORD ADVICE =================
            if (input.Contains("password change") ||
                input.Contains("update password"))
            {
                AddBotMessage("Regular password changes improve security. Consider using a password manager.");
                activityLog.Add($"[{DateTime.Now:HH:mm}] NLP: Password Advice");
                SaveLog();
                return;
            }

            // ================= PRIVACY ADVICE =================
            if (input.Contains("privacy settings") ||
                input.Contains("review privacy"))
            {
                AddBotMessage("Review your privacy settings regularly to protect personal data.");
                activityLog.Add($"[{DateTime.Now:HH:mm}] NLP: Privacy Advice");
                SaveLog();
                return;
            }

            // ================= SHOW TASKS =================
            if (input.Contains("show tasks") || input.Contains("view tasks"))
            {
                if (tasks.Count == 0)
                {
                    AddBotMessage("No tasks available.");
                    return;
                }

                foreach (var t in tasks)
                {
                    AddBotMessage(t.ToString());
                }

                activityLog.Add($"[{DateTime.Now:HH:mm}] NLP: Show Tasks");
                SaveLog();
                return;
            }

            // ================= ACTIVITY LOG =================
            if (input.Contains("activity log") ||
                input.Contains("show activity") ||
                input.Contains("what have you done for me"))
            {
                ShowLogButton_Click(null, null);
                return;
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }
        private void SaveLog()
        {
            File.WriteAllLines(logFile, activityLog);
        }
        private void RestartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quizMode = false;
            currentQuestion = 0;
            score = 0;

            QuizScoreText.Text = "0 / " + quizQuestions.Count;

            AddBotMessage("Quiz restarted. Click Start Quiz to begin again.");
        }
    }
}
