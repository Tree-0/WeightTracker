using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace WeightTracker
{
    public partial class Form1 : Form
    {
        // Exercises are added to this list upon clicking the "add exercise" button
        // This list is used to instantiate a Workout object when the "submit" button is pressed
        // After each workout is submitted, this list is cleared and reused
        List<Exercise> exercises = new List<Exercise>();

        public Form1()
        {
            InitializeComponent();

            repWeightGrid.ReadOnly = false;
            // add method to list of methods that occur during CellValidating event
            repWeightGrid.CellValidating += repWeightGrid_CellValidating;
        }

        /*private void avgWeightLabel_Click(object sender, EventArgs e)
        {

        }
        */

        private void repWeightGrid_CellContentClick(object sender, EventArgs e)
        {

        }
        

        private void addSetButton_Click(object sender, EventArgs e)
        {
            repWeightGrid.Rows.Add();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            //Store Exercise Object with all data in winform to file of data
            DateTime selectedDate = monthCalendar1.SelectionStart;
            Workout w = new Workout(exercises, selectedDate);
            // MessageBox.Show(w.toString()); // Debug : display toString of the workout
            
            // Serialize Workout object and append to a text file
            string JsonString = JsonConvert.SerializeObject(w);
            String fileName = "workoutData.json";
            string fullPath = Path.GetFullPath(fileName);

            File.AppendAllText(fullPath, JsonString);

            MessageBox.Show("Workout Submitted");

            // reset the list of exercises to be reused for the next workout
            exercises.Clear();

            
            //MessageBox.Show(JsonString);
            Process.Start(fileName);
        }

        // When clicked, add all field values into a new exercise object
        private void addExerciseButton_Click(object sender, EventArgs e)
        {
            //creating values for Exercise 
            String name = exerciseBox.Text;
            int sets = repWeightGrid.RowCount - 1;

            // From the table in the window, generate lists of reps and weights
            int[] repList = new int[sets];
            int[] weightList = new int[sets];
            for (int i = 0; i < sets; i++)
            {
                repList[i] = int.Parse(repWeightGrid.Rows[i].Cells[0].Value.ToString());
                weightList[i] = int.Parse(repWeightGrid.Rows[i].Cells[1].Value.ToString());
            }

            Exercise ex = new Exercise(name, sets, repList, weightList);
            // Add to global var 
            exercises.Add(ex);

            // display to user that it worked
            MessageBox.Show("Exercise added");

        }

        // ensure only numbers are entered into the data grid
        private void repWeightGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            string inputValue = e.FormattedValue.ToString();

            if (String.IsNullOrEmpty(inputValue))
                return;

            // Use a suitable validation method, such as TryParse, to check if the input is a number
            if (!int.TryParse(inputValue, out _))
            {
                // Display an error message
                MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Cancel the editing process to keep the focus on the current cell
                e.Cancel = true;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Person person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };

            // Serialize the object to a JSON string
            string jsonString = JsonConvert.SerializeObject(person);
        }

        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }
    }
    
}
