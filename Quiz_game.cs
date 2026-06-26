using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PART2_POE_
{
    public class Quiz_game
    {
        private List<Question> questions = new List<Question>();
        private int currentQuestionIndex = 0;
        private int score = 0;

        // References to the XAML controls passed in from the window
        private TextBlock questionTitle;
        private TextBlock questionTxt;
        private Button btnA;
        private Button btnB;
        private Button btnC;
        private Button btnD;
        private TextBlock questionA;
        private TextBlock questionB;
        private TextBlock questionC;
        private TextBlock questionD;
        private TextBlock feedback;
        private TextBox txtKeyword;
        private Button btnSubmit;
        private Button btnNext;
        private Button btnExit;


        // Initialises the quiz and wires it to the UI controls from the window.
        public Quiz_game(
            TextBlock questionTitle,
            TextBlock questionTxt,
            Button btnA,
            Button btnB,
            Button btnC,
            Button btnD,
            TextBlock questionA,
            TextBlock questionB,
            TextBlock questionC,
            TextBlock questionD,
            TextBlock feedback,
            TextBox txtKeyword,
            Button btnSubmit,
            Button btnNext,
            Button btnExit)
        {
            this.questionTitle = questionTitle;
            this.questionTxt = questionTxt;
            this.btnA = btnA;
            this.btnB = btnB;
            this.btnC = btnC;
            this.btnD = btnD;
            this.questionA = questionA;
            this.questionB = questionB;
            this.questionC = questionC;
            this.questionD = questionD;
            this.feedback = feedback;
            this.txtKeyword = txtKeyword;
            this.btnSubmit = btnSubmit;
            this.btnNext = btnNext;
            this.btnExit = btnExit;

            LoadQuestions();
            ShowCurrentQuestion();
        }


        // Populates the question list with multiple choice, true/false, and keyword questions.
        private void LoadQuestions()
        {
            // Multiple choice: Question 1
            questions.Add(new Question
            {
                QuestionText = "What is phishing?",
                OptionA = "A type of outdoor fishing sport",
                OptionB = "Stealing sensitive info via fake messages",
                OptionC = "A secure file transfer protocol",
                OptionD = "A type of antivirus software",
                CorrectAnswer = "B",
                Explanation = "Phishing is a social engineering attack where attackers pretend to be trusted sources to steal credentials or personal data.",
                Type = QuestionType.MultipleChoice
            });

            // Multiple choice: Question 2
            questions.Add(new Question
            {
                QuestionText = "Which of these is the strongest password?",
                OptionA = "password123",
                OptionB = "John1990",
                OptionC = "Tr!8#kL@92z",
                OptionD = "qwerty",
                CorrectAnswer = "C",
                Explanation = "Strong passwords mix uppercase, lowercase, numbers and special characters, making them much harder to crack.",
                Type = QuestionType.MultipleChoice
            });

            // Multiple choice: Question 3
            questions.Add(new Question
            {
                QuestionText = "What does VPN stand for?",
                OptionA = "Virtual Private Network",
                OptionB = "Verified Public Node",
                OptionC = "Visual Programming Network",
                OptionD = "Variable Protocol Number",
                CorrectAnswer = "A",
                Explanation = "A VPN (Virtual Private Network) encrypts your internet connection and masks your IP address to protect your privacy online.",
                Type = QuestionType.MultipleChoice
            });

            // True or False: Question 4
            questions.Add(new Question
            {
                QuestionText = "True or False: Using the same password for all accounts is safe because it is easier to remember.",
                OptionA = "True",
                OptionB = "False",
                CorrectAnswer = "B",
                Explanation = "Reusing passwords is risky. If one account is breached, attackers can access all your other accounts with the same credentials.",
                Type = QuestionType.TrueFalse
            });

            // Keyword: Question 5
            questions.Add(new Question
            {
                QuestionText = "What term describes software designed to harm or gain unauthorised access to a computer system? (type your answer below)",
                CorrectAnswer = "malware",
                Explanation = "Malware (malicious software) is an umbrella term covering viruses, ransomware, worms and spyware — all built to damage or infiltrate systems without permission.",
                Type = QuestionType.Keyword
            });

            /*******************/
            // Multiple choice: Question 6
            questions.Add(new Question
            {
                QuestionText = "What is two-factor authentication (2FA)?",
                OptionA = "A password that contains two words",
                OptionB = "A security process requiring two forms of verification",
                OptionC = "Logging in from two different devices",
                OptionD = "A type of firewall protection",
                CorrectAnswer = "B",
                Explanation = "2FA adds a second verification step (like a one-time code) on top of your password, so attackers cannot access your account with a stolen password alone.",
                Type = QuestionType.MultipleChoice
            });

            // True or False: Question 7
            questions.Add(new Question
            {
                QuestionText = "True or False: It is safe to click a link in a pop-up warning that says your computer is infected and urges you to act immediately.",
                OptionA = "True",
                OptionB = "False",
                CorrectAnswer = "B",
                Explanation = "Fake pop-up warnings are a common safe browsing threat. Legitimate security software never asks you to click a browser pop-up — close it and run your own antivirus scan instead.",
                Type = QuestionType.TrueFalse
            });

            // Multiple choice: Question 8
            questions.Add(new Question
            {
                QuestionText = "Which of the following best describes ransomware?",
                OptionA = "Software that speeds up your computer",
                OptionB = "A virus that displays annoying ads",
                OptionC = "Malware that encrypts your files and demands payment",
                OptionD = "A tool used by IT departments to manage networks",
                CorrectAnswer = "C",
                Explanation = "Ransomware locks or encrypts your files and demands a ransom to restore access. Regular backups are your best defence — never pay the ransom.",
                Type = QuestionType.MultipleChoice
            });

            // Multiple choice: Question 9
            questions.Add(new Question
            {
                QuestionText = "What should you do when you receive an email asking for your password?",
                OptionA = "Reply with your password",
                OptionB = "Delete the email",
                OptionC = "Report it as phishing",
                OptionD = "Ignore it",
                CorrectAnswer = "C",
                Explanation = "Legitimate services never ask for your password via email. Reporting phishing emails helps protect others and alerts your email provider.",
                Type = QuestionType.MultipleChoice
            });

            // True or False: Question 10
            questions.Add(new Question
            {
                QuestionText = "True or False: Antivirus software alone is enough to keep your computer fully secure.",
                OptionA = "True",
                OptionB = "False",
                CorrectAnswer = "B",
                Explanation = "Antivirus is one layer of defence, but full security also requires strong passwords, software updates, safe browsing habits, and 2FA.",
                Type = QuestionType.TrueFalse
            });

            // Multiple choice: Question 11
            questions.Add(new Question
            {
                QuestionText = "Which of the following is a common warning sign of an online scam?",
                OptionA = "The website uses HTTPS",
                OptionB = "An offer that seems too good to be true",
                OptionC = "The sender's email matches the company domain",
                OptionD = "The website asks you to create an account",
                CorrectAnswer = "B",
                Explanation = "Scammers lure victims with unrealistically attractive offers. If a deal, prize, or opportunity seems too good to be true online, it almost certainly is.",
                Type = QuestionType.MultipleChoice
            });

            // Keyword: Question 12
            questions.Add(new Question
            {
                QuestionText = "What is the term for unsolicited bulk email, often used to spread phishing links or malware? (type your answer)",
                CorrectAnswer = "spam",
                Explanation = "Spam is unwanted mass email. Beyond being annoying, it is often a delivery mechanism for phishing attacks, scams, and malware-laced attachments.",
                Type = QuestionType.Keyword
            });

            // True or False: Question 13
            questions.Add(new Question
            {
                QuestionText = "True or False: HTTPS in a website URL means the site is completely safe and trustworthy.",
                OptionA = "True",
                OptionB = "False",
                CorrectAnswer = "B",
                Explanation = "HTTPS only means the connection between you and the site is encrypted. A phishing site can still use HTTPS — always verify the domain name carefully.",
                Type = QuestionType.TrueFalse
            });

            // Multiple choice: Question 14
            questions.Add(new Question
            {
                QuestionText = "How often should you update your passwords for important accounts?",
                OptionA = "Never — a strong password lasts forever",
                OptionB = "Only when you suspect a breach",
                OptionC = "Every few months or after any suspected breach",
                OptionD = "Once a year on your birthday",
                CorrectAnswer = "C",
                Explanation = "Rotating passwords every few months limits the damage if credentials are leaked in a data breach you are unaware of. A password manager makes this easy.",
                Type = QuestionType.MultipleChoice
            });

            // Keyword: Question 15
            questions.Add(new Question
            {
                QuestionText = "What is the name of a security tool that monitors and filters network traffic to block unauthorised access? (type your answer)",
                CorrectAnswer = "firewall",
                Explanation = "A firewall acts as a barrier between your trusted network and untrusted external networks, blocking suspicious traffic based on predefined security rules.",
                Type = QuestionType.Keyword
            });

        }


        // Reads the current question and configures the UI layout to match its type.
        public void ShowCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                Question current = questions[currentQuestionIndex];

                questionTitle.Text = "Question " + (currentQuestionIndex + 1) + " of " + questions.Count + ":";
                questionTxt.Text = current.QuestionText;
                feedback.Text = "";
                btnNext.Visibility = Visibility.Collapsed;

                if (current.Type == QuestionType.MultipleChoice)
                {
                    ShowMultipleChoiceLayout(current);
                }
                else if (current.Type == QuestionType.TrueFalse)
                {
                    ShowTrueFalseLayout(current);
                }
                else if (current.Type == QuestionType.Keyword)
                {
                    ShowKeywordLayout();
                }
            }
            else
            {
                ShowQuizComplete();
            }
        }


        // Shows all four A/B/C/D option buttons and their text labels.
        // Hides the keyword input controls.
        private void ShowMultipleChoiceLayout(Question current)
        {
            btnA.Visibility = Visibility.Visible;
            btnB.Visibility = Visibility.Visible;
            btnC.Visibility = Visibility.Visible;
            btnD.Visibility = Visibility.Visible;

            questionA.Visibility = Visibility.Visible;
            questionB.Visibility = Visibility.Visible;
            questionC.Visibility = Visibility.Visible;
            questionD.Visibility = Visibility.Visible;

            txtKeyword.Visibility = Visibility.Collapsed;
            btnSubmit.Visibility = Visibility.Collapsed;

            questionA.Text = current.OptionA;
            questionB.Text = current.OptionB;
            questionC.Text = current.OptionC;
            questionD.Text = current.OptionD;

            txtKeyword.Text = "";
        }


        // Shows only A (True) and B (False). Hides C, D, and the keyword controls.
        private void ShowTrueFalseLayout(Question current)
        {
            btnA.Visibility = Visibility.Visible;
            btnB.Visibility = Visibility.Visible;
            btnC.Visibility = Visibility.Collapsed;
            btnD.Visibility = Visibility.Collapsed;

            questionA.Visibility = Visibility.Visible;
            questionB.Visibility = Visibility.Visible;
            questionC.Visibility = Visibility.Collapsed;
            questionD.Visibility = Visibility.Collapsed;

            txtKeyword.Visibility = Visibility.Collapsed;
            btnSubmit.Visibility = Visibility.Collapsed;

            questionA.Text = current.OptionA;
            questionB.Text = current.OptionB;

            txtKeyword.Text = "";
        }


        // Hides all A/B/C/D buttons and reveals the text input and Submit button instead.
        private void ShowKeywordLayout()
        {
            btnA.Visibility = Visibility.Collapsed;
            btnB.Visibility = Visibility.Collapsed;
            btnC.Visibility = Visibility.Collapsed;
            btnD.Visibility = Visibility.Collapsed;

            questionA.Visibility = Visibility.Collapsed;
            questionB.Visibility = Visibility.Collapsed;
            questionC.Visibility = Visibility.Collapsed;
            questionD.Visibility = Visibility.Collapsed;

            txtKeyword.Visibility = Visibility.Visible;
            btnSubmit.Visibility = Visibility.Visible;

            txtKeyword.Text = "";
        }


        // Hides all input controls and displays the final score with a performance message.
        private void ShowQuizComplete()
        {
            questionTitle.Text = "Quiz Complete!";
            questionTxt.Text = "You scored " + score + " out of " + questions.Count + ".";
            feedback.Text = GetFinalMessage();

            btnA.Visibility = Visibility.Collapsed;
            btnB.Visibility = Visibility.Collapsed;
            btnC.Visibility = Visibility.Collapsed;
            btnD.Visibility = Visibility.Collapsed;

            questionA.Visibility = Visibility.Collapsed;
            questionB.Visibility = Visibility.Collapsed;
            questionC.Visibility = Visibility.Collapsed;
            questionD.Visibility = Visibility.Collapsed;

            txtKeyword.Visibility = Visibility.Collapsed;
            btnSubmit.Visibility = Visibility.Collapsed;
            btnNext.Visibility = Visibility.Collapsed;

            btnExit.Visibility = Visibility.Visible;
        }


        // Returns a performance summary message based on the final score.
        private string GetFinalMessage()
        {
            if (score == questions.Count)
            {
                return "Perfect score! Outstanding cybersecurity awareness!";
            }
            else if (score >= 3)
            {
                return "Good effort! Review the topics you missed to strengthen your knowledge.";
            }
            else
            {
                return "Keep learning! Cybersecurity knowledge is key to staying safe online.";
            }
        }


        // Evaluates a multiple choice or true/false answer.
        // Disables the option buttons and reveals the Next button after checking.
        public void HandleMultipleChoiceAnswer(string selectedOption)
        {
            if (currentQuestionIndex >= questions.Count)
            {
                return;
            }

            Question current = questions[currentQuestionIndex];

            if (selectedOption.ToUpper() == current.CorrectAnswer.ToUpper())
            {
                score++;
                feedback.Text = "✔ Correct! " + current.Explanation;
            }
            else
            {
                feedback.Text = "✘ Incorrect. The correct answer was " + current.CorrectAnswer + ". " + current.Explanation;
            }

            // Lock buttons so the player cannot change their answer
            btnA.IsEnabled = false;
            btnB.IsEnabled = false;
            btnC.IsEnabled = false;
            btnD.IsEnabled = false;

            currentQuestionIndex++;
            btnNext.Visibility = Visibility.Visible;
        }


        // Evaluates a typed keyword answer.
        // Locks the input and reveals the Next button after checking.
        public void HandleKeywordAnswer(string userInput)
        {
            if (currentQuestionIndex >= questions.Count)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(userInput))
            {
                feedback.Text = "⚠ Please type an answer before submitting.";
                return;
            }

            Question current = questions[currentQuestionIndex];
            string trimmedInput = userInput.Trim().ToLower();
            string correctAnswer = current.CorrectAnswer.ToLower();

            if (trimmedInput == correctAnswer)
            {
                score++;
                feedback.Text = "✔ Correct! " + current.Explanation;
            }
            else
            {
                feedback.Text = "✘ Incorrect. The correct answer was '" + current.CorrectAnswer + "'. " + current.Explanation;
            }

            // Lock the input so the player cannot resubmit
            btnSubmit.IsEnabled = false;
            txtKeyword.IsReadOnly = true;

            currentQuestionIndex++;
            btnNext.Visibility = Visibility.Visible;
        }


        // Re-enables all controls and advances to the next question.
        // Called by the Next button in the window.
        public void MoveToNextQuestion()
        {
            btnA.IsEnabled = true;
            btnB.IsEnabled = true;
            btnC.IsEnabled = true;
            btnD.IsEnabled = true;
            btnSubmit.IsEnabled = true;
            txtKeyword.IsReadOnly = false;

            ShowCurrentQuestion();
        }
    }
}