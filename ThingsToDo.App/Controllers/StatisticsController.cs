using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThingsToDo.BL.Interfaces;

namespace ThingsToDo.App.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly ITasksService _taskService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(ITasksService taskService, ICategoriesService categoriesService, IStatisticsService statisticsService)
        {
            _taskService = taskService;
            _categoriesService = categoriesService;
            _statisticsService = statisticsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStatistics()
        {
            try
            {
                var userId = User.Identity.GetUserId<int>();

                var result =  _statisticsService.GetAllStatistics(userId);

                if(result != null)
                {
                    return Json(new { success = true, statistics = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JStatus(false, Resources.ThingsToDo.lblErrorLoadingStatistics);
                }
            }
            catch (Exception ex)
            {
                return JError();
            }
        }
    }
}