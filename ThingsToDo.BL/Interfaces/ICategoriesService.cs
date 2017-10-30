using System.Collections.Generic;
using System.Threading.Tasks;
using ThingsToDo.BL.ServiceHelpers;
using ThingsToDo.Models.Datatables;
using ThingsToDo.Models.Entity;
using ThingsToDo.Models.ViewModels;

namespace ThingsToDo.BL.Interfaces
{
    public interface ICategoriesService
    {
        Task<ServiceActionResult<List<CategoryModel>>> GetAllCategoriesAsync(int userId);
        Task<ServiceActionResult<CategoryModel>> CreateCategory(CategoryModel model);
        Task<DataTableResultModel<TaskModel>> GetTasksAsync(int id, jQueryDataTableParamModel param);
        Task<string> GetCategoryName(int categoryId);
    }
}
