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
        #region Category
        //
        // GET: /Todo/
        public ActionResult Index()
        {
            ViewBag.PageName = "CategoryMain";
            var model = WcfApi.Instance.GetAllCategories(User.Identity.Name);
            return View(model);
        }
        
        //
        // GET: /Todo/CategoryAdd/
        public ActionResult CategoryAdd()
        {
            CategoryModel dummy = new CategoryModel();
            dummy.AllIcons = WcfApi.Instance.GetAllIcons();
            return View(dummy);
        }

        //
        // POST: /Todo/CategoryAdd/Task
        [HttpPost]
        public ActionResult CategoryAdd(CategoryModel catModel)
        {
            if (ModelState.IsValid)
            {
                long userId = WcfApi.Instance.GetUserId(User.Identity.Name);
                catModel.UserId = userId;
                //TODO: to change
                catModel.IconId = 1;

                Category cat = catModel.GetCategoryObject();
                WcfApi.Instance.AddCategory(cat);
                return RedirectToAction("Index");
            }

            return View(catModel);
        }

        //
        //GET: /Todo/DeleteCategory?catId=
        public ActionResult DeleteCategory(long id)
        {
            WcfApi.Instance.DeleteCategory(id);
            return this.RedirectToAction("Index");
        }

        #endregion

        #region Task

        //
        // GET: /Todo/Task/id

        public ActionResult Tasks(long id)
        {
            ViewBag.PageName = "TaskMain";
            ViewBag.Message = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();
            Task dummyTask = new Task() { CatId = id };
            List<Task> taskList = WcfApi.Instance.GetAllTasks(id);
            var model = new TaskMainViewModel() { TaskObj = dummyTask, TaskList = taskList };

            return View(model);
        }

        //
        // Post: /Todo/AddTask/

        [HttpPost]
        public ActionResult Tasks(Task task)
        {
            if (!String.IsNullOrEmpty(task.Title))
            {
                Task newTask = WcfApi.Instance.AddTask(task.CatId, task);
            }
            return this.RedirectToAction("Tasks", new { id = task.CatId });
        }

        //
        // Get: /Todo/DeleteTask?catId=&taskId=

        public ActionResult DeleteTask(long catId, long taskId)
        {
            WcfApi.Instance.RemoveTask(taskId);
            return this.RedirectToAction("Tasks", new { id = catId });
        }

        public ActionResult SwitchStatusTask(long catId, long taskId)
        {
            Task task = WcfApi.Instance.GetTask(taskId);
            task.Completed = !task.Completed;
            WcfApi.Instance.UpdateTask(task);
            return this.RedirectToAction("Tasks", new { id = catId });
        }
        #endregion
    }
}
