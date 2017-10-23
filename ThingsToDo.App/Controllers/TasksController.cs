using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThingsToDo.App.Helpers;
using ThingsToDo.BL.Interfaces;
using ThingsToDo.BL.ServiceHelpers;
using ThingsToDo.BL.Services;
using ThingsToDo.Models;
using ThingsToDo.Models.Datatables;
using ThingsToDo.Models.Entity;
using ThingsToDo.Models.ViewModels;

namespace ThingsToDo.App.Controllers
{
    public class TasksController : BaseController
    {
        private readonly ITasksService _taskService;
        private readonly ICategoriesService _categoriesService;
        public TasksController(ITasksService taskService, ICategoriesService categoriesService)
        {
            _taskService = taskService;
            _categoriesService = categoriesService;
        }

        // GET: Tasks

        public async Task<ActionResult> Create(int categoryId = 0)
        {
            try
            {
                var userId = User.Identity.GetUserId<int>();
                var model = new TaskModel();
                var categories = await _categoriesService.getAllCategoriesAsync(userId);
                if (categories.Status == ServiceActionStatus.Ok)
                {
                    model.CategoryId = categoryId;
                    model.Categories = categories.Entity;
                    return PartialView("_CreateTask", model);
                }
                else
                {
                    return JError();
                }
            }

            catch (Exception ex)
            {
                return JError();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(TaskModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskService.CreateTask(model);
                    if (result.Status == ServiceActionStatus.Created)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Successfully created task"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (result.Exception != null)
                        {
                            throw result.Exception;
                        }
                        ModelState.AddModelError("", "Internal Server Error!");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return JError();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(int pk, string name, string value)
        {
            try
            {
                var result = await _taskService.Update(pk, name, value);

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = name == Resources.ThingsToDo.lblTitle ? 
                        "Successfully updated task" : name == Resources.ThingsToDo.lblDescription ? 
                        "Successfully updated description" : "Successfully updated date"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", "Internal Server Error!");
                }

                return View();
            }
            catch (Exception ex)
            {
                return JError();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(TaskModel model, jQueryDataTableParamModel param)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskService.DeleteTask(param);
                    if (result)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Successfully deleted task"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Internal Server Error!");
                    }
                }
                return View(param);
            }
            catch (Exception ex)
            {
                return JError();
            }
        }


        public async Task<ActionResult> GetTasksAsync(jQueryDataTableParamModel param, int categoryId = 0)
        {
            try
            {
                var model = await _taskService.GetTasksAsync(categoryId, param);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return JError();
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            try
            {
                var result = await _taskService.DeleteTask(taskId);

                if (result.Status == ServiceActionStatus.Deleted)
                {
                    return JStatus(true, result.Entity.Title + " task removed successfully!");

                }
                else
                {
                    return JError();
                }
            }
            catch (Exception ex)
            {
                return JError();
            }

        }

        public async Task<ActionResult> FinishTask(int taskId)
        {
            try
            {
                var result = await _taskService.FinishTask(taskId);

                if (result.Status == ServiceActionStatus.Finished)
                {
                    return JStatus(true, result.Entity.Title + " task finished successfully!");

                }
                else
                {
                    return JError();
                }
            }
            catch (Exception ex)
            {
                return JError();
            }

        }


        public async Task<ActionResult> GetTasksEditableAsync(jQueryDataTableParamModel param, int categoryId = 0)
        {
            try
            {
                var model = await _taskService.GetTasksEditableAsync(categoryId, param);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JError();
            }
        }

        public ActionResult Index()
        {

            return View();

        }
        public async Task<ActionResult> Finished()
        {
            try
            {
                var userId = User.Identity.GetUserId<int>();

                var result = await _taskService.GetAllFinishedTasksAsync(userId);

                if(result != null)
                {
                   return View(result);
                }
                else
                {
                    return JError();
                }
            }
            catch (Exception ex)
            {
                return JError();
            }
        }

        public async Task<ActionResult> Removed()
        {
            try
            {
                var userId = User.Identity.GetUserId<int>();

                var result = await _taskService.GetAllRemovedTasksAsync(userId);

                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    return JError();
                }
            }
            catch (Exception)
            {
                return JError();
            }
        }

        public ActionResult Details()
        {

            return View();
        }

     
    }
}
