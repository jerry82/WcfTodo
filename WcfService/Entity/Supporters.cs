using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Entity
{
    /// <summary>
    /// return login status 
    /// </summary>
    public enum LoginStatus
    {
        Success = 1,
        WrongUser = 2,
        WrongPass = 3,
        Fail = 4
    }

}