using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WcfService.DataAccess;
using WcfService.Entity;

namespace WcfService
{
    /// <summary>
    /// class that store result strings
    /// </summary>
    public class ResultCodes
    {
        public static string LoginSuccess = "SUCCESS";
        public static string LoginWrongUser = "WRONG_USER";
        public static string LoginWrongPassword = "WRONG_PASSWORD";

        public static string RegisterUserFail = "FAIL";
        public static string RegisterUserExists = "USER_EXISTS";
        public static string RegisterUserSuccess = "SUCCESS";
    }
}