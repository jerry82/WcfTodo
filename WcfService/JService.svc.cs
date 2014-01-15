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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class TodoService : ITodo
    {
        private Repository _repo = new Repository();

        LoginResult ITodo.Login(string username, string password)
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

        RegisterResult ITodo.Register(string username, string password)
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

        bool ITodo.ChangePassword(string username, string newPassword)
        {
            bool success = false;

            if (_repo.UserExists(username))
            {
                _repo.ChangePassword(username, newPassword);
                success = true;
            }

            return success;
        }

        List<Category> ITodo.GetAllCategories(string username)
        {
            return _repo.GetAllCategories(_repo.GetUserId(username));
        }

        bool ITodo.AddCategory(Entity.Category category)
        {
            throw new NotImplementedException();
        }

        bool ITodo.RemoveCategory(Entity.Category category)
        {
            throw new NotImplementedException();
        }
    }
}
