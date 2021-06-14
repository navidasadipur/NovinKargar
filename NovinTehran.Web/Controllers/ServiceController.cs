using NovinTehran.Core.Models;
using NovinTehran.Core.Utility;
using NovinTehran.Infrastructure.Repositories;
using NovinTehran.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NovinTehran.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ServicesRepository _servicesRepo;
        private readonly ServiceCategoriesRepository _serviceCategoriesRepo;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        //private readonly ServiceTagsRepository _serviceTagsRepo;

        public ServiceController(
            ServicesRepository servicesRepo,
            ServiceCategoriesRepository serviceCategoriesRepo,
            StaticContentDetailsRepository staticContentDetailsRepo
            //ServiceTagsRepository serviceTagsRepo
            )
        {
            _servicesRepo = servicesRepo;
            _serviceCategoriesRepo = serviceCategoriesRepo;
            _staticContentRepo = staticContentDetailsRepo;
            _serviceCategoriesRepo = serviceCategoriesRepo;
            //_serviceTagsRepo = serviceTagsRepo;
        }
        //// GET: Service
        //public ActionResult Index(int pageNumber = 1, string searchString = null, int? category = null)
        //{
        //    var services = new List<Service>();
        //    var take = 3;
        //    var skip = pageNumber * take - take;
        //    var count = 0;
        //    if (category != null)
        //    {
        //        services = _servicesRepo.GetServicesList(skip, take, category.Value);
        //        count = _servicesRepo.GetServicesCount(category.Value);
        //        var cat = _serviceCategoriesRepo.Get(category.Value);
        //        ViewBag.CategoryId = category;
        //        ViewBag.Title = $"دسته {cat.Title}";
        //    }
        //    else if (!string.IsNullOrEmpty(searchString))
        //    {
        //        services = _servicesRepo.GetServicesList(skip, take, searchString);
        //        count = _servicesRepo.GetServicesCount(searchString);
        //        ViewBag.SearchString = searchString;
        //        ViewBag.Title = $"جستجو: {searchString}";
        //    }//}
        //    else
        //    {
        //        services = _servicesRepo.GetServicesList(skip, take);
        //        count = _servicesRepo.GetServicesCount();
        //        ViewBag.Title = "بلاگ";
        //    }

        //    var pageCount = (int) Math.Ceiling((double) count / take);
        //    ViewBag.PageCount = pageCount;
        //    ViewBag.CurrentPage = pageNumber;

        //    var vm = new List<LatestServicesViewModel>();
        //    foreach (var item in services)
        //        vm.Add(new LatestServicesViewModel(item));

        //    var banner = "";
        //    try
        //    {
        //        banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ وبلاگ").Image;
        //        banner = "/Files/StaticContentImages/Image/" + banner;
        //    }
        //    catch
        //    {

        //    }

        //    ViewBag.banner = banner;

        //    ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

        //    return View(vm);

        // GET: Service
        public ActionResult Index(int pageNumber = 1, string searchString = null, int? category = null)
        {
            var allCategoriesWithServices = _serviceCategoriesRepo.GetAllServiceCategoriesWithServices();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(allCategoriesWithServices);
        }

        //public ActionResult ServiceCategoriesSection()
        //{
        //    var categories = _serviceCategoriesRepo.GetMainServiceCategories();

        //    var serviceCategoriesVm = new List<ServiceCategoriesViewModel>();

        //    foreach (var item in categories)
        //    {
        //        var vm = new ServiceCategoriesViewModel();
        //        vm.Id = item.Id;
        //        vm.Title = item.Title;
        //        vm.ServiceCount = _servicesRepo.GetServicesCount(item.Id);
        //        vm.Children = item.Children;
        //        serviceCategoriesVm.Add(vm);
        //    }

        //    return PartialView(serviceCategoriesVm);
        //}

        public ActionResult TopServicesSection(int take)
        {
            var vm = new List<LatestServicesViewModel>();
            var services = _servicesRepo.GetTopServices(take);
            foreach (var item in services)
                vm.Add(new LatestServicesViewModel(item));

            return PartialView(vm);
        }

        //public ActionResult AdServiceSection()
        //{
        //    var model = _staticContentRepo.GetStaticContentDetail((int)StaticContents.ServiceAd);

        //    return PartialView(model);
        //}

        //public ActionResult ServiceDetailsTagsSection(int id)
        //{
        //    //SocialViewModel model = new SocialViewModel();

        //    //model.Instagram = _staticContentDetailsRepo.GetStaticContentDetail(1009).Link;
        //    //model.Aparat = _staticContentDetailsRepo.GetStaticContentDetail(1012).Link;

        //    var tags = _serviceTagsRepo.GetServiceTags(id);

        //    return PartialView(tags);
        //}

        //public ActionResult ServiceListTagsSection()
        //{
        //    //SocialViewModel model = new SocialViewModel();

        //    //model.Instagram = _staticContentDetailsRepo.GetStaticContentDetail(1009).Link;
        //    //model.Aparat = _staticContentDetailsRepo.GetStaticContentDetail(1012).Link;

        //    var tags = _serviceTagsRepo.GetAll();

        //    return PartialView(tags);
        //}

        [Route("Service/ServiceDetails/{id}/{title}")]
        [Route("Service/Service/{id}/{title}")]

        public ActionResult ServiceDetails(int id)
        {
            _servicesRepo.UpdateServiceViewCount(id);

            var service = _servicesRepo.GetService(id);

            var serviceDetailsVm = new ServiceDetailsViewModel(service);

            var serviceHeadlines = _servicesRepo.GetServiceHeadlines(id);
            serviceDetailsVm.HeadLines = serviceHeadlines;

            //get next service
            Service nextService = null;
            var nextId = id;

            var latestService = _servicesRepo.GetLatestServices(1).FirstOrDefault();

            while (nextService == null && nextId < latestService.Id)
            {
                nextId++;
                nextService = _servicesRepo.GetService(nextId);
            }

            serviceDetailsVm.Next = nextService;

            //get previous service
            Service previousService = null;
            var previousId = id;

            while (previousService == null && previousId > 1)
            {
                previousId--;
                previousService = _servicesRepo.GetService(previousId);
            }

            serviceDetailsVm.Previous = previousService;

            var banner = "";
            try
            {
                banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ وبلاگ").Image;
                banner = "/Files/StaticContentImages/Image/" + banner;
            }
            catch
            {

            }

            ViewBag.banner = banner;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(serviceDetailsVm);
        }

        //[HttpPost]
        //public ActionResult PostComment(CommentFormViewModel form)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var comment = new ServiceComment()
        //        {
        //            ServiceId = form.ServiceId.Value,
        //            ParentId = form.ParentId,
        //            Name = form.Name,
        //            Email = form.Email,
        //            Message = form.Message,
        //            AddedDate = DateTime.Now,
        //        };
        //        _servicesRepo.AddComment(comment);
        //        return RedirectToAction("ContactUsSummary", "Home");
        //    }
        //    return RedirectToAction("ServiceDetails", new { id = form.ServiceId });
        //}

        public ActionResult ShareSocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram);
            model.Linkedin = _staticContentRepo.GetStaticContentDetail((int)StaticContents.linkedin);

            return PartialView(model);
        }

        public ActionResult SocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram);
            model.Linkedin = _staticContentRepo.GetStaticContentDetail((int)StaticContents.linkedin);

            return PartialView(model);
        }

        public ActionResult RelatedServicesSection(int? categoryId, int take)
        {
            var relatedServices = new List<LatestServicesViewModel>();

            var takedServices = new List<Service>();

            if (categoryId != null)
            {
                var services = _servicesRepo.GetServicesByCategoryId(categoryId.Value).OrderByDescending(b => b.InsertDate).ToList();

                if (services.Count() < take)
                {
                    takedServices = services;
                }
                else
                {
                    takedServices = services.GetRange(0, take);
                }

            }

            foreach (var blog in takedServices)
            {
                var vm = new LatestServicesViewModel(blog);

                relatedServices.Add(vm);
            }

            return PartialView(relatedServices);
        }
    }
}