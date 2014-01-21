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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITodoRestService" in both code and config file together.
    [ServiceContract]
    public interface ITodoRestService
    {
        [WebGet(UriTemplate = "login/{username}/{password}")]
        [OperationContract]
        LoginResult Login(string username, string password);

        [WebInvoke(Method = "POST", UriTemplate = "register/{username}/{password}")]
        [OperationContract]
        RegisterResult Register(string username, string password);

        [WebInvoke(Method = "POST", UriTemplate = "changepass/{username}/{password}")]
        [OperationContract]
        bool ChangePassword(string username, string password);

        [WebGet(ResponseFormat = WebMessageFormat.Xml, UriTemplate = "?username={username}")]
        [OperationContract]
        long GetUserId(string username);

        [WebGet(ResponseFormat = WebMessageFormat.Xml, UriTemplate = "{username}/cats")]
        [OperationContract]
        List<Category> GetAllCategories(string username);

        [WebGet(ResponseFormat = WebMessageFormat.Xml, UriTemplate = "tasks?catId={catId}")]
        [OperationContract]
        List<Task> GetAllTasks(long catId);

        [WebInvoke(Method = "POST", UriTemplate = "cats")]
        [OperationContract]
        bool AddCategory(Category category);

        [WebInvoke(Method = "DELETE", UriTemplate = "cats?id={catId}")]
        [OperationContract]
        bool RemoveCategory(long catId);

        [WebInvoke(Method = "POST", UriTemplate = "tasks/cats?id={catId}")]
        [OperationContract]
        Task AddTask(long catId, Task task);

        [WebInvoke(Method = "PUT", UriTemplate = "tasks")]
        [OperationContract]
        void UpdateTask(Task task);

        [WebGet(ResponseFormat = WebMessageFormat.Xml, UriTemplate = "task?id={taskId}")]
        [OperationContract]
        Task GetTask(long taskId);

        [WebInvoke(Method="DELETE", UriTemplate = "task?id={taskId}")]
        [OperationContract]
        void RemoveTask(long taskId);
    }
}
