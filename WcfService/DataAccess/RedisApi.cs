using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

using WcfService.Entity;

namespace WcfService.DataAccess
{
    //TODO: to implement IoC 
    public class RedisApi
    {
        private static IRedisTypedClient<User> _userDB = null;
        public static IRedisTypedClient<User> UserDB
        {
            get
            {
                if (_userDB == null)
                {
                    _userDB = Client.As<User>();
                }
                return _userDB;
            }
        }

        private static IRedisTypedClient<Category> _categoryDB = null;
        public static IRedisTypedClient<Category> CategoryDB
        {
            get
            {
                if (_categoryDB == null)
                    _categoryDB = Client.As<Category>();
                return _categoryDB;
            }
        }

        private static IRedisTypedClient<Task> _taskDB = null;
        public static IRedisTypedClient<Task> TaskDB
        {
            get
            {
                if (_taskDB == null)
                    _taskDB = Client.As<Task>();
                return _taskDB;
            }
        }

        private static IRedisTypedClient<CIcon> _iconDB = null;
        public static IRedisTypedClient<CIcon> IconDB
        {
            get
            {
                if (_iconDB == null)
                    _iconDB = Client.As<CIcon>();
                return _iconDB;
            }
        }

        public static RedisClient Client = new RedisClient("localhost");
        public static IRedisClientsManager RedisManager = new BasicRedisClientManager(new string[] { "localhost" });
    }
}