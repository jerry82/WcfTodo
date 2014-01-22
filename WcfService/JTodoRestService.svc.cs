using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using WcfService.DataAccess;
using WcfService.Entity;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TodoRestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TodoRestService.svc or TodoRestService.svc.cs at the Solution Explorer and start debugging.
    public class TodoRestService : ITodoRestService
    {
        private RedisRepository _repo = new RedisRepository();

        public long GetUserId(string username)
        {
            return _repo.GetUserId(username);
        }

        public List<Category> GetAllCategories(string username)
        {
            return _repo.GetAllCategories(_repo.GetUserId(username));
        }

        public List<Task> GetAllTasks(long catId)
        {
            return _repo.GetAllTasks(catId);
        }

        public Task GetTask(long taskId)
        {
            return _repo.GetTask(taskId);
        }

        public void RemoveTask(long taskId)
        {
            _repo.RemoveTask(taskId);
        }

        public bool AddCategory(Category category)
        {
            return _repo.AddCategory(category) != null;
        }

        public bool RemoveCategory(long catId)
        {
            return _repo.RemoveCategory(catId);
        }

        public Task AddTask(long catId, Task task)
        {
            return _repo.AddTask(catId, task);
        }

        public void UpdateTask(Task task)
        {
            _repo.UpdateTask(task);
        }

        public LoginResult Login(string username, string password)
        {
            int resultInt;
            User user = _repo.Login(username, password, out resultInt);
            LoginStatus status = (LoginStatus)resultInt;
            LoginResult result = new LoginResult();
            switch (status)
            {
                case (LoginStatus.Success):
                    result.ResultString = ResultCodes.LoginSuccess;
                    result.UserId = user.Id;
                    result.Username = user.Username;
                    break;
                case (LoginStatus.WrongPass):
                    result.ResultString = ResultCodes.LoginWrongPassword;
                    break;
                case (LoginStatus.WrongUser):
                    result.ResultString = ResultCodes.LoginWrongUser;
                    break;
            }

            return result;
        }

        public RegisterResult Register(string username, string password)
        {
            RegisterResult result = new RegisterResult();
            result.ResultString = ResultCodes.RegisterUserFail;
            result.UserId = 0;

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                if (!_repo.UserExists(username))
                {
                    User user = _repo.AddUser(username, password);
                    result.ResultString = ResultCodes.RegisterUserSuccess;
                    result.Username = user.Username;
                    result.UserId = user.Id;
                }
                else
                {
                    result.ResultString = ResultCodes.RegisterUserExists;
                }
            }

            return result;
        }

        public bool ChangePassword(string username, string newPassword)
        {
            bool success = false;

            if (_repo.UserExists(username))
            {
                _repo.ChangePassword(username, newPassword);
                success = true;
            }

            return success;
        }
    }
}
