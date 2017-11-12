using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThingsToDo.BL.Interfaces;
using Microsoft.AspNet.Identity;
using ThingsToDo.BL.ServiceHelpers;
using ThingsToDo.App.Helpers;
using ThingsToDo.Models.ViewModels;

namespace ThingsToDo.App.Controllers
{
    public class CategoriesController : BaseHelper
    {
        private readonly ICategoriesService _categoriesService;
        private readonly ITasksService _taskService;

        public CategoriesController(ICategoriesService categoriesService, ITasksService taskService)
        {
            _categoriesService = categoriesService;
            _taskService = taskService;
        }
        public async Task<ActionResult> Show(int ID)
        {
            ViewBag.Title = await _categoriesService.GetCategoryName(ID);

            return View();
        }

        public async Task<ActionResult> GetAllCategories()
        {
            try
            {
                var userId = User.Identity.GetUserId<int>();

                var result = await _categoriesService.GetAllCategoriesAsync(userId);

                if (result.Status == ServiceActionStatus.Ok)
                {
                    return Json(new
                    {
                        status = true,
                        categories = result.Entity
                    }, JsonRequestBehavior.AllowGet);
                }

                return JError();
            }
            catch (Exception ex)
            {
                return JError();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.UserId = User.Identity.GetUserId<int>();

                    var result = await _categoriesService.CreateCategory(model);

                    if (result.Status == ServiceActionStatus.Created)
                    {
                        return JStatus(true, Resources.ThingsToDo.lblCategoryCreated);
                    }
                    else
                    {
                        return JStatus(false, Resources.ThingsToDo.Global_InternalServerError);
                    }
                }
            }
            catch (Exception ex)
            {
                return JError();
            }

            return Redirect("Tasks");
        }
    }
}
