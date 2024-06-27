using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WorkoutTracker
{
    public class Workout
    {

        public List<Exercise> exercises { get; set; }   // The list of exercises from the workout
        public DateTime dateOfWorkout { get; set; } // the date the workout was completed
        public String notes { get; set; }           // any additional information about the workout

        //Constructors
        public Workout() { }

        public Workout(List<Exercise> loe, DateTime d)
        {
            exercises = loe;
            dateOfWorkout = d.Date;
            notes = string.Empty;
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
        override public String ToString()
        {

            StringBuilder sb = new StringBuilder();
            foreach (Exercise ex in this.exercises)
            {
                sb.Append(ex.ToString());
            }
            
            // Don't really need this because of how I structured the Form
            //sb.Append(this.dateOfWorkout.ToString());

            return sb.ToString();
        }

        // Check if a workout contains both of the input exercises
        // TODO: Need to add buttons to the winform that support checking for this
        //       Check if passes edge cases - more than one exercise in workout with same name, strange input arguments
        public Boolean containsBoth(string n1, string n2) 
        {
            if (n1.Equals(n2)) throw new Exception("workouts must be different");

            int count = 0;
            foreach (Exercise ex in this.exercises)
            {
                if (ex.Name.Equals(n1) || ex.Name.Equals(n2))
                    count++;
            }

            if (count == 2) return true;

            return false;
        
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
        public void addRepsToSeries(Series s, string n)
        {
            foreach (Exercise e in this.exercises)
            {
                if (e.Name.Equals(n))
                {
                    // Add a point to the series with the date and total reps of exercise
                    s.Points.AddXY(this.dateOfWorkout, e.TotalReps());
                }
            }
        }

        public void addTotalWeightToSeries(Series s, string n)
        {
            foreach (Exercise e in this.exercises)
            {
                if (e.Name.Equals(n))
                {
                    // Add a point to the series with the date and total weight lifted for that exercise
                    s.Points.AddXY(this.dateOfWorkout, e.TotalWeightLifted());
                }
            }
        }

        // TODO: Add some sort of indicator for a given exercise if another exercise that uses similar muscles was done during the same workout
        // idea is to be able to determine if exercise weight/rep/sets were lower because other similar exercises were also performed that day
        public void addJointExerciseIndicatorToSeries(Series s, string n)
        {

        }
    }
}
