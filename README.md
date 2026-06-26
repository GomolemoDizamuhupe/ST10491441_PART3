# Cyberbot — Cybersecurity Awareness Chatbot

A WPF desktop application built in C# that educates users on cybersecurity topics through an interactive chatbot, task manager, quiz game, and activity log.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Database Setup](#database-setup)
- [How to Run](#how-to-run)
- [How to Use](#how-to-use)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)

---

## Overview

Cyberbot is a GUI-based cybersecurity awareness assistant developed for PROG6221 (Programming 2A) as part of the Portfolio of Evidence (POE). It combines a conversational chatbot, a task assistant with database-backed reminders, a cybersecurity quiz game, and an activity log — all built on top of the WPF (Windows Presentation Foundation) framework.

---

## Features

### Chatbot (Parts 1 & 2)
- Greets users by name and remembers returning users
- Covers 9 cybersecurity topics: passwords, malware, phishing, 2FA, ransomware, safe browsing, privacy, VPN, and scams
- Rotates responses randomly so the same question gives varied answers
- Tracks which topics a user searches most and comments when interest is detected
- Remembers a user's favourite topic across the conversation
- Sentiment detection — responds empathetically to keywords like "worried", "frustrated", or "curious"
- Follow-up question support via keywords like "explain", "more", or "another tip"

### Task Assistant (Part 3 — Task 1)
- Add cybersecurity tasks via natural chat commands (e.g. `add task enable 2FA`)
- Auto-generates a relevant task description based on keywords in the task name
- Set reminders by days (e.g. `remind me in 7 days`) or by saying `tomorrow`
- View all tasks in a dedicated ListView showing title, description, due date, and status
- Double-click a task to mark it as **done**; double-click again to **delete** it
- All changes are saved to and loaded from a MySQL database

### Cybersecurity Quiz (Part 3 — Task 2)
- 15 questions covering phishing, passwords, VPNs, ransomware, safe browsing, scams, malware, 2FA, privacy, and firewalls
- Three question types: multiple choice, true/false, and typed keyword answers
- Immediate feedback with an explanation after every answer
- Score tracked throughout and displayed at the end with a performance message

### NLP Simulation (Part 3 — Task 3)
- Recognises user intent even when phrased differently (e.g. "add a task", "add task", "remind me to...")
- Regex used to extract numbers from natural language (e.g. "in 5 days")
- Handles "tomorrow" as a special case reminder
- Supports multiple phrasings for sentiment, favourite topics, and activity log commands

### Activity Log (Part 3 — Task 4)
- Logs every significant chatbot action with a timestamp
- Tracked actions include: tasks added, reminders set, quiz started/exited, sentiment detected, favourite topic set, task marked done, task deleted
- Type `show activity log` or `activity log` or click the **Activity Log** button to view the last 10 entries (newest first)
- Type `show full log` to see the complete history in chronological order

---

## Prerequisites

- Windows OS
- Visual Studio 2022 (or later) with the **.NET Desktop Development** workload installed
- .NET 6.0 or later
- MySQL Server (local instance)
- `MySql.Data` NuGet package (installed via NuGet Package Manager)

---

## Database Setup

1. Open MySQL Workbench (or any MySQL client) and run the following script to create the database and table:

```sql
CREATE DATABASE IF NOT EXISTS CyberChatbotDB;

USE CyberChatbotDB;

CREATE TABLE IF NOT EXISTS tasks (
    task_id INT AUTO_INCREMENT PRIMARY KEY,
    task_name VARCHAR(255),
    task_description VARCHAR(500),
    task_dueDate VARCHAR(100),
    task_status VARCHAR(50)
);
```

2. In the project, open `tasks.cs` and update the connection string to match your MySQL credentials:

```csharp
private string connection = "Server=localhost;Database=CyberChatbotDB;Uid=root;Pwd=YOUR_PASSWORD;";
```

---

## How to Run

1. Clone the repository:
   ```
   git clone https://github.com/YOUR_USERNAME/PART2_POE_.git
   ```
2. Open the solution file (`PART2_POE_.sln`) in Visual Studio.
3. Restore NuGet packages — right-click the solution in Solution Explorer and select **Restore NuGet Packages**.
4. Ensure MySQL is running and the database has been set up (see above).
5. Press **F5** or click **Start** to build and run the application.

---

## How to Use

| What you want to do | What to type |
|---|---|
| Add a task | `add task enable two-factor authentication` |
| Set a reminder | `remind me to update my password in 5 days` |
| Set a reminder for tomorrow | `remind me to check privacy settings tomorrow` |
| View tasks | Click the **View Tasks** button |
| Mark a task done | Double-click the task in the task list |
| Delete a task | Double-click a task already marked as done |
| Start the quiz | Type `quiz` or `game`, or click the **Quiz** button |
| Ask about a topic | Type `phishing`, `VPN`, `malware`, etc. |
| Show activity log | Type `show activity log` or click **Activity Log** |
| Show full history | Type `show full log` |
| Set favourite topic | Type `I'm interested in ransomware` |

---

## Project Structure

```
PART2_POE_/
│
├── MainWindow.xaml          # Main UI layout (all grids: welcome, username, chatbot, quiz, tasks)
├── MainWindow.xaml.cs       # Main interaction logic and NLP response handling
├── bot.cs                   # Cybersecurity topic responses, sentiment detection, NLP memory
├── tasks.cs                 # MySQL database helper — CRUD operations and ObservableCollection
├── ActivityLog.cs           # Dedicated activity log class with timestamped entries
├── Quiz_game.cs             # Quiz logic, question loading, answer handling, scoring
├── Message.cs               # Message model for chatbot bubble binding
├── SoundGreet.cs            # Audio greeting on startup
├── logo.png                 # Application logo
├── Greeting.wav             # Bot voice greeting audio
└── README.md                # This file
```

---

## Technologies Used

- **C#** — primary programming language
- **WPF (Windows Presentation Foundation)** — GUI framework using XAML
- **MySQL** — relational database for task storage
- **MySql.Data** — ADO.NET connector for MySQL
- **Regex** — used for number extraction in NLP reminder parsing
- **.NET 6+** — runtime framework
