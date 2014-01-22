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

    /// <summary>
    /// map with wcf object: Category
    /// </summary>
    public class CategoryModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name="Title")]
        public string Title { get; set; }

        public int TaskNum { get; set; }
        
        public int IconId { get; set; }
        
        public long UserId { get; set; }
        
        public int Order { get; set; }

        public List<CIcon> AllIcons { get; set; }

        public Category GetCategoryObject() 
        {
            return new Category()
            {
                Id = this.Id,
                Title = this.Title,
                TaskNum = this.TaskNum,
                IconId = this.IconId,
                UserId = this.UserId,
                Order = this.Order
            };
        }
    }
}