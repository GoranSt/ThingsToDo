using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingsToDo.Models.ViewModels;

namespace ThingsToDo.BL.Interfaces
{
    public interface IStatisticsService
    {
        #region Public
        StatisticModel GetAllStatistics(int userId);
        #endregion
    }
}
