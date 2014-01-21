using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using WcfService.DataAccess;
using WcfService.Entity;

namespace WcfService
{
    public class JTodoService : ITodoService
    {
        private Repository _repo = new Repository();

        LoginResult ITodoService.Login(string username, string password)
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

        RegisterResult ITodoService.Register(string username, string password)
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

        bool ITodoService.ChangePassword(string username, string newPassword)
        {
            bool success = false;

            if (_repo.UserExists(username))
            {
                _repo.ChangePassword(username, newPassword);
                success = true;
            }

            return success;
        }

        long ITodoService.GetUserId(string username)
        {
            return _repo.GetUserId(username);
        }

        List<Category> ITodoService.GetAllCategories(string username)
        {
            return _repo.GetAllCategories(_repo.GetUserId(username));
        }

        List<Task> ITodoService.GetAllTasks(long catId)
        {
            return _repo.GetAllTasks(catId);
        }

        bool ITodoService.AddCategory(Entity.Category category)
        {
            Category cat = _repo.AddCategory(category);
            return cat != null;
        }

        bool ITodoService.RemoveCategory(long catId)
        {
            return _repo.RemoveCategory(catId);
        }

        Task ITodoService.AddTask(long catId, Task task)
        {
            return _repo.AddTask(catId, task);
        }

        Task ITodoService.GetTask(long taskId)
        {
            return _repo.GetTask(taskId);
        }

        void ITodoService.UpdateTask(Task task) 
        {
            _repo.UpdateTask(task);   
        }

        void ITodoService.RemoveTask(long taskId)
        {
            _repo.RemoveTask(taskId);
        }
    }
}
