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
        [Test, Order(1)]
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

        // Test relies on data being written in Test WriteToDB
        [Test, Order(2)]
        public void ReadObjectsFromDB()
        {
            WorkoutDAL workoutDAL = new WorkoutDAL();

            string dbFilePath = "C:\\Users\\Natha\\OneDrive\\Desktop\\SQLite\\Workouts.db";
            SQLiteConnection connection = WorkoutDAL.ConnectToDatabase(dbFilePath);

            // Read workout from DB
            Workout readWorkout = workoutDAL.GetWorkoutByDate(connection, DateTime.Now.Date);
            Console.WriteLine($"DateTime.Now.Date: {DateTime.Now.Date}");

            // Create workout manually
            #region Declaring Workout
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
            #endregion

            // manual data
            Workout workout = new Workout(exercises, date);
            Tuple<int[], int[]> benchRepsWeights = Tuple.Create(workout.exercises[0].reps, workout.exercises[0].weights);
            Tuple<int[], int[]> shoulderRepsWeights = Tuple.Create(workout.exercises[1].reps, workout.exercises[1].weights);
            Tuple<int, int> benchSetOne = Tuple.Create(workout.exercises[0].reps[0], workout.exercises[0].weights[0]);
            Tuple<int, int> benchSetFour = Tuple.Create(workout.exercises[0].reps[3], workout.exercises[0].weights[3]);

            // DB data - should match manual data
            int benchId = workoutDAL.GetExerciseId(connection, workoutDAL.GetWorkoutIdByDate(connection, date), "bench press");
            int shoulderId = workoutDAL.GetExerciseId(connection, workoutDAL.GetWorkoutIdByDate(connection, date), "shoulder press");
            Tuple<int[], int[]> readBenchRepsWeights = workoutDAL.GetRepsWeights(connection, benchId, 4);
            Tuple<int[], int[]> readShoulderRepsWeights = workoutDAL.GetRepsWeights(connection, shoulderId, 4);

            #region debug prints
            string strreps = String.Join(", ", benchRepsWeights.Item1);
            string strweights = String.Join(", ", benchRepsWeights.Item2);
            string readStringReps = String.Join(", ", readBenchRepsWeights.Item1);
            string readStringWeights = String.Join(", ", readBenchRepsWeights.Item2);

            Console.WriteLine($"bench reps: {strreps}, weights: {strweights}");
            Console.WriteLine($"read bench reps: {readStringReps}, weights: {readStringWeights}");
            #endregion

            // DB workout and manual workout should be the same
            Assert.That(readWorkout.Equals(workout));

            // DB Exercise and manual Exercise should be the same
            Assert.That(e1.Equals(workoutDAL.GetExerciseByName(connection, workoutDAL.GetWorkoutIdByDate(connection, DateTime.Now.Date), e1.Name)));
            Assert.That(e2.Equals(workoutDAL.GetExerciseByName(connection, workoutDAL.GetWorkoutIdByDate(connection, DateTime.Now.Date), e2.Name)));

            // reps and weights from DB and manual workout should match 
            bool arraysEqual = benchRepsWeights.Item1.SequenceEqual(readBenchRepsWeights.Item1) &&
                               benchRepsWeights.Item2.SequenceEqual(readBenchRepsWeights.Item2);
            Assert.That(arraysEqual);

            arraysEqual = shoulderRepsWeights.Item1.SequenceEqual(readShoulderRepsWeights.Item1) &&
                          shoulderRepsWeights.Item2.SequenceEqual(readShoulderRepsWeights.Item2);
            Assert.That(arraysEqual);

            bool intsEqual = benchSetOne.Item1 == workoutDAL.GetRepsWeightsOneSet(connection, benchId, 1).Item1;
            Assert.That(intsEqual);
            intsEqual = benchSetOne.Item2 == workoutDAL.GetRepsWeightsOneSet(connection, benchId, 1).Item2;
            Assert.That(intsEqual);
            intsEqual = benchSetFour.Item1 == workoutDAL.GetRepsWeightsOneSet(connection, benchId, 4).Item1;
            Assert.That(intsEqual);
            intsEqual = benchSetFour.Item2 == workoutDAL.GetRepsWeightsOneSet(connection, benchId, 4).Item2;
        }

        [Test, Order(3)]
        public void DeleteFromDB()
        {
            WorkoutDAL workoutDAL = new WorkoutDAL();
            string dbFilePath = "C:\\Users\\Natha\\OneDrive\\Desktop\\SQLite\\Workouts.db";
            SQLiteConnection connection = WorkoutDAL.ConnectToDatabase(dbFilePath);

            DateTime date = DateTime.Now.Date;
            workoutDAL.DeleteWorkoutByDate(connection, date);
        }
    }
}
