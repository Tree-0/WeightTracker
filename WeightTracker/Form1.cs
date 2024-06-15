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
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SQLite;

//
// Nathaniel Stall
//
// Personal project - Track workouts, visualize them to see progress.
// Eventually add more features... calculations, making workout plans...
//
// Features Checklist: 
// https://docs.google.com/document/d/1ylS3EJpUSgirsgJCRAvSfPP_sIsRLDkDQlpYvgdLyCU/edit
//

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

        private void addSetButton_Click(object sender, EventArgs e)
        {
            repWeightGrid.Rows.Add();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            //Store the created list of exercises, along with the date, in a new workout object
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            Workout w = new Workout(exercises, selectedDate);

            WorkoutDAL DAL = new WorkoutDAL();
            DAL.AddWorkout(w);

            MessageBox.Show("Workout Submitted");

            // reset the list of exercises to be reused for the next workout
            exercises.Clear();
            exerciseInfoBox.Clear();
        }

        // When clicked, add all field values into a new exercise object
        private void addExerciseButton_Click(object sender, EventArgs e)
        {
            //creating values for Exercise 
            String name = exerciseBox.Text;

            if (String.IsNullOrEmpty(name)) {
                MessageBox.Show("Please select an exercise.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            // Add name of exercise into textbox displaying all exercises of current workout
            exerciseInfoBox.AppendText(ex.Name + Environment.NewLine);

            // display to user that it worked
            //MessageBox.Show("Exercise added");

        }

        // When clicked, graph the data from the file on a chart
        private void graphButton_Click(object sender, EventArgs e)
        {
            // create new window to open chart in
            Form chartForm = new Form();
            chartForm.Size = new Size(400, 300);

            // create Chart and add to form
            Chart weightGraph = new Chart();
            chartForm.Controls.Add(weightGraph);

            // create chartArea
            ChartArea chartArea = new ChartArea("WeightChartArea");
            weightGraph.ChartAreas.Add(chartArea);
            weightGraph.Dock = DockStyle.Fill;

            // create series of data
            Series avgRepWeightSeries = new Series("Avg. rep weight");
            avgRepWeightSeries.ChartType = SeriesChartType.Point;

            Series repSeries = new Series("Total reps");
            repSeries.ChartType = SeriesChartType.Point;

            Series weightSeries = new Series("Total weight lifted");
            weightSeries.ChartType = SeriesChartType.Point;

            // 

            WorkoutDAL DAL = new WorkoutDAL();

            List<Workout> workouts = DAL.GetAllWorkouts();

            // which exercise to graph is selected in a listBox
            string exerciseName = graphListBox.Text;

            foreach (Workout w in workouts)
            {
                w.addExercisesToSeries(avgRepWeightSeries, exerciseName);
                w.addRepsToSeries(repSeries, exerciseName);
                w.addTotalWeightToSeries(weightSeries, exerciseName);
            }

            // Add series to chart
            weightGraph.Series.Add(avgRepWeightSeries);
            weightGraph.Series.Add(repSeries);
            //weightGraph.Series.Add(weightSeries);

            // Make Datapoints more visible
            avgRepWeightSeries.MarkerSize = 10;
            repSeries.MarkerSize = 10;
            //weightSeries.MarkerSize = 10;


            // Axis labels
            chartArea.AxisX.Title = "Date of Workout";
            
            //  Legend for Series
            Legend key = new Legend();
            weightGraph.Legends.Add(key);

            // Add labels for each series to legend
            weightGraph.Series["Avg. rep weight"].LegendText = "Average rep weight";
            weightGraph.Series["Total reps"].LegendText = "Total reps";
            //weightGraph.Series["Total weight lifted"].LegendText = "Total weight lifted";

            // display graph upon button press
            chartForm.Show();

        }

        // Get date from monthCalendar, check for workouts in DB, and display info to the WorkoutDisplayBox
        private void UpdateWorkoutDisplayBox()
        {
            WorkoutDAL DAL = new WorkoutDAL();
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;

            // Display date of workout selected
            workoutDisplayBoxLabel.Text = "Workout Date: " + selectedDate.ToString();

            // Display workout information, if there is any
            Workout W;
            try
            {
                W = DAL.GetWorkoutByDate(selectedDate);
                workoutDisplayBox.Text = W.ToString();
            }
            catch
            {
                workoutDisplayBox.Text = "No Workout On Selected Date";
            }
        }

        // Whenever a date on the calendar is selected, check if any workout occurred on that date and display it
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            UpdateWorkoutDisplayBox();
        }

        // When clicked, delete the selected workout from the DB and update displays accordingly
        private void deleteWorkoutButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Delete this workout?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            // Handle the result
            switch (result)
            {
                case DialogResult.OK: // deletion confirmed
                    WorkoutDAL DAL = new WorkoutDAL();
                    DAL.DeleteWorkoutByDate(monthCalendar1.SelectionStart.Date);
                    UpdateWorkoutDisplayBox(); // show that workout was deleted
                    break;
                case DialogResult.Cancel: // cancel action
                    break;
            }
        }

        // Delete an exercise from the current workout, based on the name selected in the drop down menu on the left hand side
        private void deleteExerciseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Delete {graphListBox.Text} from workout?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            
            // Handle the result
            switch (result)
            {
                case DialogResult.OK: // deletion confirmed
                    WorkoutDAL DAL = new WorkoutDAL();
                    DAL.DeleteExerciseByName(DAL.GetWorkoutIdByDate(monthCalendar1.SelectionStart.Date), graphListBox.Text);
                    UpdateWorkoutDisplayBox(); // show that workout was deleted
                    break;
                case DialogResult.Cancel: // cancel action
                    break;
            }
        }

        // Delete a set from a selected exercise of the current workout, based on number input to a text box
        private void deleteSetButton_Click(object sender, EventArgs e)
        {
            WorkoutDAL DAL = new WorkoutDAL();
            int workoutId = DAL.GetWorkoutIdByDate(monthCalendar1.SelectionStart.Date);

            // Validate the set number 
            if (!setToDeleteBox_Validating(workoutId)) { return; }
            int set = int.Parse(setToDeleteBox.Text);

            DialogResult result = MessageBox.Show($"Delete set {setToDeleteBox.Text} from {graphListBox.Text}?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            // Handle the result
            switch (result)
            {
                case DialogResult.OK: // deletion confirmed
                    int exerciseId = DAL.GetExerciseId(workoutId, graphListBox.Text);
                    DAL.DeleteRepsWeights(exerciseId, set);
                    UpdateWorkoutDisplayBox(); // show that workout was updated, set deleted
                    break;
                case DialogResult.Cancel: // cancel action
                    break;
            }
        }

        // ensure an int between 1 and the total sets for that exercise is used
        // I'm not sure if this is the appropriate way to validate. 
        // This method does not subscribe to an event like repWeightGrid_CellValidating...
        private bool setToDeleteBox_Validating(int workoutId)
        {
            if (!int.TryParse(setToDeleteBox.Text, out _))
            {
                // Display an error message
                MessageBox.Show("Please enter a valid number between 1 and your number of sets.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return false;
            }

            WorkoutDAL DAL = new WorkoutDAL();
            int maxSet = DAL.GetExerciseByName(workoutId, graphListBox.Text).sets; // number of sets in exercise
            int set = int.Parse(setToDeleteBox.Text); // user's selected set

            // selected set must be in range
            if (set < 1 || set > maxSet)
            {
                MessageBox.Show("Please enter a number between 1 and your number of sets.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // ensure only numbers are entered into the data grid
        private void repWeightGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            string inputValue = e.FormattedValue.ToString();

            if (String.IsNullOrEmpty(inputValue))
                return;

            // check if input is a valid number
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
