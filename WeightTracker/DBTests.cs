using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WeightTracker
{
    [TestFixture]
    internal class DBTests
    {
        [Test]
        public void WriteToDB() 
        {   
            List<Exercise> exercises = new List<Exercise>();

            int[] reps = new int[] { 10, 10, 10, 10 };
            int[] weights = new int[] { 135, 155, 175, 185 };
            Exercise e1 = new Exercise("bench press", 4, reps, weights);
            reps = new int[] { 10, 10, 8, 8 };
            weights = new int[] { 50, 50, 55, 55 };
            Exercise e2 = new Exercise("shoulder press", 4, reps, weights);

            exercises.Add(e1);
            exercises.Add(e2);

            DateTime date = DateTime.Now.Date;

            Workout workout = new Workout(exercises, date);

            WorkoutDAL workoutDAL = new WorkoutDAL();

            string dbFilePath = "C:\\Users\\Natha\\OneDrive\\Desktop\\SQLite\\Workouts.db";
            SQLiteConnection connection = WorkoutDAL.ConnectToDatabase(dbFilePath);

            workoutDAL.AddWorkout(connection, workout);
        }
    }
}
