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
    [ServiceContract]
    public interface ITodoService
    {
        [FaultContract(typeof(ServiceDataFault))]
        [OperationContract]
        LoginResult Login(string username, string password);

        [OperationContract]
        RegisterResult Register(string username, string password);

        [OperationContract]
        bool ChangePassword(string username, string password);

        [OperationContract]
        long GetUserId(string username);

        [OperationContract]
        List<Category> GetAllCategories(string username);

        [OperationContract]
        List<Task> GetAllTasks(long catId);

        [OperationContract]
        Category GetCategory(long catId);

        [OperationContract]
        bool AddCategory(Category category);

        [OperationContract]
        bool UpdateCategory(Category category);

        [OperationContract]
        bool RemoveCategory(long catId);

        [OperationContract]
        Task AddTask(long catId, Task task);

        [OperationContract]
        void UpdateTask(Task task);

        [OperationContract]
        Task GetTask(long taskId);

        [OperationContract]
        void RemoveTask(long taskId);

        [OperationContract]
        List<CIcon> GetAllIcons();

        [OperationContract]
        CIcon GetIcon(long id);
        
    }
}
