using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Security;
using System.Data.Entity.Core.Common.CommandTrees;

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

        // // CREATE

        // Add the date and notes for a workout to the Workout table
        public void AddWorkout(SQLiteConnection connection, Workout workout)
        {   
            using (var cmd = new SQLiteCommand("INSERT INTO Workouts (Date, Notes) VALUES (@date, @notes)", connection))
            {
                cmd.Parameters.AddWithValue("@date", workout.dateOfWorkout);
                cmd.Parameters.AddWithValue("@notes", workout.notes);
                cmd.ExecuteNonQuery();
            }

            // Add workout's exercises to the Exercises table
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


        // // DELETE 

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

        // Delete all sets (all reps and weights) of an exercise
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


        // // READ 

        // Find a unique workout in the db, return a Workout OBJECT
        public Workout GetWorkoutByDate(SQLiteConnection connection, DateTime workoutDate)
        {
            Workout workout = new Workout();
            List<Exercise> exercises;

            // retrieve the notes from the workout table
            using (var cmd = new SQLiteCommand("SELECT WorkoutId, Notes FROM Workouts WHERE Date = @date", connection))
            {
                cmd.Parameters.AddWithValue("@date", workoutDate);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new InvalidOperationException("No workouts found for the specified date.");
                    }

                    if (reader.Read())
                    {
                        // read relevant data
                        int workoutId = reader.GetInt32(reader.GetOrdinal("WorkoutId"));
                        string notes = reader.GetString(reader.GetOrdinal("Notes"));

                        // retrieve the exercises from the exercise table
                        exercises = GetExercises(connection, workoutId);

                        workout.dateOfWorkout = workoutDate;
                        workout.notes = notes;
                        workout.exercises = exercises;
                    }
                }
            }

            return workout;

        }

        // Find exercises from a given workout, return a list of Exercise OBJECTS
        public List<Exercise> GetExercises(SQLiteConnection connection, int workoutId)
        {
            List<Exercise> exercises = new List<Exercise>();
            Tuple<int[], int[]> repsWeights;

            using (var cmd = new SQLiteCommand("SELECT ExerciseID, Name, Sets FROM Exercises WHERE WorkoutId = @workoutId", connection))
            {
                cmd.Parameters.AddWithValue("@workoutId", workoutId);
                using (var reader = cmd.ExecuteReader())
                {
                    // for every selected exercise, format data into objects and add to list
                    while (reader.Read())
                    {
                        int exerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseID"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));
                        int sets = reader.GetInt32(reader.GetOrdinal("Sets"));

                        // need to get rep and weight information from respective table
                        repsWeights = GetRepsWeights(connection, exerciseId, sets);

                        exercises.Add(new Exercise(name, sets, repsWeights.Item1, repsWeights.Item2));
                    }
                }
            }

            return exercises;
        }

        // Find exercise from a given workout with the given name, return an Exercise OBJECT
        public Exercise GetExerciseByName(SQLiteConnection connection, int workoutId, string name)
        {

            string query = "SELECT ExerciseID, Name, Sets FROM Exercises WHERE WorkoutId = @workoutId And Name = @name";
            using (var cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@workoutId", workoutId);
                cmd.Parameters.AddWithValue("@name", name);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int exerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"));
                        int sets = reader.GetInt32(reader.GetOrdinal("Sets"));
                        Tuple<int[], int[]> repsWeights = GetRepsWeights(connection, exerciseId, sets);

                        return new Exercise(name, sets, repsWeights.Item1, repsWeights.Item2);
                    }
                    else
                    {
                        throw new InvalidOperationException($"No Exercise with name {name} in this workout");
                    }
                }


            }
        }

        // Find all reps and weights for an exercise (all sets), return arrays of reps and weights as a Tuple
        public Tuple<int[], int[]> GetRepsWeights(SQLiteConnection connection, int exerciseId, int sets)
        {
            int[] reps = new int[sets];
            int[] weights = new int[sets];

            using (var cmd = new SQLiteCommand("SELECT Reps, Weight FROM RepsWeights WHERE ExerciseId = @exerciseId", connection))
            {
                cmd.Parameters.AddWithValue("@exerciseId", exerciseId);
                using (var reader = cmd.ExecuteReader())
                {
                    // for every selected row, add reps and weights into arrays
                    int i = 0;
                    while (reader.Read())
                    {
                        reps[i] = reader.GetInt32(reader.GetOrdinal("Reps"));
                        weights[i] = reader.GetInt32(reader.GetOrdinal("Weight"));
                        i++;
                    }
                }
            }

            return Tuple.Create(reps, weights);
        }

        // Find reps and weight for a particular set of an exercise, return reps and weight as a Tuple
        public Tuple<int, int> GetRepsWeightsOneSet(SQLiteConnection connection, int exerciseId, int setNum)
        {
            int reps;
            int weight;

            string query = "SELECT Reps, Weight FROM RepsWeights WHERE ExerciseId = @exerciseId AND SetNumber = @SetNumber";
            using (var cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@exerciseId", exerciseId);
                cmd.Parameters.AddWithValue("@SetNumber", setNum);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reps = reader.GetInt32(reader.GetOrdinal("Reps"));
                        weight = reader.GetInt32(reader.GetOrdinal("Weight"));
                    }
                    else
                    {
                        throw new InvalidOperationException($"set number {setNum} not found for this exercise");
                    }
                }
            }

            return Tuple.Create(reps, weight);
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
        public int GetRepsWeightsId(SQLiteConnection connection, int exerciseId, int setNumber)
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
