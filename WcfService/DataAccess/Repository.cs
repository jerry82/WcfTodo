﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.Redis;
using ServiceStack.Text;

using WcfService.Entity;

namespace WcfService.DataAccess
{
    public interface IRepository
    {
        User Login(string username, string password, out int resultInt);

        User AddUser(string username, string password);
        
        bool UserExists(string username);

        void ChangePassword(string username, string newPassword);
        
        void RemoveUser(User user);
        
        void RemoveAllUsers();

        List<Category> GetAllCategories(long userId);

        Category AddCategory(string title, long userId);

        Category GetCategory(long id);

        void UpdateCategory(Category cat);

        bool RemoveCategory(long userId, long catId);

        void ForceRemoveAllCategories(long userId);

        Task AddTask(long catId, Task task);

        List<Task> GetAllTasks(long catId);

        Task GetTask(long id);

        void UpdateTask(Task task);

        void RemoveTask(long id);
    }

    /// <summary>
    /// return login status 
    /// </summary>
    public enum LoginStatus
    {
        Success = 1,
        WrongUser = 2, 
        WrongPass = 3,
        Fail = 4
    }

    public class Repository : IRepository
    {
        //define all redis keys userd for indexing
        static class UserCategoryIndex
        {
            public static string Categories(long userId) { return String.Format("urn:user>cat:{0}", userId); }
        }

        static class CategoryTaskIndex
        {
            public static string Tasks(long catId) { return String.Format("urn:cat>task:{0}", catId); }
        }

        /// <summary>
        /// constructore: empty
        /// </summary>
        public Repository() { }

        #region user/login
        public User Login(string username, string password, out int resultInt)
        {
            LoginStatus result = LoginStatus.WrongUser;
            User user = RedisApi.UserDB.GetAll().FirstOrDefault(item => item.Username.Equals(username));

            if (user != null)
            {
                result = Utils.VerifyHash(password, ((User)user).PasswordHash) ? LoginStatus.Success : LoginStatus.WrongPass;
            }
            resultInt = Convert.ToInt32(result);

            return user;
        }

        public bool UserExists(string username)
        {
            var user = RedisApi.UserDB.GetAll().FirstOrDefault(item => item.Username.Equals(username));
            return user != null;
        }

        public void ChangePassword(string username, string newPassword)
        {
            var user = RedisApi.UserDB.GetAll().FirstOrDefault(item => item.Username.Equals(username));
            if (user != null)
            {
                user.PasswordHash = Utils.GetHash(newPassword);
                RedisApi.UserDB.Store(user);
            }
        }

        public User AddUser(string username, string password)
        {
            string hash = Utils.GetHash(password);
            var userDB = RedisApi.UserDB;
            User user = new User() { Id = userDB.GetNextSequence(), Username = username, PasswordHash = hash };
            return userDB.Store(user);
        }

        public void RemoveUser(User user)
        {
            RedisApi.UserDB.DeleteById(user.Id);
        }

        public void RemoveAllUsers()
        {
            RedisApi.UserDB.DeleteAll();
        }
        #endregion

        #region categories
        /// <summary>
        /// get all categories that belong to a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Category> GetAllCategories(long userId)
        {
            var catIds = RedisApi.Client.GetAllItemsFromSet(UserCategoryIndex.Categories(userId));
            return RedisApi.CategoryDB.GetByIds(catIds).ToList();
        }

        //TODO: to change IconId
        public Category AddCategory(string title, long userId)
        {
            Category cat = new Category() 
            {
                Id = RedisApi.CategoryDB.GetNextSequence(), 
                Title = title, 
                UserId = userId, 
                IconId = 1,
                Order = 1
            };

            Category returnCat = RedisApi.CategoryDB.Store(cat);
            RedisApi.Client.AddItemToSet(UserCategoryIndex.Categories(userId), returnCat.Id.ToString());

            return returnCat;
        }

        public Category GetCategory(long id)
        {
            return RedisApi.CategoryDB.GetById(id);
        }

        public void UpdateCategory(Category cat)
        {
            RedisApi.CategoryDB.Store(cat);
        }

        /// <summary>
        /// only can remove empty category
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="catId"></param>
        public bool RemoveCategory(long userId, long catId)
        {
            //only allow to remove empty categories
            var taskIds = RedisApi.Client.GetAllItemsFromSet(CategoryTaskIndex.Tasks(catId));
            if (taskIds.Count > 0)
            {
                return false;
            }

            //remove cat>task relationship
            RedisApi.CategoryDB.DeleteById(catId);
            RedisApi.Client.RemoveItemFromSet(UserCategoryIndex.Categories(userId), catId.ToString());
            return true;
        }

        /// <summary>
        /// Force remove all the categories even though they have tasks.
        /// Use this function with care
        /// </summary>
        /// <param name="userId"></param>
        public void ForceRemoveAllCategories(long userId)
        {
            var catIds = RedisApi.Client.GetAllItemsFromSet(UserCategoryIndex.Categories(userId));

            //removate all tasks and cat>task relationship
            catIds.ToList().ForEach(catId => 
            {
                var taskIds = RedisApi.Client.GetAllItemsFromSet(CategoryTaskIndex.Tasks(long.Parse(catId)));
                taskIds.ToList().ForEach(item =>
                {
                    RedisApi.TaskDB.DeleteById(long.Parse(item));
                    RedisApi.Client.RemoveItemFromSet(CategoryTaskIndex.Tasks(long.Parse(catId)), item);
                });

                //after the categories are empty, process to delete them
                RemoveCategory(userId, long.Parse(catId));
            });
        }

        public Task AddTask(long catId, Task task)
        {
            task.Id = RedisApi.TaskDB.GetNextSequence();

            RedisApi.TaskDB.Store(task);
            RedisApi.Client.AddItemToSet(CategoryTaskIndex.Tasks(catId), task.Id.ToString());

            return task;
        }

        public List<Task> GetAllTasks(long catId)
        {
            var taskIds = RedisApi.Client.GetAllItemsFromSet(CategoryTaskIndex.Tasks(catId));
            return RedisApi.TaskDB.GetByIds(taskIds).ToList();
        }

        public Task GetTask(long id)
        {
            return RedisApi.TaskDB.GetById(id);
        }

        public void UpdateTask(Task task)
        {
            RedisApi.TaskDB.Store(task);
        }

        public void RemoveTask(long id)
        {
            var task = RedisApi.TaskDB.GetById(id);
            RedisApi.TaskDB.DeleteById(id);
            RedisApi.Client.RemoveItemFromSet(CategoryTaskIndex.Tasks(task.CatId), id.ToString());
        }

        #endregion

        #region helpers
        public long GetUserId(string username)
        {
            long id = -1;
            if (!String.IsNullOrEmpty(username))
            {
                var user = RedisApi.UserDB.GetAll().ToList().FirstOrDefault(item => item.Username.Equals(username));
                if (user != null)
                    id = ((User)user).Id;
            }
            return id;
        }
        #endregion
    }
}