using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using ThingsToDo.DAL;
using ThingsToDo.Models.Entity;
using ThingsToDo.BL.Interfaces;
using ThingsToDo.BL.Services;
using Microsoft.AspNet.Identity;
using ThingsToDo.BL.ServiceHelpers;
using ThingsToDo.App.Helpers;
using ThingsToDo.Models.Datatables;
using ThingsToDo.Models.ViewModels;

namespace ThingsToDo.App.Controllers
{
    public class CategoriesController : BaseHelper
    {
        private ThingsToDoAppContext db = new ThingsToDoAppContext();
        private readonly ICategoriesService _categoriesService;
        private readonly ITasksService _taskService;
        public CategoriesController()
        {
            _categoriesService = new CategoriesService();
            _taskService = new TasksService();
        }
        // GET: Categories
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        public ActionResult Show(int id)
        {
            ViewBag.Title = _categoriesService.GetCategoryName(id);

            return View();
        }
            
        public async Task<ActionResult> GetAllCategories()
        {
            try
            {
                var userId = User.Identity.GetUserId<int>();

                var result = await _categoriesService.getAllCategoriesAsync(userId);
                if (result.Status == ServiceActionStatus.Ok)
                {
                    return Json(new
                    {
                        status = true,
                        categories = result.Entity
                    },
                        JsonRequestBehavior.AllowGet);
                }
                return JError();
            }
            catch (Exception ex)
            {
                return JError();
            }
        }

        // GET: Categories/Details/5


        // GET: Categories/Create
        //public ActionResult Create()
        //{
        //    try
        //    {
        //        var model = new CategoryModel();

        //        return PartialView("_CreateCategory", model);

        //    }
        //    catch (Exception ex)
        //    {
        //        return JError();
        //    }
        //}

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                        return JStatus(true, "Category created successfully!");
                    }
                    else
                    {
                        return JStatus(false, "Internal server error");
                    }
                }
            }

            catch (Exception ex)
            {

                return JError();
            }



            return Redirect("Tasks");
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categoriesModel = await db.Categories.FindAsync(id);
            if (categoriesModel == null)
            {
                return HttpNotFound();
            }
            return View(categoriesModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,UserId")] Categories categoriesModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriesModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoriesModel);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categoriesModel = await db.Categories.FindAsync(id);
            if (categoriesModel == null)
            {
                return HttpNotFound();
            }
            return View(categoriesModel);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Categories categoriesModel = await db.Categories.FindAsync(id);
            db.Categories.Remove(categoriesModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
