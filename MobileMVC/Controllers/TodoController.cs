using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using MobileMVC.Models;
using MobileMVC.wcfService;

namespace MobileMVC.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        //
        // GET: /Todo/

        public ActionResult Index()
        {
            ViewBag.PageName = "CategoryMain";
            var model = WcfApi.Instance.GetAllCategories(User.Identity.Name);
            return View(model);
        }

        //
        // GET: /Todo/Items/id

        public ActionResult Tasks(long id)
        {
            ViewBag.PageName = "TaskMain";
            var model = WcfApi.Instance.GetAllTasks(id);
            return View(model);
        }
    }
}
