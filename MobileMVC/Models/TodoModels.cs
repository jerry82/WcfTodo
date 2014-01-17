using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

using MobileMVC.wcfService;

namespace MobileMVC.Models
{
    public class TaskMainViewModel
    {
        public Task TaskObj { get; set; }
        public List<Task> TaskList { get; set; }
    }
}