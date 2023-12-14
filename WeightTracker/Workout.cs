using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace WeightTracker
{
    public class Workout
    {

        public List<Exercise> exercises { get; set; }   // The list of exercises from the workout
        public DateTime dateOfWorkout;     // the date the workout was completed

        //Constructors
        public Workout() { }

        public Workout(List<Exercise> loe, DateTime d)
        {
            exercises = loe;
            dateOfWorkout = d;
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
                if (ex.Equals(e))
                {
                    exercises.Remove(ex);
                }
            }
        }

        // return a string of all Exercise names, and the date
        public String toString()
        {
            
            StringBuilder sb = new StringBuilder();
            foreach(Exercise ex in this.exercises)
            {
                sb.Append(ex.Name + " reps: " + ex.reps + " weights: " + ex.weights + "\n");

            }
            sb.Append(this.dateOfWorkout.ToString());

            return sb.ToString();
        }

        public Boolean Equals(Workout w)
        {
            // if different amounts of exercises they are not the same workout
            if (this.exercises.Count != w.exercises.Count) 
                return false;

            // Check if every workout in each exercise list is identical using the Exercise.equals method
            for (int i = 0; i < exercises.Count; i++)
            {
                if (!this.exercises[i].Equals(w.exercises[i]))
                {
                    return false;
                }
            }

            // Check if the DateTime of each workout is the same
            if (this.dateOfWorkout != w.dateOfWorkout)
            {
                return false;
            }

            return true;
        }

        // checks all the exercises in a workout and adds the average rep weight 
        // of any exercises with matching name to a series
        public void addExercisesToSeries(Series s, string n)
        {
            foreach (Exercise e in this.exercises)
            {
                if (e.Name.Equals(n))
                {
                    // Add a point to the series with the date and avg rep weight of exercise
                    s.Points.AddXY(this.dateOfWorkout, e.AverageWeightLifted());
                }
            }
        }
    }
}
