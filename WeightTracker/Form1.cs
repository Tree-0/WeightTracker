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
    //
    //
    // CHANGE FROM JSON SERIALIZER/DESERIALIZER TO WORKOUTDAL ADDING TO DATABASE
    //
    //
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
            exerciseInfoBox.Clear();
            //MessageBox.Show(JsonString);
            //Process.Start(fileName);
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

            // Some code below is reused from submitButton eventHandler -> can be abstracted into function?
            // read contents of WorkoutContainer from file, Use to create series for a particular exercise

     //
     //
     // CHANGE FROM JSON TO WORKOUTDAL READING FROM DB 
     //
     //

            //get path of file 
            string fileName = "workoutData.json";
            string fullPath = Path.GetFullPath(fileName);

            //de-serialize contents of file into a WorkoutContainer object
            string fileText = File.ReadAllText(fullPath);

            WorkoutContainer WC = new WorkoutContainer();
            if (!string.IsNullOrEmpty(fileText))
            {
                WC = JsonConvert.DeserializeObject<WorkoutContainer>(fileText);
            }
            // END of reused code, can this be abstracted?s

            // which exercise to graph is selected in a listBox
            string exerciseName = graphListBox.Text;

            foreach (Workout w in WC.Workouts)
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
