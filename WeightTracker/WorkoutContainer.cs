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

        //Methods
        public void Add(Workout w)
        {
            Workouts.Add(w);
        }

        public void Remove(Workout w) //Remove any workout with all the same values 
        {
            foreach(Workout wkt in Workouts)
            {
                if (wkt.Equals(w))
                {
                    Workouts.Remove(wkt);
                }
            }
        }

        //Overload Remove with methods that remove workouts with specific properties?
    }
}
