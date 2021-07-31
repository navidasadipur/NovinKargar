using NovinKargar.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NovinKargar.Web.ViewModels
{
    public class ServiceFormViewModel
    {
        public int Id { get; set; }
        [Display(Name = "عنوان مقاله")]
        [MaxLength(600, ErrorMessage = "{0} باید از 600 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public int ServiceCategoryId { get; set; }
        public HttpPostedFileBase ServiceImage { get; set; }

        public List<ServiceHeadLineViewModel> ServiceHeadLines { get; set; }
    }

    public class ServiceHeadLineViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
    }

    public class ServiceInfoViewModel
    {
        public ServiceInfoViewModel()
        {
            
        }

        public ServiceInfoViewModel(Service service)
        {
            this.Id = service.Id;
            this.Title = service.Title;
            this.Author = service.User != null? $"{service.User.FirstName} {service.User.LastName}" : "-";
            this.ServiceCategory = service.ServiceCategory != null? service.ServiceCategory.Title : "-";
            this.PersianAddedDate = service.AddedDate != null? new PersianDateTime(service.AddedDate.Value).ToString() : "-";
            this.AddedDate = service.AddedDate;
        }
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "نویسنده")]
        public string Author { get; set; }
        [Display(Name = "دسته بندی")]
        public string ServiceCategory { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public string PersianAddedDate { get; set; }
        public DateTime? AddedDate { get; set; }
    }

    //public class CommentWithPersianDateViewModel : ServiceComment
    //{
    //    public CommentWithPersianDateViewModel(ServiceComment comment)
    //    {
    //        this.Comment = comment;
    //        this.PersianDate = comment.AddedDate != null ? new PersianDateTime(comment.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
    //    }
    //    public ServiceComment Comment { get; set; }
    //    [Display(Name = "تاریخ ثبت")]
    //    public string PersianDate { get; set; }
    //}
    public class ServicesListViewModel
    {
        public ServicesListViewModel()
        {
        }

        public ServicesListViewModel(ServiceCategory serviceCategory)
        {
            this.Id = serviceCategory.Id;
            this.Title = serviceCategory.Title;
            if (serviceCategory.Services != null)
            {
                this.Services = serviceCategory.Services.ToList();
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Service> Services { get; set; }
    }


    public class LatestServicesViewModel
    {
        public LatestServicesViewModel()
        {
        }

        public LatestServicesViewModel(Service service)
        {
            this.Id = service.Id;
            this.Title = service.Title;
            this.Image = service.Image;
            this.ShortDescription = service.ShortDescription;
            this.Author = $"{service.User.FirstName} {service.User.LastName}";
            this.PersianDate = service.AddedDate != null ? new PersianDateTime(service.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
            if (service.ServiceCategory != null)
            {
                this.Category = service.ServiceCategory;
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Author { get; set; }
        public string PersianDate { get; set; }
        public ServiceCategory Category { get; set; }
    }

    public class ServiceDetailsViewModel
    {
        public ServiceDetailsViewModel()
        {

        }
        public ServiceDetailsViewModel(Service service)
        {
            this.Id = service.Id;
            this.CategoryId = service.ServiceCategoryId.Value;
            this.CategoryTitle = service.ServiceCategory.Title;
            this.Title = service.Title;
            this.Image = service.Image;
            this.ShortDescription = service.ShortDescription;
            this.Description = service.Description;
            this.Author = service.User != null ? $"{service.User.FirstName} {service.User.LastName}" : "-";
            this.AuthorImage = service.User != null ? service.User.Avatar : "user-avatar.png";
            this.AuthorInfo = service.User != null ? service.User.Information : "";
            this.PersianDate = service.AddedDate != null ? new PersianDateTime(service.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string AuthorImage { get; set; }
        public string AuthorInfo { get; set; }
        public string PersianDate { get; set; }
        public List<ServiceHeadLine> HeadLines { get; set; }
        //public List<ServiceTag> Tags { get; set; }
        //public List<ServiceCommentViewModel> ServiceComments { get; set; }
        public CommentFormViewModel CommentForm { get; set; }

        public Service Next { get; set; }
        public Service Previous { get; set; }
    }

    //public class ServiceCommentViewModel
    //{
    //    public ServiceCommentViewModel()
    //    {

    //    }

    //    public ServiceCommentViewModel(ServiceComment comment)
    //    {
    //        this.Id = comment.Id;
    //        this.ParentId = comment.ParentId;
    //        this.Name = comment.Name;
    //        this.Email = comment.Email;
    //        this.Message = comment.Message;
    //        this.AddedDate = comment.AddedDate != null ? new PersianDateTime(comment.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
    //    }
    //    public int Id { get; set; }
    //    public int? ParentId { get; set; }
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //    public string Message { get; set; }
    //    public string AddedDate { get; set; }
    //}

    public class ServiceCategoriesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ServiceCount { get; set; }
        //public virtual ICollection<ServiceCategory> Children { get; set; }
    }
}