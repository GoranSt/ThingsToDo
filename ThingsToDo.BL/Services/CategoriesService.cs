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

                var entity = new Categories
                {
                    Title = model.Title,
                    UserId = model.UserId
                };

                db.Categories.Add(entity);
                var result = await db.SaveChangesAsync();

                if (result > 0)
                {
                    return new ServiceActionResult<CategoryModel>(model, ServiceActionStatus.Created);
                }
                else
                {
                    return new ServiceActionResult<CategoryModel>(model, ServiceActionStatus.Error, "Error adding new task!");
                }
            }
        }

        public async Task<ServiceActionResult<List<CategoryModel>>> getAllCategoriesAsync(int userId)
        {
            var categories = new List<CategoryModel>();
            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {

                categories = await db.Categories.Where(x => x.UserId == userId).Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Title = x.Title

                }).ToListAsync();
                if (categories != null)
                {
                    return new ServiceActionResult<List<CategoryModel>>(categories, ServiceActionStatus.Ok, "Found");
                }
            }

            return new ServiceActionResult<List<CategoryModel>>(categories, ServiceActionStatus.Error, "Error getting categories!");
        }

        public string GetCategoryName(int categoryId)
        {
            var result = "";
            using (var db = new ThingsToDo.DAL.ThingsToDoAppContext())
            {
                result =  db.Categories.FirstOrDefault(x => x.Id == categoryId).Title;
            }
            return result;
        }

        public async Task<DataTableResultModel<TaskModel>> GetTasksAsync(int id, jQueryDataTableParamModel param)
        {

            DataTableResultModel<TaskModel> model = new DataTableResultModel<TaskModel>();

            using (var db = new DAL.ThingsToDoAppContext())
            {
                var query = db.Tasks.Include(x => x.Category).Where(x => x.CategoryId == id).AsNoTracking();

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
                        CategoryName = x.Category.Title
                    }).ToList();
                }
                model.sEcho = param.sEcho;
            }
            return model;
        }
    }
}
