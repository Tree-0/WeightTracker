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
            //Store the created list of exercises, along with the date, in a new workout object
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            Workout w = new Workout(exercises, selectedDate);

            //get path of file 
            string fileName = "workoutData.json";
            string fullPath = Path.GetFullPath(fileName);

            //de-serialize contents of file into a WorkoutContainer object
            string fileText = File.ReadAllText(fullPath);

            // check if there is anything in file - if there is, put it in WorkoutContainer object 
            // if nothing in file, create new WC object
            WorkoutContainer WC = new WorkoutContainer();
            try
            {
                //Try to create a container from data in file
                if (!string.IsNullOrEmpty(fileText))
                {
                    WC = JsonConvert.DeserializeObject<WorkoutContainer>(fileText);
                }
            } 
            catch (JsonReaderException err) 
            {
                //show user error message and create an empty WorkoutContainer
                MessageBox.Show($"error reading data from file: {err.Message}");
                return;
            }

            WC.Add(w); //Add the newest workout to the container
            
            // Re-Serialize WorkoutContainer object and put back into file
            string JsonString = JsonConvert.SerializeObject(WC, Formatting.Indented);           
            File.WriteAllText(fullPath, JsonString);

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
            int sets = repWeightGrid.RowCount - 1; // -1 because there is always an empty last row

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

        private void graphButton_Click(object sender, EventArgs e)
        {
            // Load a chart of one or more selected exercises plotted x: date / y: weights
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
    }
    
}
