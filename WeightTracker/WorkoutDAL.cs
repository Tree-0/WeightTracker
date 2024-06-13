using System;
using System.Data.SQLite;

namespace WeightTracker
{
    public class WorkoutDAL
    {
        // Constructor
        public WorkoutDAL() { }

        // Open and return a connection the database
        public static SQLiteConnection ConnectToDatabase(string dbFilePath)
        {
            var connectionString = new SQLiteConnectionStringBuilder { DataSource = dbFilePath }.ToString();
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }


        //
        // CRUD functionality
        //


        // Add the date and notes for a workout to the Workout table
        public void AddWorkout(SQLiteConnection connection, Workout workout)
        {   
            using (var cmd = new SQLiteCommand("INSERT INTO Workouts (Date, Notes) VALUES (@date, @notes)", connection))
            {
                cmd.Parameters.AddWithValue("@date", workout.dateOfWorkout);
                cmd.Parameters.AddWithValue("@notes", workout.notes);
                cmd.ExecuteNonQuery();
            }

            foreach (Exercise e in workout.exercises)
            {
                AddExercise(connection, e, workout.dateOfWorkout);
            }
        }

        // Add the name and number of sets of an exercise into the Exercise table
        public void AddExercise(SQLiteConnection connection, Exercise exercise, DateTime workoutDate)
        {
            int workoutId = GetWorkoutIdByDate(connection, workoutDate);
            if (workoutId == -1)
            {
                throw new InvalidOperationException("No workout found on the specified date.");
            }

            using (var cmd = new SQLiteCommand("INSERT INTO Exercises (WorkoutId, Name, Sets) VALUES (@workoutId, @exerciseName, @sets)", connection))
            {
                cmd.Parameters.AddWithValue("@workoutId", workoutId);
                cmd.Parameters.AddWithValue("@exerciseName", exercise.Name);
                cmd.Parameters.AddWithValue("@sets", exercise.sets);
                cmd.ExecuteNonQuery();
            }

            AddRepsWeights(connection, exercise, workoutId);
        }

        // Add the reps and weights of an exercise into the RepsWeights table
        public void AddRepsWeights(SQLiteConnection connection, Exercise exercise, int workoutId)
        {
            int exerciseId = GetExerciseId(connection, workoutId, exercise.Name);
            using (var cmd = new SQLiteCommand("INSERT INTO RepsWeights (ExerciseId, SetNumber, Reps, Weight) VALUES (@exerciseId, @setNumber, @reps, @weight)", connection))
            {
                cmd.Parameters.AddWithValue("@exerciseId", exerciseId);
                cmd.Parameters.AddWithValue("@setNumber", 0);
                cmd.Parameters.AddWithValue("@reps", 0);
                cmd.Parameters.AddWithValue("@weight", 0);

                using (var transaction = connection.BeginTransaction())
                {
                    for (int i = 0; i < exercise.sets; i++)
                    {
                        cmd.Parameters["@setNumber"].Value = i + 1;
                        cmd.Parameters["@reps"].Value = exercise.reps[i];
                        cmd.Parameters["@weight"].Value = exercise.weights[i];

                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        // Delete the selected workout - also deletes all associated exercises and exercise information 
        public void DeleteWorkoutByDate(SQLiteConnection connection, DateTime workoutDate)
        {   
            int workoutId = GetWorkoutIdByDate(connection, workoutDate);

            // delete each exercise with a key that points to workoutId
            string sql = "Select ExerciseId FROM Exercises WHERE WorkoutId = @WorkoutId";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@WorkoutId", workoutId);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    // iterate through results of selection and manipulate data
                    while (reader.Read())
                    {
                        int ExerciseId = reader.GetInt32(0);
                        Console.WriteLine($"Exercise ID: {ExerciseId}, Workout ID: {workoutId}");
                        DeleteExercises(connection, workoutId); // Delete the exercise
                    }
                }
            }

            // delete workouts
            sql = "DELETE from Workouts WHERE Date = @Date";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@Date", workoutDate);

                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} rows deleted in Workouts"); //debug
            }
        }

        // Delete the exercises associated with the workout - also deletes all associated exercise information (reps, weights)
        public void DeleteExercises(SQLiteConnection connection, int workoutId)
        {
            // delete reps and weights for exercise
            string sql = "SELECT ExerciseId from Exercises WHERE WorkoutId = @WorkoutId";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@WorkoutId", workoutId);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    // iterate through results of selection and manipulate data
                    while (reader.Read())
                    {
                        int ExerciseId = reader.GetInt32(0);
                        
                        DeleteRepsWeights(connection, ExerciseId); // Delete all set information for an exercise
                    }
                }
            }

            // delete exercise
            sql = "DELETE from Exercises WHERE WorkoutId = @WorkoutId";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@WorkoutId", workoutId);

                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} rows deleted in Exercises"); //debug
            }
        }

        // Delete the reps and weights for an exercise
        public void DeleteRepsWeights(SQLiteConnection connection, int exerciseId)
        {
            string sql = "DELETE from RepsWeights WHERE ExerciseId = @ExerciseId";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@ExerciseId", exerciseId);

                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} rows deleted in RepsWeights"); //debug
            }
        }

        // Delete a specific set of an exercise
        public void DeleteRepsWeights(SQLiteConnection connection, int exerciseId, int setNumber)
        {
            string sql = "DELETE from RepsWeights WHERE ExerciseId = @ExerciseId AND SetNumber = @SetNumber";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@ExerciseId", exerciseId);
                cmd.Parameters.AddWithValue("@SetNumber", setNumber);

                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"{affectedRows} rows deleted in RepsWeights"); //debug
            }
        }


        //
        // Getters
        //


        // Find a unique workout in the db, return the id number
        public int GetWorkoutIdByDate(SQLiteConnection connection, DateTime workoutDate)
        {
            using (var cmd = new SQLiteCommand("SELECT WorkoutId FROM Workouts WHERE Date = @date", connection))
            {
                cmd.Parameters.AddWithValue("@date", workoutDate);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0); // get workoutId
                    }
                    else
                    {
                        return -1; // throw an exception if the workout does not exist
                    }
                }
            }
        }

        // Find a unique exercise in the db, return the id number
        public int GetExerciseId(SQLiteConnection connection, int workoutId, string exerciseName)
        {
            using (var cmd = new SQLiteCommand("SELECT ExerciseID FROM Exercises WHERE WorkoutId = @workoutId AND Name = @name", connection))
            {
                cmd.Parameters.AddWithValue("@workoutId", workoutId);
                cmd.Parameters.AddWithValue("@name", exerciseName);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0); // get ExerciseId
                    }
                    else
                    {
                        return -1; // throw an exception if the exercise does not exist
                    }
                }
            }
        }

        // Find a particular set of an exercise in the db, return the id number
        public int GetRepWeightId(SQLiteConnection connection, int exerciseId, int setNumber)
        {
            string sql = "SELECT RepsWeightsId FROM RepsWeights WHERE ExerciseId = @ExerciseId AND SetNumber = @SetNumber";
            using (var cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("ExerciseId", exerciseId);
                cmd.Parameters.AddWithValue("SetNumber", setNumber);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        return -1; // throw an exception if the exercise information does not exist
                    }
                }
            }
        }
    }
}
