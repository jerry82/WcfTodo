using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Entity
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}