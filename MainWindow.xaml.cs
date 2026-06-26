using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PART2_POE_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bot botManager = new bot();
        string voicePath;
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        SoundGreet SoundManager = new SoundGreet();

        private Quiz_game quizGame;

        // Create an instance for the class tasks
        // with an object name manage_tasks
        tasks manage_tasks = new tasks();

        // Exposes manage_tasks' collection so the ListView in XAML
        // (which binds to this window's DataContext) can actually see it.
        public ObservableCollection<tasks> TaskList => manage_tasks.Tasks;

        // Global variables to hold the task details
        string task_name, task_description, task_dueDate, task_status = string.Empty;

        string reminder, days_number, format_date = string.Empty;

        int days;

        DateTime user_reminder;

        // Activity Log 
        // Dedicated ActivityLog instance — all logging goes through this class.
        private ActivityLog activityLog = new ActivityLog();


        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            // Playing audio (Bot voice)
            voicePath = "Greeting.wav";
            SoundManager.botVoice(voicePath);

            /*Quiz Game*/
            quizGame = new Quiz_game(
                questionTitle,
                questionTxt,
                btnA, btnB, btnC, btnD,
                questionA, questionB, questionC, questionD,
                feedback,
                txtKeyword,
                btnSubmit,
                btnNext,
                btnExit
            );
        }

        private void start_bot(object sender, RoutedEventArgs e)
        {
            welcome_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }


        // Message layout helpers

        // User message layout method
        private void AddUserMessage(string text, string sender)
        {
            Messages.Add(new Message
            {
                Time = DateTime.Now.ToString("HH:mm"),
                Text = text,
                Sender = sender,
                MessageColor = Brushes.LimeGreen
            });
        }

        // Bot message layout method
        private void AddBotMessage(string text, string sender)
        {
            Messages.Add(new Message
            {
                Time = DateTime.Now.ToString("HH:mm"),
                Text = text,
                Sender = sender,
                MessageColor = Brushes.LimeGreen
            });
        }

        // Username submission grid event handler
        private void submit_username(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();

            if (!string.IsNullOrWhiteSpace(username))
            {
                if (HasSpecialChars(username))
                {
                    string filePath = "Usernames.txt";

                    bool returningUser = DoesUserExists(username, filePath);

                    if (!returningUser)
                    {
                        File.AppendAllText(filePath, username + "\n");
                    }

                    username_grid.Visibility = Visibility.Hidden;
                    chatbot_grid.Visibility = Visibility.Visible;

                    if (returningUser)
                    {
                        AddBotMessage($"Hi welcome back {username}. I hope you still remember me, my name is Cyberbot.", "Bot");
                    }
                    else
                    {
                        AddBotMessage($"Hi {username}, my name is Cyberbot.", "Bot");
                    }

                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                    AddBotMessage($"Let me tell you a bit about myself, I'm here to help you with online password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy, VPN and scam.", "Bot");
                    AddBotMessage($"How can I help you {username} ? ", "Bot");
                }
                else
                {
                    MessageBox.Show("Your name must not contain numbers[0-9] and special characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Clear();
                    txtUsername.Focus();
                }
            }
            else
            {
                MessageBox.Show("Enter your name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Clear();
                txtUsername.Focus();
            }
        }

        // Method that checks if the file exists
        public bool DoesUserExists(string username, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            string users = File.ReadAllText(filePath);
            return users.Contains(username);
        }


        // Submit button for user response event handler
        private void submit_response(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();

            string chat_response = string.Empty;
            string user_response = txtUserResponse.Text.Trim().ToLower();

            logicResponse(username, chat_response, user_response);
        }

        // Method checks if the string has an integer or special character
        private bool HasSpecialChars(string userName)
        {
            var pattern = @"^[A-Za-z]+$";
            return Regex.IsMatch(userName, pattern);
        }


        // display the activity log in chat 
        //Shared display logic used by both the chat command and the ACTIVITY LOG button.
        //Shows the last 10 entries (newest first), with an offer to see the full log.
        private void DisplayActivityLog()
        {
            if (activityLog.Count == 0)
            {
                AddBotMessage("No actions have been recorded yet. Try adding a task, setting a reminder, or playing the quiz!", "Bot");
                return;
            }

            List<string> recent = activityLog.GetRecent(10);

            AddBotMessage("Here's a summary of recent actions:", "Bot");

            for (int i = 0; i < recent.Count; i++)
            {
                AddBotMessage($"  {i + 1}. {recent[i]}", "Bot");
            }

            // Offer "show full log" if there are older entries beyond the 10 shown
            if (activityLog.Count > 10)
            {
                AddBotMessage($"  ... and {activityLog.Count - 10} older action(s). Type 'show full log' to see everything.", "Bot");
            }
        }

        // Displays the complete activity log in chat (all entries, oldest first).
        private void DisplayFullActivityLog()
        {
            if (activityLog.Count == 0)
            {
                AddBotMessage("No actions recorded yet.", "Bot");
                return;
            }

            List<string> all = activityLog.GetAll();

            AddBotMessage($"Full activity log ({activityLog.Count} action(s)):", "Bot");

            for (int i = 0; i < all.Count; i++)
            {
                AddBotMessage($"  {i + 1}. {all[i]}", "Bot");
            }
        }


        // response logic
        private void logicResponse(string username, string chat_response, string user_response)
        {
            task_title.Text = $"YOUR TASKS {username} :";

            if (!string.IsNullOrWhiteSpace(txtUserResponse.Text))
            {
                AddUserMessage(user_response, username);


                // Show activity log (last 10 actions)
                if (user_response.Contains("show activity log") ||
                    user_response.Contains("what have you done") ||
                    user_response.Contains("activity log") ||
                    user_response.Contains("show log"))
                {
                    DisplayActivityLog();

                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }

                //Show full activity log (all entries)
                else if (user_response.Contains("show full log"))
                {
                    DisplayFullActivityLog();

                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }

                //Reminder / remind keyword
                else if (user_response.Contains("remind") || user_response.Contains("reminder"))
                {
                    // Strip the trigger word to get the task name
                    task_name = user_response.Replace("remind", "").Replace("reminder", "").Trim();

                    // Generate a description based on keywords in the task name
                    task_description = get_task_description(task_name);

                    if (user_response.Contains("tomorrow"))
                    {
                        reminder = "1";

                        // Extract the number from the reminder string
                        days_number = Regex.Replace(reminder, @"[^0-9]", "");

                        // Cast days_number to an int
                        days = int.Parse(days_number);

                        // Add the days to the current date
                        user_reminder = DateTime.Now.AddDays(days);

                        // Format the date (e.g. June 27 2026)
                        format_date = user_reminder.ToString("MMMM dd yyyy");

                        // Assign formatted date and default status
                        task_dueDate = format_date;
                        task_status = "pending";

                        chat_response = $"Reminder set for '{task_name}' on tomorrow's date({format_date}).";

                        AddBotMessage(chat_response, "Bot");
                        txtUserResponse.Clear();
                        txtUserResponse.Focus();

                        manage_tasks.insert_task(task_name, task_description, task_dueDate, task_status);

                        //Log the action 
                        activityLog.LogAction($"Reminder set: '{task_name}' for tomorrow ({format_date}).");
                    }
                    else
                    {
                        reminder = user_response;

                        // Extract the number from the reminder string
                        days_number = Regex.Replace(reminder, @"[^0-9]", "");

                        // Cast days_number to an int
                        days = int.Parse(days_number);

                        // Add the days to the current date
                        user_reminder = DateTime.Now.AddDays(days);

                        // Format the date (e.g. June 27 2026)
                        format_date = user_reminder.ToString("MMMM dd yyyy");

                        // Assign formatted date and default status
                        task_dueDate = format_date;
                        task_status = "pending";

                        chat_response = $"Reminder set for '{task_name}' in {days} day(s), which will be on {format_date}.";

                        AddBotMessage(chat_response, "Bot");
                        txtUserResponse.Clear();
                        txtUserResponse.Focus();

                        manage_tasks.insert_task(task_name, task_description, task_dueDate, task_status);

                        //Log the action
                        activityLog.LogAction($"Reminder set: '{task_name}' in {days} day(s), due {format_date}.");
                    }
                }

                //Add task keyword
                else if (user_response.Contains("add task") || user_response.Contains("add a task"))
                {
                    // Strip the trigger phrase to get the task name
                    task_name = user_response.Replace("add task", "").Replace("add a task", "").Trim();

                    // Generate a description based on keywords in the task name
                    task_description = get_task_description(task_name);

                    chat_response = $"Task is added with description \"{task_description}.\" Would you like a reminder ?";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();

                    //Log the action
                    activityLog.LogAction($"Task added: '{task_name}' — awaiting reminder response.");
                }

                // User confirms reminder (yes branch)
                else if (user_response.StartsWith("yes") || user_response.Contains("yes"))
                {
                    reminder = user_response.Replace("yes, remind me in", "");

                    // Extract the number from the reminder string
                    days_number = Regex.Replace(reminder, @"[^0-9]", "");

                    // Cast days_number to an int
                    days = int.Parse(days_number);

                    // Add the days to the current date
                    user_reminder = DateTime.Now.AddDays(days);

                    // Format the date (e.g. June 27 2026)
                    format_date = user_reminder.ToString("MMMM dd yyyy");

                    // Assign formatted date and default status
                    task_dueDate = format_date;
                    task_status = "pending";

                    chat_response = $"Got it! I'll remind you in {days} days, which will be on the {format_date}.";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();

                    manage_tasks.insert_task(task_name, task_description, task_dueDate, task_status);

                    //Log the action
                    activityLog.LogAction($"Reminder confirmed for '{task_name}', due in {days} day(s) on {format_date}.");
                }

                //User declines reminder (no branch)
                else if (user_response.Contains("no"))
                {
                    // Default to today (0 days added)
                    string days_number_local = "0";
                    days = int.Parse(days_number_local);

                    user_reminder = DateTime.Now.AddDays(days);
                    string format_date_local = user_reminder.ToString("MMMM dd yyyy");

                    task_dueDate = format_date_local;
                    task_status = "pending";

                    chat_response = "Ohk, your task was added successfully.";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();

                    manage_tasks.insert_task(task_name, task_description, task_dueDate, task_status);

                    //Log the action
                    activityLog.LogAction($"Task added without reminder: '{task_name}'.");
                }

                //Favourite topic / memory
                else if (user_response.Contains("i'm interested in") ||
                    user_response.Contains("im interested in") ||
                    user_response.Contains("i am interested in") ||
                    user_response.Contains("my favourite topic is") ||
                    user_response.Contains("my favorite topic is"))
                {
                    chat_response = botManager.SetUserFavouriteTopic(user_response, username);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();

                    //Log the action
                    activityLog.LogAction($"NLP: User set favourite topic from input — '{user_response}'.");

                    chat_response = "I hope you are enjoying, learning about cybersecurity topics. You can test your knowledge with our quiz game, to open it you can click the Quiz button or type 'Quiz' or 'Game'.";
                    AddBotMessage(chat_response, "Bot");
                }

                //Sentiment detection
                else if (user_response.Contains("worried") ||
                    user_response.Contains("curious") ||
                    user_response.Contains("frustrated") ||
                    user_response.Contains("stressed"))
                {
                    chat_response = botManager.sentimentDetection(user_response, username);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();

                    //Log the action
                    activityLog.LogAction($"NLP: Sentiment detected in user input — responded with empathetic message.");
                }

                //Cybersecurity keyword topics 
                else if (user_response.Contains("password") ||
                    user_response.Contains("malware") ||
                    user_response.Contains("2fa") ||
                    user_response.Contains("phishing") ||
                    user_response.Contains("ransomware") ||
                    user_response.Contains("safe browsing") ||
                    user_response.Contains("privacy") ||
                    user_response.Contains("scam") ||
                    user_response.Contains("vpn"))
                {
                    chat_response = botManager.GetTopic(user_response);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }

                else if (user_response.Contains("my name"))
                {
                    chat_response = $"Your name is {username}";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("purpose"))
                {
                    chat_response = "My purpose is to help you with online safety topics like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy, VPN and scam. ";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("how are you"))
                {
                    chat_response = $"I'm great {username}, how can I help you with online safety topics like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy, VPN and scam? ";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }

                //Quiz / game keyword 
                else if (user_response.Contains("quiz") ||
                    user_response.Contains("game"))
                {
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();

                    new Quiz_game(
                        questionTitle,
                        questionTxt,
                        btnA, btnB, btnC, btnD,
                        questionA, questionB, questionC, questionD,
                        feedback,
                        txtKeyword,
                        btnSubmit,
                        btnNext,
                        btnExit
                    );

                    //Log the action
                    activityLog.LogAction("Quiz game started via chat command.");

                    chatbot_grid.Visibility = Visibility.Hidden;
                    quiz_grid.Visibility = Visibility.Visible;
                }

                else if (user_response.Contains("i ask"))
                {
                    chat_response = "You can ask me about with online safety topics like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy, VPN and scam. ";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("explain") ||
                    user_response.Contains("another tip") ||
                    user_response.Contains("more"))
                {
                    chat_response = botManager.followUpQuestions(user_response, username);

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else if (user_response.Contains("hello") ||
                    user_response.Contains("hi") ||
                    user_response.Contains("hey"))
                {
                    chat_response = $"Hello {username}, I hope you are doing good.\nYou can ask me about online safety topics like password safety, phishing, safe browsing, 2FA, malware, ransomware, privacy, VPN and scam.";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
                else
                {
                    chat_response = "I didn't quite understand that. Could you rephrase?";

                    AddBotMessage(chat_response, "Bot");
                    txtUserResponse.Clear();
                    txtUserResponse.Focus();
                }
            }
            else
            {
                MessageBox.Show($"Please enter something {username}.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Clear();
                txtUsername.Focus();
            }
        }

        // Exit button to exit the chatbot
        private void Exit_chatbot(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Quiz game button event handler
        private void quiz_game(object sender, RoutedEventArgs e)
        {
            new Quiz_game(
                questionTitle,
                questionTxt,
                btnA, btnB, btnC, btnD,
                questionA, questionB, questionC, questionD,
                feedback,
                txtKeyword,
                btnSubmit,
                btnNext,
                btnExit
            );

            //Log the action
            activityLog.LogAction("Quiz game opened via Quiz button.");

            chatbot_grid.Visibility = Visibility.Hidden;
            quiz_grid.Visibility = Visibility.Visible;
        }


        //Quiz button event handlers
        private void btnA_Click(object sender, RoutedEventArgs e)
        {
            quizGame.HandleMultipleChoiceAnswer("A");
        }

        private void btnB_Click(object sender, RoutedEventArgs e)
        {
            quizGame.HandleMultipleChoiceAnswer("B");
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            quizGame.HandleMultipleChoiceAnswer("C");
        }

        private void btnD_Click(object sender, RoutedEventArgs e)
        {
            quizGame.HandleMultipleChoiceAnswer("D");
        }

        // Submit answer button event handler
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            quizGame.HandleKeywordAnswer(txtKeyword.Text);
        }

        // Next question button event handler
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            quizGame.MoveToNextQuestion();
        }

        // Exit quiz game event handler
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Log the action 
            activityLog.LogAction("Quiz game completed/exited by user.");

            chatbot_grid.Visibility = Visibility.Visible;
            quiz_grid.Visibility = Visibility.Hidden;
        }

        //ACTIVITY LOG button event handler
        // Clicking the ACTIVITY LOG button displays the recent log directly in chat,
        // identical to typing "show activity log".
        private void Activity_Logs(object sender, RoutedEventArgs e)
        {
            DisplayActivityLog();
        }


        // Method to generate a relevant task description based on keywords in the task name
        private string get_task_description(string task_name)
        {
            string name_lower = task_name.ToLower();

            if (name_lower.Contains("password"))
            {
                return "Update or strengthen your password to keep your accounts secure.";
            }
            else if (name_lower.Contains("2fa") || name_lower.Contains("two-factor") || name_lower.Contains("two factor"))
            {
                return "Enable two-factor authentication for an extra layer of account security.";
            }
            else if (name_lower.Contains("privacy"))
            {
                return "Review your account privacy settings to ensure your data is protected.";
            }
            else if (name_lower.Contains("phishing") || name_lower.Contains("scam"))
            {
                return "Stay alert for phishing emails and suspicious links that try to steal your information.";
            }
            else
            {
                return "General cybersecurity task to help you stay safe online.";
            }
        }

        // Back to chats button event handler
        private void back_to_chats(object sender, RoutedEventArgs e)
        {
            chatbot_grid.Visibility = Visibility.Visible;
            viewTask_grid.Visibility = Visibility.Hidden;
        }

        // View all tasks button event handler
        private void view_allTask(object sender, RoutedEventArgs e)
        {
            chatbot_grid.Visibility = Visibility.Hidden;
            viewTask_grid.Visibility = Visibility.Visible;

            // Auto-load tasks from the database
            autoLoad_task();
        }

        // Manage task event handler — mark done or delete.
        // Triggered by double-clicking a row in the view_task ListView.
        private void manage_task(object sender, MouseButtonEventArgs e)
        {
            // SelectedItem is the actual "tasks" object bound to the row
            // that was double-clicked — no string parsing needed.
            if (view_task.SelectedItem is tasks selectedTask)
            {
                if (!int.TryParse(selectedTask.task_num, out int id))
                {
                    MessageBox.Show("Could not read this task's ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool isDone = !string.IsNullOrEmpty(selectedTask.task_status) &&
                              selectedTask.task_status.Trim().ToLower() == "done";

                if (isDone)
                {
                    // Already done → double-click again removes it
                    manage_tasks.delete_task(id);

                    //Log the action
                    activityLog.LogAction($"Task deleted: '{selectedTask.task_title}' (was already marked as done).");
                }
                else
                {
                    // Not done yet → mark it done
                    manage_tasks.update_taskStatus(id);

                    //Log the action 
                    activityLog.LogAction($"Task marked as done: '{selectedTask.task_title}'.");
                }

                // Refresh the task list
                autoLoad_task();
            }
        }

        // Method to auto-load all tasks from the database into the ListView
        private void autoLoad_task()
        {
            // load_tasks() clears and repopulates manage_tasks.Tasks itself,
            // which the ListView is bound to via the TaskList property.
            manage_tasks.load_tasks();
        }
    }


    // ── Message model ─────────────────────────────────────────────────────────

    // ─────────────────────────────────────────────────────────────────────────
}