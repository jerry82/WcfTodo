using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        long GetUserId(string username);

        List<Category> GetAllCategories(long userId);

        Category AddCategory(Category cat);

        Category GetCategory(long id);

        void UpdateCategory(Category cat);

        bool RemoveCategory(long catId);

        void ForceRemoveAllCategories(long userId);

        Task AddTask(long catId, Task task);

        List<Task> GetAllTasks(long catId);

        Task GetTask(long id);

        void UpdateTask(Task task);

        void RemoveTask(long id);

        List<CIcon> GetAllIcons();

        void ClearAllIcons();

        CIcon AddIcon(CIcon icon);
    }
}
