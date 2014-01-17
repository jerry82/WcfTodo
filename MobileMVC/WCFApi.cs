﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        private wcfService.TodoClient _wsClient = null;

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
            _wsClient = new wcfService.TodoClient();
        }
        #endregion

        #region user management
        public LoginResult Login(string username, string password)
        {
            return _wsClient.Login(username, password);
        }

        public RegisterResult Register(string username, string password)
        {
            return _wsClient.Register(username, password);
        }

        public bool ChangePassword(string username, string newPassword)
        {
            return _wsClient.ChangePassword(username, newPassword);
        }
        #endregion

        public long GetUserId(string username)
        {
            return _wsClient.GetUserId(username);
        }

        public List<Category> GetAllCategories(string username)
        {
            return _wsClient.GetAllCategories(username).ToList<Category>();
        }

        public void AddCategory(Category cat)
        {
            _wsClient.AddCategory(cat);
        }

        public bool DeleteCategory(long catId)
        {
            return _wsClient.RemoveCategory(catId);
        }

        public List<Task> GetAllTasks(long catId)
        {
            return _wsClient.GetAllTasks(catId).ToList();
        }

        public Task AddTask(long catId, Task task)
        {
            return _wsClient.AddTask(catId, task);
        }

        public Task GetTask(long taskId)
        {
            return _wsClient.GetTask(taskId);
        }

        public void UpdateTask(Task task)
        {
            _wsClient.UpdateTask(task);
        }

        public void RemoveTask(long taskId)
        {
            _wsClient.RemoveTask(taskId);
        }
    }
}