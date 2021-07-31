﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace NovinKargar.Core.Models
{
    public class ServiceHeadLine : IBaseEntity
    {
        public int Id { get; set; }
        [MaxLength(700)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
