using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using WcfService.Entity;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ITodo
    {
        #region login
        [OperationContract]
        LoginResult Login(string username, string password);

        [OperationContract]
        RegisterResult Register(string username, string password);

        [OperationContract]
        bool ChangePassword(string username, string password);
        #endregion

        [OperationContract]
        List<Category> GetAllCategories(string username);

        [OperationContract]
        List<Task> GetAllTasks(long catId);

        [OperationContract]
        bool AddCategory(Category category);

        [OperationContract]
        bool RemoveCategory(Category category);
    }
}
