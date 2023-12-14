using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightTracker
{
    public class WorkoutContainer
    {
        public List<Workout> Workouts;

        //constructors
        public WorkoutContainer()
        {
            Workouts = new List<Workout>();
        }
        public WorkoutContainer(List<Workout> workouts)
        {
            Workouts = workouts;
        }

        // Methods
        public void Add(Workout w)
        {
            Workouts.Add(w);
        }

        // Remove any workout with all the same values 
        public void Remove(Workout w) 
        {
            foreach(Workout wkt in Workouts)
            {
                if (wkt.Equals(w))
                {
                    Workouts.Remove(wkt);
                }
            }
        }

        // Remove workouts on a particular date
        public void RemoveOnDate(DateTime date)
        {
            for (int i = Workouts.Count - 1; i >= 0; i--)
            {
                if (Workouts[i].dateOfWorkout.Equals(date))
                {
                    // Remove the item at index i
                    Workouts.RemoveAt(i);
                }
            }
        }

        //Overload Remove with methods that remove workouts with specific properties
        // CHECK DOCUMENTATION TO SEE IF THERE ARE ALREADY BUILT IN METHODS
    }
}
