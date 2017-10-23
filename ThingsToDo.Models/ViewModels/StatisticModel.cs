using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.Models.ViewModels
{
    public class StatisticModel
    {
        public StatisticModel()
        {
            TimeSeriesChartListFinishedTasks = new List<TimeSeriesChart>();
            TimeSeriesChartListRemovedTasks = new List<TimeSeriesChart>();
            TimeSeriesChartListRemainingTasks = new List<TimeSeriesChart>();
        }

        // weekly
        public int WeeklyFinishedTasks { get; set; }
        public int WeeklyRemovedTasks { get; set; }
        public int WeeklyRemainingTasks { get; set; }

        // monthly
        public int MonthlyFinishedTasks { get; set; }
        public int MonthlyRemovedTasks { get; set; }
        public int MonthlyRemainingTasks { get; set; }

        // quarter
        public int QuarterFinishedTasks { get; set; }
        public int QuarterRemovedTasks { get; set; }
        public int QuarterRemainingTasks { get; set; }

        public List<TimeSeriesChart> TimeSeriesChartListFinishedTasks { get; set; }
        public List<TimeSeriesChart> TimeSeriesChartListRemovedTasks { get; set; }
        public List<TimeSeriesChart> TimeSeriesChartListRemainingTasks { get; set; }
    }
}
