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
        private readonly CustomersRepository _customerRepo;
        private readonly ServiceOrdersRepository _serviceOrdersRepo;

        //private readonly ServiceTagsRepository _serviceTagsRepo;

        public ServiceController(
            ServicesRepository servicesRepo,
            ServiceCategoriesRepository serviceCategoriesRepo,
            StaticContentDetailsRepository staticContentDetailsRepo
            , CustomersRepository customerRepo
            , ServiceOrdersRepository serviceOrdersRepo
            //ServiceTagsRepository serviceTagsRepo
            )
        {
            _servicesRepo = servicesRepo;
            _serviceCategoriesRepo = serviceCategoriesRepo;
            _staticContentRepo = staticContentDetailsRepo;
            this._customerRepo = customerRepo;
            this._serviceOrdersRepo = serviceOrdersRepo;
            _serviceCategoriesRepo = serviceCategoriesRepo;
            //_serviceTagsRepo = serviceTagsRepo;
        }

        // GET: Service
        public ActionResult Index(int pageNumber = 1, string searchString = null, int? category = null)
        {
            var allCategoriesWithServices = _serviceCategoriesRepo.GetAllServiceCategoriesWithServices();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(allCategoriesWithServices);
        }

        public ActionResult TopServicesSection(int take)
        {
            var vm = new List<LatestServicesViewModel>();
            var services = _servicesRepo.GetTopServices(take);
            foreach (var item in services)
                vm.Add(new LatestServicesViewModel(item));

            return PartialView(vm);
        }

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

        public ActionResult ServiceOrder(int id)
        {
            var service = _servicesRepo.GetService(id);

            ViewBag.ServiceTitle = service.Title;
            ViewBag.ServiceId = service.Id;

            return View();
        }

        [HttpPost]
        public ActionResult ServiceOrder(ServiceOrder model)
        {
            var customer = _customerRepo.GetCurrentCustomer();

            if (customer != null)
            {
                //serviceOrder.CustomerFirstName = customer.User.FirstName;
                //serviceOrder.CustomerLastName = customer.User.LastName;
                //serviceOrder.Email = customer.User.Email;
                //serviceOrder.Phone = customer.User.PhoneNumber;
                //serviceOrder.PostalCode = customer.PostalCode;
                //serviceOrder.Address = customer.Address;
                model.Customer = customer;
            }

            //ViewBag.ServiceTitle = _servicesRepo.GetService(id).Title;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddServiceOrder(ServiceOrder serviceOrder)
        {
            var customer = _customerRepo.GetCurrentCustomer();

            if (customer != null)
            {
                serviceOrder.CustomerId = customer.Id;
            }

            // updating info
            serviceOrder.AddedDate = DateTime.Now;

            try
            {
                _serviceOrdersRepo.Add(serviceOrder);

                ViewBag.Added = true;

                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "ثبت سفارش ناموفق";
                ViewBag.Added = false;

                return View();
            }
        }
    }
}