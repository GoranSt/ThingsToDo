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
using ThingsToDo.Resources;


namespace ThingsToDo.BL.Services
{
    public class TasksService : ITasksService
    {
        public async Task<ServiceActionResult<TaskModel>> CreateTask(TaskModel model)
        {
            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {
                var entity = new Tasks
                {
                    Title = model.Title,
                    Description = model.Description,
                    FromDate = Convert.ToDateTime(model.FromDate),
                    ToDate = Convert.ToDateTime(model.ToDate),
                    CategoryId = model.CategoryId,
                    Priority = model.Priority,
                };

                db.Tasks.Add(entity);

                var result = await db.SaveChangesAsync();

                if (result > 0)
                {
                    model.isFinished = false;

                    return new ServiceActionResult<TaskModel>(model, ServiceActionStatus.Created);
                }
                else
                {
                    return new ServiceActionResult<TaskModel>(model, ServiceActionStatus.Error, Resources.ThingsToDo.Global_InternalServerError);
                }
            }
        }

        public async Task<ServiceActionResult<RemovedTasks>> DeleteTask(int taskId)
        {
            using (var db = new ThingsToDoAppContext())
            {
                var entity = await db.Tasks.Where(x => x.Id == taskId).FirstOrDefaultAsync();

                var removedTask = new RemovedTasks
                {
                    TaskId = taskId,
                    Title = entity.Title,
                    Priority = entity.Priority,
                    CategoryId = entity.CategoryId,
                    RemovedDate = DateTime.UtcNow,
                };

                db.RemovedTasks.Add(removedTask);

                var result = await db.SaveChangesAsync();

                db.Tasks.Remove(entity);

                result = await db.SaveChangesAsync();

                if (result > 0)
                {
                    return new ServiceActionResult<RemovedTasks>(removedTask, ServiceActionStatus.Deleted);
                }
                else
                {
                    return new ServiceActionResult<RemovedTasks>(removedTask, ServiceActionStatus.Error); ;
                }
            }
        }

        public async Task<bool> DeleteTask(jQueryDataTableParamModel param)
        {
            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {
                var entity = await db.Tasks.Where(x => x.Id == param.DT_RowId).FirstOrDefaultAsync();

                if (entity != null)
                {
                    db.Tasks.Remove(entity);
                }

                var result = await db.SaveChangesAsync();

                return result > 0 ? true : false;
            }
        }

        public async Task<ServiceActionResult<Tasks>> FinishTask(int taskId)
        {

            using (var db = new ThingsToDoAppContext())
            {
                var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

                if (task != null)
                {
                    task.FinishedDate = DateTime.UtcNow;
                }

                var result = await db.SaveChangesAsync();

                if (result > 0)
                {
                    return new ServiceActionResult<Tasks>(task, ServiceActionStatus.Finished);
                }
                else
                {
                    return new ServiceActionResult<Tasks>(task, ServiceActionStatus.Error);
                }
            }
        }

        public async Task<List<Tasks>> GetAllTasksAsync()
        {
            var model = new List<Tasks>();

            using (var db = new DAL.ThingsToDoAppContext())
            {
                model = await db.Tasks.Where(x => !x.FinishedDate.HasValue).ToListAsync();
            }

            return model;
        }

        public async Task<DataTableResultModel<TaskModel>> GetTasksAsync(int categoryId, jQueryDataTableParamModel param)
        {
            DataTableResultModel<TaskModel> model = new DataTableResultModel<TaskModel>();

            using (var db = new DAL.ThingsToDoAppContext())
            {
                var query = categoryId != 0 ?
                    db.Tasks.Include(x => x.Category).Where(x => x.CategoryId == categoryId && !x.FinishedDate.HasValue).AsNoTracking()
                  : db.Tasks.Include(x => x.Category).AsNoTracking();

                model.recordsTotal = await query.CountAsync();

                var sortColumnIndex = param.iSortCol_0;
                var sortDirection = param.sSortDir_0;

                switch (sortColumnIndex)
                {
                    case 1:
                        query = query.OrderByDirection(x => x.Description, sortDirection);
                        break;
                    default:
                        query = query.OrderByDirection(x => x.Title, sortDirection);
                        break;
                }

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    string searchTerm = param.sSearch.ToLower();
                    query = query.Where(x => x.Title.ToLower().Contains(searchTerm) || x.Description.ToLower().Contains(searchTerm));
                }

                model.recordsFiltered = await query.CountAsync();

                var entities = await query.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToListAsync();

                if (entities.Any())
                {
                    model.data = entities.Select(x => new TaskModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        CategoryName = x.Category.Title,
                        Priority = x.Priority,
                        FromDate = x.FromDate,
                        ToDataTableToDateFormat = x.ToDate.ToString("dd/MM/yyyy"),
                        ToDate = x.ToDate
                    }).ToList();
                }
                model.sEcho = param.sEcho;
            }
            return model;
        }

        public async Task<DataTableResultModel<TaskModel>> GetTasksEditableAsync(int categoryId, jQueryDataTableParamModel param)
        {
            DataTableResultModel<TaskModel> model = new DataTableResultModel<TaskModel>();

            using (var db = new DAL.ThingsToDoAppContext())
            {
                var query = categoryId != 0 ?
                    db.Tasks.Include(x => x.Category).Where(x => x.CategoryId == categoryId && !x.FinishedDate.HasValue)
                  : db.Tasks.Where(x => !x.FinishedDate.HasValue);

                model.recordsTotal = await query.CountAsync();

                var sortColumnIndex = param.iSortCol_0;
                var sortDirection = param.sSortDir_0;

                switch (sortColumnIndex)
                {
                    case 1:
                        query = query.OrderByDirection(x => x.Title, sortDirection);
                        break;
                    case 2:
                        query = query.OrderByDirection(x => x.Description, sortDirection);
                        break;
                    case 3:
                        query = query.OrderByDirection(x => x.ToDate, sortDirection);
                        break;
                    default:
                        query = query.OrderByDirection(x => x.Priority, sortDirection);
                        break;
                }

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    string searchTerm = param.sSearch.ToLower();
                    query = query.Where(x => x.Title.ToLower().Contains(searchTerm) || x.Description.ToLower().Contains(searchTerm));
                }

                model.recordsFiltered = await query.CountAsync();

                var entities = await query.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToListAsync();

                if (entities.Any())
                {
                    model.data = entities.Select(x => new TaskModel()
                    {
                        Id = x.Id,
                        DT_RowId = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        CategoryName = x.Category.Title,
                        Priority = x.Priority,
                        PriorityName = Common.Helpers.CheckPriority.CheckTaskPriority(x.Priority),
                        FinishedDate = x.FinishedDate,
                        isExpired = DateTime.UtcNow > x.ToDate.AddDays(1) ? true : false,
                        FromDate = x.FromDate,
                        CategoryId = x.CategoryId,
                        ToDataTableToDateFormat = x.ToDate.ToString("dd MMMM, yyyy"),
                        ToDate = x.ToDate

                    }).ToList();
                }

                model.sEcho = param.sEcho;
            }
            return model;
        }

        public async Task<bool> Update(int Id, string column, string value)
        {

            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {

                var task = await db.Tasks.Where(x => x.Id == Id && !x.FinishedDate.HasValue).FirstOrDefaultAsync();

                if (column == Resources.ThingsToDo.lblTitle)
                {
                    task.Title = value;
                }
                else if (column == Resources.ThingsToDo.lblDescription)
                {
                    task.Description = value;
                }
                else
                {
                    task.ToDate = Convert.ToDateTime(value);
                }

                var result = await db.SaveChangesAsync();

                return result > 0 ? true : false;
            }
        }

        public async Task<List<TaskModel>> GetAllFinishedTasksAsync(int userId)
        {
            var finishedTasks = new List<TaskModel>();

            using (var db = new ThingsToDoAppContext())
            {
                finishedTasks = await db.Tasks.Include(x => x.Category).Where(x => x.Category.UserId == userId && x.FinishedDate.HasValue).Select(x => new TaskModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    FinishedDate = x.FinishedDate.Value,
                    CategoryName = x.Category.Title,
                    Priority = x.Priority

                }).OrderBy(x => x.FinishedDate).ToListAsync();

                if (finishedTasks.Any())
                {
                    foreach (var task in finishedTasks)
                    {
                        task.PriorityName = Common.Helpers.CheckPriority.CheckTaskPriority(task.Priority);

                    }
                }
            }

            return finishedTasks;
        }

        public async Task<List<TaskModel>> GetAllRemovedTasksAsync(int userId)
        {
            var removedTasks = new List<TaskModel>();

            using (var db = new ThingsToDoAppContext())
            {
                removedTasks = await db.RemovedTasks.Include(x => x.Category).Where(x => x.Category.UserId == userId).Select(x => new TaskModel
                {
                    Id = x.TaskId,
                    Title = x.Title,
                    RemovedDate = x.RemovedDate,
                    CategoryName = x.Category.Title,
                    Priority = x.Priority

                }).OrderBy(x => x.RemovedDate).ToListAsync();
                if (removedTasks.Any())
                {
                    foreach (var task in removedTasks)
                    {
                        task.PriorityName = Common.Helpers.CheckPriority.CheckTaskPriority(task.Priority);
                    }
                }
            }

            return removedTasks;
        }

    }
}
