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
        private string _SiteName = String.Empty;

        public TodoController()
        {
            _SiteName = ViewBag.Message = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();
        }
        #region Category
        //
        // GET: /Todo/
        public ActionResult Index()
        {
            ViewBag.PageName = "CategoryMain";
            List<Category> categories = WcfApi.Instance.GetAllCategories(User.Identity.Name);
            List<CategoryIconModel> cateIconModel = new List<CategoryIconModel>();

            foreach (Category cat in categories)
            {
                CategoryIconModel cateModel = new CategoryIconModel()
                {
                    Id = cat.Id, 
                    Order = cat.Order, 
                    TaskNum = cat.TaskNum, 
                    UserId = cat.UserId, 
                    Title = cat.Title, 
                    IconId = cat.IconId
                };
                cateModel.CIcon = WcfApi.Instance.GetIcon(cat.IconId);
                cateIconModel.Add(cateModel);
            }

            return View(cateIconModel);
        }
        
        //
        // GET: /Todo/CategoryAddEdit/
        public ActionResult CategoryAdd()
        {
            CategoryModel dummy = new CategoryModel();
            dummy.IconModel = new IconSelectModel()
            {
                //default
                SelectedId = 1, 
                Icons = WcfApi.Instance.GetAllIcons()
            };

            return View("CategoryAddEdit", dummy);
        }

        //
        // POST: /Todo/CategoryAdd
        [HttpPost]
        public ActionResult CategoryAdd(CategoryModel catModel)
        {
            if (ModelState.IsValid)
            {
                long userId = WcfApi.Instance.GetUserId(User.Identity.Name);
                catModel.UserId = userId;

                Category cat = catModel.GetCategoryObject();
                if (cat.IconId == 0)
                    cat.IconId = 1;
                WcfApi.Instance.AddCategory(cat);
                return RedirectToAction("Index");
            }

            catModel.IconModel = new IconSelectModel()
            {
                //default
                SelectedId = 1,
                Icons = WcfApi.Instance.GetAllIcons()
            };

            return View("CategoryAddEdit", catModel);
        }

        // Get: /Todo/CategoryEdit/id
        public ActionResult CategoryAddEdit(long id)
        {
            Category cat = WcfApi.Instance.GetCategory(id);

            CategoryModel model = new CategoryModel()
            {
                Id = cat.Id,
                Order = cat.Order,
                TaskNum = cat.TaskNum,
                Title = cat.Title,
                UserId = cat.UserId,
                IconId = cat.IconId,
            };

            model.IconModel = new IconSelectModel() { 
                SelectedId = cat.IconId,
                Icons = WcfApi.Instance.GetAllIcons()
            };

            return View(model);
        }

        //
        // Post: /Todo/CategoryEdit/id
        [HttpPost]
        public ActionResult CategoryAddEdit(CategoryModel catModel)
        {
            if (ModelState.IsValid)
            {
                long userId = WcfApi.Instance.GetUserId(User.Identity.Name);
                catModel.UserId = userId;

                Category cat = catModel.GetCategoryObject();
                if (cat.IconId == 0)
                    cat.IconId = 1;
                WcfApi.Instance.UpdateCategory(cat);
                return RedirectToAction("Index");
            }

            catModel.IconModel = new IconSelectModel()
            {
                //default
                SelectedId = catModel.IconId,
                Icons = WcfApi.Instance.GetAllIcons()
            };

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
