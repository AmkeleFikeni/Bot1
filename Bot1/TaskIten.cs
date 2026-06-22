using System;

namespace Bot1
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime ReminderDate { get; set; }

        public bool Completed { get; set; }

        public override string ToString()
        {
            return $"{Title} | Due: {ReminderDate:d} | " +
                   (Completed ? "Completed" : "Pending");
        }
    }
}