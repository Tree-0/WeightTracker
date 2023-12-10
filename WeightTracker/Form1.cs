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

namespace WeightTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            repWeightGrid.ReadOnly = false;
       
            repWeightGrid.CellValidating += repWeightGrid_CellValidating;
        }

        private void avgWeightLabel_Click(object sender, EventArgs e)
        {

        }

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
            Workout w = new Workout();
            

            string content = JsonConvert.SerializeObject(w);
            String filePath = "workoutData.json";
            File.AppendAllText(filePath, content);
        }

        // When clicked, add all field values into a new exercise object
        private void addExerciseButton_Click(object sender, EventArgs e)
        {
            //creating values for Exercise 
            String name = exerciseBox.Text;
            int sets = repWeightGrid.RowCount;

            // From the table in the window, generate lists of reps and weights
            int[] repList = new int[sets];
            int[] weightList = new int[sets];
            for (int i = 0; i < sets; i++)
            {
                repList[i] = (int)repWeightGrid.Rows[i].Cells[0].Value;
                weightList[i] = (int)repWeightGrid.Rows[i].Cells[1].Value;
            }

            Exercise ex = new Exercise(name, sets, repList, weightList);
            // add to some workout object 

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
    }

    // Workout Class
    public class Workout
    {
        
        private List<Exercise> exercises;   // The list of exercises from the workout
        private DateTime dateOfWorkout;     // the date the workout was completed

        // return list of exercises
        public List<Exercise> getExercises()
        {
            return this.exercises;
        }

        // set the entire list of exercises in Workout 
        public void setExercises(List<Exercise> ex)
        {
            this.exercises = ex;
        }

        // add an exercise to the end of the list
        // in the future, add a way to sort lists by date
        public void addExercise(Exercise e)
        {
            exercises.Add(e);
        }

        // removes an exercise from the workout list if it matches the input argument
        // ideally never removes more than one at a time because of dateOfWorkout
        public void removeExercise(Exercise e)
        {
            foreach (Exercise ex in exercises)
            {
                if(ex.Equals(e))
                {
                    exercises.Remove(ex);
                }
            }
        }

        // get/set date
        public DateTime getDateOfWorkout()
        {
            return this.dateOfWorkout;
        }

        public void setDateOfWorkout(DateTime d)
        {
            this.dateOfWorkout = d;
        }
    }

    // Exercise Class 
    public class Exercise
    {
        private String name;   // name of exercise
        private int sets;      // number of sets done
        private int[] reps;    // a list of numbers, one entry per set
        private int[] weights; // a list of numbers, one entry per set

        public Exercise(string n, int s, int[] r, int[] w)
        {
            name = n;
            sets = s;
            reps = r;
            weights = w;

        }
        // getters and setters
        public String getName()
        {
            return this.name;
        }

        public void setName(String n)
        {
            this.name = n;
        }

        public int getSets()
        {
            return this.sets;
        }

        public void setSets(int s)
        {
            this.sets = s;
        }

        public int[] getReps()
        {
            return this.reps;
        }

        public void setReps(int[] r)
        {
            this.reps = r;
        }

        public int[] getWeights()
        {
            return this.weights;
        }

        public void setWeights(int[] w)
        {
            this.weights = w;
        }
    }
}
