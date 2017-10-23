using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThingsToDo.App.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        // GET: Base
        protected JsonResult JStatus(bool success, string message = null)
        {
            return Json(new { success, message }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult JError()
        {
            if (Request.IsAjaxRequest())
            {
                return JStatus(false, "Internal Server Error!");
            }

            return RedirectToAction("Error", "Home");
        }

        protected ActionResult J500()
        {
            if (Request.IsAjaxRequest())
            {
                Response.StatusCode = 500;

                return Json("Internal Server Error!", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Error", "Home");
        }

        protected JsonResult ReturnModelStateErrors(ModelStateDictionary modelState)
        {
            string stringErrors = "";

            if (modelState.Values.Any())
            {
                foreach (var error in ModelState.Values)
                {
                    foreach (var errorMsg in error.Errors)
                    {
                        if (string.IsNullOrEmpty(stringErrors))
                        {
                            stringErrors += errorMsg.ErrorMessage;
                        }
                        else
                        {
                            stringErrors += " | " + errorMsg.ErrorMessage;
                        }
                    }
                }
            }

            return JStatus(false, stringErrors);
        }

        protected ActionResult Forbidden()
        {
            if (Request.IsAjaxRequest())
            {
                return JStatus(false, "Forbidden");
            }

            return View("Forbidden");
        }


    }
}
