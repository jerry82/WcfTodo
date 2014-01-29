using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Security;

using System.Security.Cryptography.X509Certificates;

using MobileMVC.wcfService;

namespace MobileMVC
{
    /// <summary>
    /// class that interfaces with the service
    /// implemented with singleton pattern
    /// </summary>
    public class WcfApi
    {
        #region declaration
        private static WcfApi _instance = null;
        private wcfService.TodoServiceClient _wsClient = null;

        public static WcfApi Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WcfApi();
                }
                return _instance;
            }
        }

        private WcfApi()
        {
            try
            {
                _wsClient = new wcfService.TodoServiceClient();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion

        #region user management
        public LoginResult Login(string username, string password)
        {
            LoginResult result = null;
            try
            {
                result = _wsClient.Login(username, password); 
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return result;
        }

        public RegisterResult Register(string username, string password)
        {
            RegisterResult result = null;

            try
            {
                result = _wsClient.Register(username, password);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }

            return result;
        }

        public bool ChangePassword(string username, string newPassword)
        {
            bool result = false;
            try
            {
                result = _wsClient.ChangePassword(username, newPassword);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }

            return result;
        }

        public long GetUserId(string username)
        {
            long id = -1;
            try
            {
                id = _wsClient.GetUserId(username);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return id;
        }
        #endregion

        #region category
        public List<Category> GetAllCategories(string username)
        {
            List<Category> cats = new List<Category>();
            try
            {
                cats = _wsClient.GetAllCategories(username).ToList<Category>();
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return cats;
        }

        public Category GetCategory(long id)
        {
            Category cat = null;
            try
            {
                cat = _wsClient.GetCategory(id);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return cat;
        }

        public bool UpdateCategory(Category cat)
        {
            bool success = false;
            try
            {
                success = _wsClient.UpdateCategory(cat);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return success;
        }

        public void AddCategory(Category cat)
        {
            try
            {
                _wsClient.AddCategory(cat);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
        }

        public bool DeleteCategory(long catId)
        {
            bool success = false;

            try
            {
                success = _wsClient.RemoveCategory(catId);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return success;
        }

        #endregion

        #region tasks
        public List<Task> GetAllTasks(long catId)
        {
            List<Task> tasks = new List<Task>();
            try
            {
                tasks = _wsClient.GetAllTasks(catId).ToList();
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return tasks;
        }

        public Task AddTask(long catId, Task task)
        {
            Task newTask = null;
            try
            {
                newTask = _wsClient.AddTask(catId, task);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return newTask;
        }

        public Task GetTask(long taskId)
        {
            Task task = null;
            try
            {
                task = _wsClient.GetTask(taskId);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
            return task; 
        }

        public void UpdateTask(Task task)
        {
            try
            {
                _wsClient.UpdateTask(task);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
        }

        public void RemoveTask(long taskId)
        {
            try
            {
                _wsClient.RemoveTask(taskId);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }
        }
        #endregion

        #region icons
        public List<CIcon> GetAllIcons()
        {
            List<CIcon> icons = new List<CIcon>();

            try
            {
                icons = _wsClient.GetAllIcons().ToList<CIcon>();
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }

            return icons;
        }

        public CIcon GetIcon(long id)
        {
            CIcon icon = null;

            try
            {
                icon = _wsClient.GetIcon(id);
            }
            catch (FaultException<ServiceDataFault> fault)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(fault.Detail.Details));
            }

            return icon;
        }
        #endregion
    }
}