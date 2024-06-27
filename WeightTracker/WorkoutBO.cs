using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkoutTracker
{
    internal class WorkoutBO
    {
        public WorkoutBO() { }

        // Get workout object from UI on submission button click
        // should use UI here
        public Workout getWorkoutFromUI(List<Exercise> exercises)
        {
            DateTime selectedDate = DateTime.Now.Date; //monthCalendar1.SelectionStart.Date;
            Workout w = new Workout(exercises, selectedDate);

            return w;
        }

        // Send Workout object to DB on submission button click
        // should use DAL here

        // 
    }


}
