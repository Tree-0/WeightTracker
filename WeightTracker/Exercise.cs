using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightTracker
{
    public class Exercise
    {
        public String Name { get; set; }   // name of exercise
        public int sets { get; set; }     // number of sets done
        public int[] reps { get; set; }   // a list of numbers, one entry per set
        public int[] weights { get; set; } // a list of numbers, one entry per set

        // Constructor
        public Exercise(string n, int s, int[] r, int[] w)
        {
            Name = n;
            sets = s;
            reps = r;
            weights = w;
        }

        public Boolean Equals(Exercise e)
        {
            // bool for list of reps and weights
            Boolean eq = e.reps.SequenceEqual(this.reps) & e.weights.SequenceEqual(this.weights);
            if (e.Name.Equals(this.Name) & e.sets == this.sets & eq)
            {
                return true; // Equal values in each object
            }
            // Exercise objects are not equal
            return false;
        }
    }
}
