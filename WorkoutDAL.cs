using System;
using System.data.SQLite;

public class WorkoutDAL
{
	public WorkoutDAL()
	{

	}

	public static SQLiteConnection ConnectToDatabase(string dbFilePath)
	{
        var connectionString = new SQLiteConnectionStringBuilder { DataSource = dbFilePath }.ToString();
        var connection = new SQLiteConnection(connectionString);
        connection.Open();
        return connection;
    }

    public void AddWorkout(SQLiteConnection connection, Workout workout)
    {
        using (var cmd = new SQLiteCommand("INSERT INTO Workouts (Date, Notes) VALUES (@date, @notes)", connection))
        {
            cmd.Parameters.AddWithValue("@date", workout.dateOfWorkout);
            cmd.Parameters.AddWithValue("@description", workout.notes);
            cmd.ExecuteNonQuery();
        }
    }

    public void AddExercise(SQLiteConnection connection, Exercise exercise)
    {
        using (var cmd = new SQLiteCommand("INSERT INTO Exercises (WorkoutId, Name, Sets) VALUES (@workoutId, name, sets)", connection))
        {
            cmd.Parameters.AddWithValue("@workoutId");
            cmd.Parameters.AddWithValue("@name", exercise.Name);
            cmd.Parameters.AddWithValue("@sets", exercise.sets);
            cmd.ExecuteNonQuery();
        }
    }

    public void AddRepsWeights(SQLiteConnection connection, Exercise exercise)
    {

    }
}
