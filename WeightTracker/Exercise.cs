﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutTracker
{
    public class Exercise
    {
        public String Name { get; set; }   // name of exercise
        public int sets { get; set; }     // number of sets done
        public int[] reps { get; set; }   // number of repetitions of a movement done, one entry per set
        public int[] weights { get; set; } // weight used for movement, one entry per set

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

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + ": " + Environment.NewLine);

            for (int i = 0; i < sets; i++)
            {
                sb.Append("(" + this.weights[i].ToString() + " * " + this.reps[i].ToString() + ") " + Environment.NewLine);
            }

            return sb.ToString();
        }

        // gets the total number of reps across all sets of the exercise
        public int TotalReps()
        {
            return this.reps.Sum();
        }

        // gets the total weight lifted across all reps of the exercise
        public int TotalWeightLifted()
        {
            int sumWeights = 0;
            for (int i = 0; i < this.reps.Length; i++)
            {
                // for each set, add the number of reps times the weight lifted for that set to the sum
                sumWeights += this.reps[i] * this.weights[i];
            }
            return sumWeights;
        }

        // gets the average weight lifted for each rep
        public float AverageWeightLifted()
        {
            if (this.TotalReps() == 0) 
            {
                Console.WriteLine("No Reps -- Attempt to divide by zero");
                return 0.0f; 
            }
            return this.TotalWeightLifted() / this.TotalReps();
        }
    }
}
