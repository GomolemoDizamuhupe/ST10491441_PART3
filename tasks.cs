using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PART2_POE_
{
    public class tasks
    {

        /*********************************************/
        public string task_num { get; set; }
        public string task_title { get; set; }
        public string task_description { get; set; }
        public string task_date { get; set; }
        public string task_status { get; set; }

        //Stores the color of the messages
        public Brush MessageColor { get; set; }



        /***************************************************/

        //global connecrion string, with variable declaration
        string connection = @"Data source=(localdb)\MSSQLLocalDB;Database=prog_task_1";

        //method test the connection
        public void test_conection()
        {
            /*SqlConnection - used to make connection woth database
             * SqlCommand - used to run queries, all of them
             * SqlDataReader - used to read what is collected by 
             *                  the sqlCommeand and show the user data
             */

            //Connect to the database
            using (SqlConnection connect = new SqlConnection(connection))
            {
                //try and catch any error that it will throw
                try
                {//Open the connection and close the connection
                    connect.Open();

                    //put the database query and run it
                    MessageBox.Show("Connected...");
                }
                catch (Exception error)
                {//Show message error
                    MessageBox.Show(error.Message);
                }
            }
        }


        //method to insert or store the tasks
        public void insert_task(string name, string description, string dueDate, string status)
        {
            try
            {
                using (SqlConnection connects = new SqlConnection(connection))
                {
                    connects.Open();

                    //Explicit column list + parameters: avoids SQL injection AND
                    //avoids problems if the task name/description contains an apostrophe.
                    string query = "INSERT INTO tasks (task_name, task_description, task_dueDate, task_status) " +
                                   "VALUES (@name, @description, @dueDate, @status)";

                    SqlCommand run_query = new SqlCommand(query, connects);
                    run_query.Parameters.AddWithValue("@name", (object)name ?? DBNull.Value);
                    run_query.Parameters.AddWithValue("@description", (object)description ?? DBNull.Value);
                    run_query.Parameters.AddWithValue("@dueDate", (object)dueDate ?? DBNull.Value);
                    run_query.Parameters.AddWithValue("@status", (object)status ?? DBNull.Value);

                    run_query.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        /*SqlConnection - used to make connection woth database
 * SqlCommand - used to run queries, all of them
 * SqlDataReader - used to read what is collected by 
 *                  the sqlCommeand and show the user data
 */

        //Method to auto load the tasks of a user.
        //Populates the Tasks ObservableCollection, which the UI binds to.
        public void load_tasks()
        {
            try
            {
                //Clear out whatever was there before, otherwise every reload
                //appends the same rows again and you get duplicates.
                Tasks.Clear();

                using (SqlConnection connects = new SqlConnection(connection))
                {
                    connects.Open();

                    string query = "SELECT * FROM tasks;";

                    SqlCommand run_query = new SqlCommand(query, connects);

                    using (SqlDataReader data_collect = run_query.ExecuteReader())
                    {
                        while (data_collect.Read())
                        {
                            string task_id = data_collect["task_id"].ToString();
                            string task_name = data_collect["task_name"].ToString();
                            string task_description = data_collect["task_description"].ToString();
                            string task_dueDate = data_collect["task_dueDate"].ToString();
                            string task_status = data_collect["task_status"].ToString();

                            taskTemp(task_id, task_name, task_description, task_dueDate, task_status);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }


        public ObservableCollection<tasks> Tasks { get; set; } = new ObservableCollection<tasks>();
        /*******************************/

        private void taskTemp(string task_num, string task_title, string task_description, string task_date, string task_status)
        {
            Tasks.Add(new tasks
            {
                task_num = task_num,
                task_title = task_title,
                task_description = task_description,
                task_date = task_date,
                task_status = task_status,

                MessageColor = Brushes.LimeGreen
            });
        }


        /*******************************/

        //method to update tasks
        public void update_taskStatus(int id)
        {
            try
            {
                using (SqlConnection connects = new SqlConnection(connection))
                {
                    connects.Open();

                    string query = "UPDATE tasks SET task_status = 'done' WHERE task_id = @id";

                    SqlCommand run_query = new SqlCommand(query, connects);
                    run_query.Parameters.AddWithValue("@id", id);

                    run_query.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        //method to delete tasks
        public void delete_task(int id)
        {
            try
            {
                using (SqlConnection connects = new SqlConnection(connection))
                {
                    connects.Open();

                    string query = "DELETE FROM tasks WHERE task_id = @id";

                    SqlCommand run_query = new SqlCommand(query, connects);
                    run_query.Parameters.AddWithValue("@id", id);

                    run_query.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }


        }


    }
}