using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThingsToDo.BL.Extensions;
using ThingsToDo.BL.Interfaces;
using ThingsToDo.BL.ServiceHelpers;
using ThingsToDo.Models.Datatables;
using ThingsToDo.Models.Entity;
using ThingsToDo.Models.ViewModels;

namespace ThingsToDo.BL.Services
{
    public class CategoriesService : ICategoriesService
    {
        public async Task<ServiceActionResult<CategoryModel>> CreateCategory(CategoryModel model)
        {
            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {
                db.Categories.Add(new Categories
                {
                    Title = model.Title,
                    UserId = model.UserId
                });

                var result = await db.SaveChangesAsync();

                if (result > 0)
                {
                    return new ServiceActionResult<CategoryModel>(model, ServiceActionStatus.Created);
                }
                else
                {
                    return new ServiceActionResult<CategoryModel>(model, ServiceActionStatus.Error, Resources.ThingsToDo.lblTaskCreateError);
                }
            }
        }

        public async Task<ServiceActionResult<List<CategoryModel>>> GetAllCategoriesAsync(int userId)
        {
            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {
                var categories = await db.Categories.Where(x => x.UserId == userId).Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToListAsync();

                if (categories != null)
                {
                    return new ServiceActionResult<List<CategoryModel>>(categories, ServiceActionStatus.Ok, Resources.ThingsToDo.lblFound);
                }
                else
                {
                    return new ServiceActionResult<List<CategoryModel>>(categories, ServiceActionStatus.Error, Resources.ThingsToDo.Global_InternalServerError);
                }
            }
        }

        public async Task<string> GetCategoryName(int categoryId)
        {
            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {
                var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

                return category != null ? category.Title : "";
            }
        }

        public async Task<DataTableResultModel<TaskModel>> GetTasksAsync(int id, jQueryDataTableParamModel param)
        {

            DataTableResultModel<TaskModel> model = new DataTableResultModel<TaskModel>();

            using (var db = new DAL.ThingsToDoAppContext())
            {
                var queryTasks = db.Tasks.Include(x => x.Category).Where(x => x.CategoryId == id).AsNoTracking();

                model.recordsTotal = await queryTasks.CountAsync();
                var sortColumnIndex = param.iSortCol_0;
                var sortDirection = param.sSortDir_0;

                switch (sortColumnIndex)
                {
                    case 1:
                        queryTasks = queryTasks.OrderByDirection(x => x.Description, sortDirection);
                        break;
                    default:
                        queryTasks = queryTasks.OrderByDirection(x => x.Title, sortDirection);
                        break;
                }

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    string searchTerm = param.sSearch.ToLower();
                    queryTasks = queryTasks.Where(x => x.Title.ToLower().Contains(searchTerm) || x.Description.ToLower().Contains(searchTerm));
                }

                model.recordsFiltered = await queryTasks.CountAsync();

                if (queryTasks.Any())
                {
                    model.data = await queryTasks.Skip(param.iDisplayStart).Take(param.iDisplayLength).Select(x => new TaskModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        CategoryName = x.Category.Title
                    }).ToListAsync();
                }

                model.sEcho = param.sEcho;
            }

            return model;
        }
    }
}
