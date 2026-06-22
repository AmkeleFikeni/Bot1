using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
namespace Bot1
{
    public class DatabaseManager
    {
        private readonly string connectionString =
    "Server=localhost;Port=3306;Database=CyberBotDB;Uid=root;Pwd=;SslMode=None;";

        public void AddTask(TaskItem task)
        {
            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                    @"INSERT INTO Tasks
            (Title, Description, ReminderDate, Completed)
            VALUES
            (@Title,@Description,@ReminderDate,@Completed)";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@ReminderDate", task.ReminderDate);
                    cmd.Parameters.AddWithValue("@Completed", task.Completed);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new();

            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM Tasks";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool completed = false;

                            string completedValue =
                                reader["Completed"]?.ToString() ?? "0";

                            if (completedValue == "1" ||
                                completedValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                completed = true;
                            }

                            tasks.Add(new TaskItem
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Title = reader["Title"]?.ToString() ?? "",
                                Description = reader["Description"]?.ToString() ?? "",
                                ReminderDate = Convert.ToDateTime(reader["ReminderDate"]),
                                Completed = completed
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return tasks;
        }


        public void UpdateTask(TaskItem task)
        {
            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                    @"UPDATE Tasks
            SET Title=@Title,
                Description=@Description,
                ReminderDate=@ReminderDate,
                Completed=@Completed
            WHERE Id=@Id";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@ReminderDate", task.ReminderDate);
                    cmd.Parameters.AddWithValue("@Completed", task.Completed);
                    cmd.Parameters.AddWithValue("@Id", task.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                        "DELETE FROM Tasks WHERE Id=@Id";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}