using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WcfService;
using WcfService.Entity;
using WcfService.DataAccess;

namespace WcfDataAdapterTest
{
    [TestClass]
    public class RepoTest
    {
        Repository _repo = new Repository();

        #region Test Utils.cs
        [TestMethod]
        public void TestUtilHash()
        {
            string password1 = "secret";
            string password2 = "secret";
            string password3 = "wrongsecret";

            string hashValue = Utils.GetHash(password1);
            Assert.IsTrue(Utils.VerifyHash(password2, hashValue));
            Assert.IsFalse(Utils.VerifyHash(password3, hashValue));
        }
        #endregion

        #region Test User CRUD + Login
        [TestMethod]
        public void TestAddUser()
        {
            string username = "jerry_nguyen";
            string pass = "password1";
            User user = _repo.AddUser(username, pass);
            Assert.AreEqual(user.Username, username);
            _repo.RemoveUser(user);
        }

        [TestMethod]
        public void TestUserExists()
        {
            string username = "jerry_nguyen";
            string pass = "password1";
            User user = _repo.AddUser(username, pass);

            Assert.IsTrue(_repo.UserExists(username));
            _repo.RemoveUser(user);
        }

        [TestMethod]
        public void TestLogin()
        {
            string username = "jerry_nguyen";
            string wusername = "jerry";
            string pass = "password1";
            string wpass = "password2";

            User user = _repo.AddUser(username, pass);
            int resultInt;
            _repo.Login(username, pass, out resultInt);
            Assert.AreEqual(1, resultInt);
            _repo.Login(wusername, pass, out resultInt);
            Assert.AreEqual(2, resultInt);
            _repo.Login(username, wpass, out resultInt);
            Assert.AreEqual(3, resultInt);
            _repo.RemoveUser(user);
        }

        [TestMethod]
        public void TestChangePassword()
        {
            string username = "test";
            string password = "test";
            string newPassword = "verynew";
            int resultInt;
            User user = _repo.Login(username, password, out resultInt);
            Assert.AreEqual(1, resultInt);
            _repo.ChangePassword(username, newPassword);
            _repo.Login(username, newPassword, out resultInt);
            Assert.AreEqual(1, resultInt);

            _repo.ChangePassword(username, password);
            _repo.Login(username, password, out resultInt);
            Assert.AreEqual(1, resultInt);
        }
        #endregion

        #region Test Category CRUD
        [TestMethod]
        public void TestAddEmptyCategory()
        {
            string title = "Title 1";
            long userId = GetLoginId();
            Category cat = _repo.AddCategory(title, userId);
            Assert.AreEqual(title, cat.Title);
            bool canRemove = _repo.RemoveCategory(userId, cat.Id);
            Assert.IsTrue(canRemove);
        }

        [TestMethod]
        public void TestUpdateCategory()
        {
            long userId = GetLoginId();
            List<Category> cats = _repo.GetAllCategories(userId);
            if (cats.Count > 0)
            {
                Category cat = cats[0];
                long catId = cat.Id;
                cat.Order = 3;
                cat.IconId = 3;
                cat.Title = "Changed Title";
                _repo.UpdateCategory(cat);

                Category newCat = _repo.GetCategory(catId);
                Assert.AreEqual("Changed Title", newCat.Title);
                Assert.AreEqual(3, newCat.Order);
                Assert.AreEqual(3, newCat.IconId);
            }
        }

        /// <summary>
        /// running this test, all categories/tasks belong to user will be deleted
        /// </summary>
        [TestMethod]
        public void TestGetAllCategoriesOfUser()
        {
            long userId = GetLoginId();
            RemoveAllCategories(userId);
            int SIZE = 3;
            for (int i = 0; i < SIZE; i++)
            {
                string title = String.Format("Categories-{0}", i);
                _repo.AddCategory(title, userId);
            }
            List<Category> cats = _repo.GetAllCategories(userId);
            Assert.AreEqual(SIZE, cats.Count);
        }
        #endregion

        #region Test Task CRUD
        [TestMethod]
        public void TestCRUDTask()
        {
            long userId = GetLoginId();
            string title = "My category";
            Category cat = _repo.AddCategory(title, userId);
            Task task = new Task()
            {
                Title = "Task 1",
                Priority = 1,
                CatId = cat.Id,
                Completed = false
            };

            //add + retrieve
            Task newtask = _repo.AddTask(cat.Id, task);
            Task gettask = _repo.GetTask(newtask.Id);
            Assert.AreEqual(newtask.Id, gettask.Id);
            Assert.AreEqual(newtask.Title, newtask.Title);
            Assert.AreEqual(newtask.Priority, newtask.Priority);
            Assert.AreEqual(newtask.Completed, newtask.Completed);

            //update
            string updatedTitle = "Update Title";
            newtask.Title = updatedTitle;
            _repo.UpdateTask(newtask);
            gettask = _repo.GetTask(newtask.Id);
            Assert.AreEqual(updatedTitle, gettask.Title);

            //remove
            _repo.RemoveTask(gettask.Id);
            gettask = _repo.GetTask(gettask.Id);
            Assert.IsNull(gettask); 
        }

        [TestMethod]
        public void TestListTasks()
        {
            long userId = GetLoginId();
            string title = "My category";
            Category cat = _repo.AddCategory(title, userId);
            int Size = 3;
            for (int i = 0; i < Size; i++)
            {
                Task task = new Task()
                {
                    Title = String.Format("Title:{0}", i.ToString()),
                    Priority = i % 3,
                    Completed = false
                };
                _repo.AddTask(cat.Id, task);
            }

            Assert.AreEqual(3, _repo.GetAllTasks(cat.Id).Count);
        }

        #endregion

        [TestCleanup]
        public void CleanUp()
        {
            long userId = GetLoginId();
            //RemoveAllCategories(userId);
        }

        #region helpers
        private void RemoveAllCategories(long userId)
        {
            _repo.ForceRemoveAllCategories(userId);
        }

        private long GetLoginId()
        {
            string username = "test";
            string password = "test";
            User user = null;

            if (!_repo.UserExists(username))
            {
                _repo.AddUser(username, password);
            }
            int resultInt; 
            user = _repo.Login(username, password, out resultInt);

            return user.Id; 
        }
        #endregion
    }
}
