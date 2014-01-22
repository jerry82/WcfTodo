using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using log4net;
using log4net.Config;

using Microsoft.Practices.Unity;

using WcfService.DataAccess;
using WcfService.Entity;

namespace WcfService
{
    public class JTodoService : ITodoService
    {
        //log4net
        private readonly ILog _log = LogManager.GetLogger(typeof(JTodoService));

        private readonly IRepository _repo;

        public JTodoService()
        {
            try
            {
                _repo = Utils.GetRepository("nosql");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "DataLayer Injection Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }
        }

        LoginResult ITodoService.Login(string username, string password)
        {
            LoginResult result = null;
            try
            {
                int resultInt;
                User user = _repo.Login(username, password, out resultInt);
                LoginStatus status = (LoginStatus)resultInt;
                result = new LoginResult();
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
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault() 
                { 
                    Issue = "Login Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }
           
            return result;
        }

        RegisterResult ITodoService.Register(string username, string password)
        {
            RegisterResult result = new RegisterResult()
            {
                ResultString = ResultCodes.RegisterUserFail,
                UserId = 0
            };

            try
            {
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
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "Register Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }


            return result;
        }

        bool ITodoService.ChangePassword(string username, string newPassword)
        {
            bool success = false;
            try
            {
                if (_repo.UserExists(username))
                {
                    _repo.ChangePassword(username, newPassword);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "ChangePassword Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }

            return success;
        }

        long ITodoService.GetUserId(string username)
        {
            long id = -1;
            try
            {
                id = _repo.GetUserId(username);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "GetUserId Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }

            return id;
        }

        List<Category> ITodoService.GetAllCategories(string username)
        {
            List<Category> list = new List<Category>();

            try
            {
                list = _repo.GetAllCategories(_repo.GetUserId(username));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "GetAllCategories Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }

            return list;
        }

        List<Task> ITodoService.GetAllTasks(long catId)
        {
            List<Task> list = new List<Task>();
            try
            {
                list = _repo.GetAllTasks(catId);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "GetAllTasks Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }

            return list;
        }

        bool ITodoService.AddCategory(Entity.Category category)
        {
            Category cat = null;
            try
            {
                cat = _repo.AddCategory(category);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "AddCategory Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }

            return cat != null;
        }

        bool ITodoService.RemoveCategory(long catId)
        {
            bool success = false;
            try
            {
                success = _repo.RemoveCategory(catId);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "RemoveCategory Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }

            return success;
        }

        Task ITodoService.AddTask(long catId, Task task)
        {
            Task newTask = null;

            try
            {
                newTask = _repo.AddTask(catId, task);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "AddTask Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }

            return newTask;
        }

        Task ITodoService.GetTask(long taskId)
        {
            try
            {
                return _repo.GetTask(taskId);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "GetTask Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }
        }

        void ITodoService.UpdateTask(Task task) 
        {
            try
            {
                _repo.UpdateTask(task);  
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "UpdateTask Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }
        }

        void ITodoService.RemoveTask(long taskId)
        {
            try
            {
                _repo.RemoveTask(taskId);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ServiceDataFault fault = new ServiceDataFault()
                {
                    Issue = "RemoveTask Fail",
                    Details = ex.ToString()
                };
                throw new FaultException<ServiceDataFault>(fault, new FaultReason(fault.Issue));
            }
            
        }
    }
}
