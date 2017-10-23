using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingsToDo.Models;
using ThingsToDo.Models.Datatables;
using ThingsToDo.Models.ViewModels;
using ThingsToDo.Models.Entity;
using ThingsToDo.BL.ServiceHelpers;

namespace ThingsToDo.BL.Interfaces
{
    public interface ITasksService
    {
        #region Tasks
        Task<List<Tasks>> getAllTasksAsync();
        Task<DataTableResultModel<TaskModel>> GetTasksAsync(int categoryId, jQueryDataTableParamModel param);
        Task<DataTableResultModel<TaskModel>> GetTasksEditableAsync(int categoryId, jQueryDataTableParamModel param);
        Task<ServiceActionResult<TaskModel>> CreateTask(TaskModel model);
        Task<bool> Update(int Id, string column, string value);
        Task<List<TaskModel>> GetAllFinishedTasksAsync(int userId);
        Task<List<TaskModel>> GetAllRemovedTasksAsync(int userId);
        Task<bool> DeleteTask(jQueryDataTableParamModel param);
        Task<ServiceActionResult<Tasks>> FinishTask(int taskId);
        Task<ServiceActionResult<RemovedTasks>> DeleteTask(int taskId);
        #endregion
    }
}
