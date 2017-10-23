using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ThingsToDo.BL.Extensions;
using ThingsToDo.BL.Interfaces;
using ThingsToDo.BL.ServiceHelpers;
using ThingsToDo.DAL;
using ThingsToDo.Models.Datatables;
using ThingsToDo.Models.Entity;
using ThingsToDo.Models.ViewModels;

namespace ThingsToDo.BL.Services
{
    public class StatisticsService : IStatisticsService
    {
        public async Task<StatisticModel> GetAllStatistics(int userId)
        {

            var today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            // monthly 
            var firstDayOfMonth = today.AddMonths(-1);
            // weekly 
            var firstDayOfWeek = today.AddDays(-7);
            // quarter
            var firstDayOfQuarter = today.AddMonths(-3);

            var until = today.AddDays(-1);

            using (var db = new DAL.ThingsToDoAppContext())
            {
                var finishedTasks = await db.Tasks.Include(x => x.Category).Where(x => x.Category.UserId == userId && x.FinishedDate.HasValue).ToListAsync();
                var remainingTasks = await db.Tasks.Include(x => x.Category).Where(x => x.Category.UserId == userId && !x.FinishedDate.HasValue).ToListAsync();
                var removedTasks = await db.RemovedTasks.Include(x => x.Category).Where(x => x.Category.UserId == userId).ToListAsync();
                var model = new StatisticModel()
                {
                    // monthly
                    MonthlyFinishedTasks = finishedTasks.Any() ? finishedTasks.Where(x => x.FinishedDate > firstDayOfMonth && x.FinishedDate < until).Count() : 0,
                    MonthlyRemainingTasks = remainingTasks.Any() ? remainingTasks.Where(x => x.ToDate > firstDayOfMonth && x.ToDate < until).Count() : 0,
                    MonthlyRemovedTasks = removedTasks.Any() ? removedTasks.Where(x => x.RemovedDate > firstDayOfMonth && x.RemovedDate < until).Count() : 0,

                    // weekly
                    WeeklyFinishedTasks = finishedTasks.Any() ? finishedTasks.Where(x => x.FinishedDate > firstDayOfWeek && x.FinishedDate < until).Count() : 0,
                    WeeklyRemainingTasks = remainingTasks.Any() ? remainingTasks.Where(x => x.ToDate > firstDayOfWeek && x.ToDate < until).Count() : 0,
                    WeeklyRemovedTasks = removedTasks.Any() ? removedTasks.Where(x => x.RemovedDate > firstDayOfWeek && x.RemovedDate < until).Count() : 0,

                    // quarter
                    QuarterFinishedTasks = finishedTasks.Any() ? finishedTasks.Where(x => x.FinishedDate > firstDayOfQuarter && x.FinishedDate < until).Count() : 0,
                    QuarterRemainingTasks = remainingTasks.Any() ? remainingTasks.Where(x => x.ToDate > firstDayOfQuarter && x.ToDate < until).Count() : 0,
                    QuarterRemovedTasks = removedTasks.Any() ? removedTasks.Where(x => x.RemovedDate > firstDayOfQuarter && x.RemovedDate < until).Count() : 0,


                };
             
                    var eachDayOfWeekBefore = -1;
                    var countTasks = 0;

                    for (var i = 0; i < 7; i++)
                    {
                   
                        countTasks = finishedTasks.Where(x => x.FinishedDate > DateTime.UtcNow.AddDays(eachDayOfWeekBefore) && x.FinishedDate < DateTime.UtcNow.AddDays(eachDayOfWeekBefore + 1)).Count();

                        model.TimeSeriesChartListFinishedTasks.Add(new TimeSeriesChart()
                        {
                            Day = i.ToString(),
                            TaskCount = countTasks
                        });
                   
                        countTasks = removedTasks.Where(x => x.RemovedDate > DateTime.UtcNow.AddDays(eachDayOfWeekBefore) && x.RemovedDate < DateTime.UtcNow.AddDays(eachDayOfWeekBefore + 1)).Count();
                        model.TimeSeriesChartListRemovedTasks.Add(new TimeSeriesChart()
                        {
                            Day = i.ToString(),
                            TaskCount = countTasks
                        });
                   
                        countTasks = remainingTasks.Where(x => x.ToDate > DateTime.UtcNow.AddDays(eachDayOfWeekBefore) && x.ToDate < DateTime.UtcNow.AddDays(eachDayOfWeekBefore + 1)).Count();
                        model.TimeSeriesChartListRemainingTasks.Add(new TimeSeriesChart()
                        {
                            Day = i.ToString(),
                            TaskCount = countTasks
                        });
                    
                        eachDayOfWeekBefore--;
                    }
                
                return model;
            }
        }
    }
}
