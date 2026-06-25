CYBERSECURITY AWARENESS CHATBOT (POE PROJECT)
1. PROJECT OVERVIEW
This project is a Cybersecurity Awareness Chatbot System developed using:
C# Windows Presentation Foundation (WPF)
MySQL Database
XAML UI Design
The system is a GUI-based application that helps users learn cybersecurity concepts through:
Chatbot interaction
Task management system
Cybersecurity quiz game
Activity logging system
Natural Language Processing (NLP) simulation
2. PROJECT OBJECTIVE
The objective of this system is to:
Educate users about cybersecurity awareness
Provide an interactive learning environment
Simulate real-world chatbot behaviour using NLP
Store and manage user tasks using a database
Track all user activities for accountability
3. SYSTEM FEATURES (REQUIRED BY POE RUBRIC)
3.1 Chatbot System (NLP Simulation)
Accepts natural language input
Uses keyword detection (e.g. task, quiz, reminder, password)
Responds dynamically based on user input
Supports flexible user commands
3.2 Task Assistant (WITH DATABASE + REMINDERS)

The system allows users to:

Add tasks (e.g. "Enable 2FA")
Set reminders for tasks
Mark tasks as complete
Delete tasks
View all tasks
 All tasks are stored in a MySQL database
CRUD operations are fully supported:

Create
Read
Update
Delete
3.3 Cybersecurity Quiz Game

The quiz system includes:

10–15 cybersecurity questions
Multiple choice and true/false questions
One question displayed at a time
Instant feedback after each answer
Final score calculation
Performance feedback message

Example feedback:

“Great job! You are a cybersecurity pro!”
“Keep learning to stay safe online!”
 3.4 Activity Log System

The activity log records:

Task added
Task completed
Task deleted
Quiz started/completed
NLP-based actions

Features:

Displays last 5–10 actions
Helps user track system activity
Improves transparency of chatbot actions
4. NATURAL LANGUAGE PROCESSING (NLP SIMULATION)

The chatbot uses keyword detection and string matching to simulate NLP.

It recognizes phrases such as:

“add task”
“remind me”
“delete task”
“start quiz”
“phishing”
“password safety”

This allows flexible user input instead of strict commands.

5.  DATABASE INTEGRATION (MySQL)

The system uses MySQL for:

Storing tasks
Updating task status
Deleting tasks
Retrieving task history
CRUD Operations:
INSERT (Add Task)
SELECT (View Tasks)
UPDATE (Mark Complete)
DELETE (Remove Task)

Ensures persistent data storage

6. USER INTERFACE (GUI REQUIREMENT)

The system is built using WPF with:

Background image (cover.png)
Application logo (botlogo.png)
Chat interface
Task panel
Activity log panel
UI Features:
Clean layout (3-column design)
Interactive buttons
Scrollable chat area
Styled panels for usability
7.  SYSTEM TESTING / FUNCTIONALITY

The system successfully demonstrates:

Chatbot interaction
NLP command recognition
Task management with database
Quiz system with scoring
Activity logging
GUI responsiveness

8. INSTALLATION GUIDE
Requirements:
Visual Studio (C# WPF support)
MySQL Server
.NET Framework
Steps:
Open project in Visual Studio
Start MySQL server
Create database (CyberBotDB)
Configure connection string
Run application
9. IMPORTANT NOTES (POE REQUIREMENT)
Images folder must contain:
cover.png
botlogo.png
Set image Build Action = Resource
Ensure database is running before launching system
Application must be GUI-based (NOT console)
10.  VIDEO PRESENTATION

A video demonstration will show:

Chatbot functionality
Task system
Quiz system
Activity log
Database integration
NLP simulation
11. CONCLUSION

This project successfully demonstrates a fully functional Cybersecurity Awareness Chatbot System that integrates:

Artificial Intelligence simulation (NLP)
Database management
Interactive learning (quiz)
Task automation
Activity tracking.
