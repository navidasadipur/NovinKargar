using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace NovinKargar.Core.Models
{
    public class Service : IBaseEntity
    {
        public int Id { get; set; }
        [Display(Name = "عنوان سرویس")]
        [MaxLength(600,ErrorMessage = "{0} باید از 600 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "توضیح کوتاه")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public int ViewCount { get; set; }
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        public DateTime? AddedDate { get; set; }

        public int? ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<ServiceHeadLine> ServiceHeadLines { get; set; }

        public ICollection<ServiceOrder> ServiceOrders { get; set; }

        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
