using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NovinKargar.Core.Models;
using NovinKargar.Core.Utility;
using NovinKargar.Infrastructure.Repositories;
using NovinKargar.Infratructure.Repositories;
using NovinKargar.Infratructure.Services;
using NovinKargar.Web.ViewModels;

namespace NovinKargar.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DiscountsRepository _discountRepo;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        private readonly OffersRepository _offersRepo;
        private readonly ProductService _productService;
        private readonly TestimonialsRepository _testimonialRepo;
        private readonly PartnersRepository _partnersRepo;
        private readonly ArticlesRepository _articlesRepo;
        private readonly ContactFormsRepository _contactFormRepo;
        private readonly ProductGroupsRepository _productGroupRepo;
        private readonly OurTeamRepository _ourTeamRepo;
        private readonly FaqGroupsRepository _faqGroupsRepo;
        private readonly EmailSubscriptionRepository _emailSubscriptionRepo;
        private readonly CertificatesRepository _certificatesRepo;
        private readonly ServiceCategoriesRepository _serviceCategoriesRepo;
        private readonly ProductsRepository _productsRepo;

        public HomeController(
            StaticContentDetailsRepository staticContentRepo,
            OffersRepository offersRepo,
            ProductsRepository productsRepo,
            ProductService productService,
            TestimonialsRepository testimonialRepo,
            PartnersRepository partnersRepo,
            ArticlesRepository articlesRepo,
            DiscountsRepository discountsRepo,
            EmailSubscriptionRepository emailSubscriptionRepo,
            ContactFormsRepository contactFormRepo,
            ProductGroupsRepository productGroupRepo,
            OurTeamRepository ourTeamRepo,
            FaqGroupsRepository faqGroupsRepo,
            CertificatesRepository certificatesRepository
            , ServiceCategoriesRepository serviceCategoriesRepo
            )
        {
            _discountRepo = discountsRepo;
            _staticContentRepo = staticContentRepo;
            _offersRepo = offersRepo;
            _productService = productService;
            _testimonialRepo = testimonialRepo;
            _partnersRepo = partnersRepo;
            _articlesRepo = articlesRepo;
            _contactFormRepo = contactFormRepo;
            _productGroupRepo = productGroupRepo;
            _ourTeamRepo = ourTeamRepo;
            _faqGroupsRepo = faqGroupsRepo;
            _emailSubscriptionRepo = emailSubscriptionRepo;
            _certificatesRepo = certificatesRepository;
            _serviceCategoriesRepo = serviceCategoriesRepo;
            _productsRepo = productsRepo;
        }

        public ActionResult Index()
        {

            var popup = _staticContentRepo.GetSingleContentByTypeId((int)StaticContentTypes.Popup);

            //ViewBag.BackImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HeaderImage).Image;

            //ViewBag.NewsBackImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.NewsBackImage).Image;
            //ViewBag.NewsTitle = _staticContentRepo.GetStaticContentDetail((int)StaticContents.NewsBackImage).Title;
            //ViewBag.NewsShorDescription = _staticContentRepo.GetStaticContentDetail((int)StaticContents.NewsBackImage).ShortDescription;

            return View(popup);
        }

        public ActionResult HeaderSection()
        {

            var allMainGroups = _productGroupRepo.GetMainProductGroups();

            foreach (var group in allMainGroups)
            {
                group.Children = _productGroupRepo.GetChildrenProductGroups(group.Id);
            }

            ViewBag.LogoImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo).Image;
            ViewBag.HeaderImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HeaderImage).Image;

            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            if (wishListModel.WishListItems != null)
            {
                ViewBag.WishListCount = wishListModel.WishListItems.Count();
            }

            return PartialView(allMainGroups);
        }

        public ActionResult MobileHeaderSection()
        {

            var allMainGroups = _productGroupRepo.GetMainProductGroups();

            foreach (var group in allMainGroups)
            {
                group.Children = _productGroupRepo.GetChildrenProductGroups(group.Id);
            }

            ViewBag.LogoImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo).Image;

            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            if (wishListModel.WishListItems != null)
            {
                ViewBag.WishListCount = wishListModel.WishListItems.Count();
            }

            return PartialView(allMainGroups);
        }

        public ActionResult FooterSection()
        {
            var vm = new FooterViewModel()
            {
                Phone = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone),
                Email = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Email),
                Address = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Address),
                Logo = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo),
                Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook),
                Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter),
                Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram),
                Youtube = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Youtube),
                Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest),
                LinkedIn = _staticContentRepo.GetStaticContentDetail((int)StaticContents.linkedin)
            };

            ViewBag.EmailSubscription = _staticContentRepo.GetStaticContentDetail((int)StaticContents.EmailSubscription);

            return PartialView(vm);
        }

        public ActionResult CartSection()
        {
            var cartModel = new CartModel();

            HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

            if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
            {
                string cartJsonStr = cartCookie.Values["cart"];
                cartModel = new CartModel(cartJsonStr);
            }
            return PartialView(cartModel);
        }

        public ActionResult WishListSection()
        {
            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            return PartialView(wishListModel);
        }

        public ActionResult HomeTopSliderSection()
        {
            var content = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.HomeTopSlider);
            return PartialView(content);
        }

        public ActionResult HomeAboutSection()
        {
            var homeAbout = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeAboutDescription);

            ViewBag.Title = homeAbout.Title;

            ViewBag.ShortDescription = homeAbout.ShortDescription;

            ViewBag.Description = homeAbout.Description;

            var content = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.HomeAboutSection);
            return PartialView(content);
        }

        // GET: Service
        public ActionResult ServiceSection()
        {
            var allCategoriesWithServices = _serviceCategoriesRepo.GetAllServiceCategoriesWithServices();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return PartialView(allCategoriesWithServices);
        }

        public ActionResult OffersSection()
        {
            var offers = _offersRepo.GetAll();
            offers = offers.OrderBy(o => o.Id).ToList();
            return PartialView(offers);
        }

        public ActionResult TopSoldProductsSection(int take)
        {
            var products = _productService.GetTopSoldProductsWithPrice(take);
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                var tempVm = new ProductWithPriceViewModel(product);

                var group = _productGroupRepo.GetGroupByProductId(product.Id);

                tempVm.ProductGroupId = group.Id;

                tempVm.ProductGroupTitle = group.Title;

                vm.Add(tempVm);
            }

            return PartialView(vm);
        }

        public ActionResult TestimonialsSection()
        {
            var testimonials = _testimonialRepo.GetAll();
            var vm = testimonials.Select(testimonial => new TestimonialViewModel(testimonial)).ToList();

            return PartialView(vm);
        }
        public ActionResult LatestProductsSection(int take)
        {
            var products = _productService.GetLatestProductsWithPrice(take);
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                var tempVm = new ProductWithPriceViewModel(product);

                var group = _productGroupRepo.GetGroupByProductId(product.Id);

                tempVm.ProductGroupId = group.Id;

                tempVm.ProductGroupTitle = group.Title;

                vm.Add(tempVm);
            }


            return PartialView(vm);
        }

        public ActionResult HighRateProductsSection(int take)
        {
            var products = _productService.GetHighRatedProductsWithPrice(take);
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
                vm.Add(new ProductWithPriceViewModel(product));

            return PartialView(vm);
        }


        public ActionResult LatestArticlesSection(int take)
        {
            //var articles = _articlesRepo.GetLatestArticles(3);
            //var vm = articles.Select(item => new LatestArticlesViewModel(item)).ToList();

            //return PartialView(vm);

            var vm = new List<LatestArticlesViewModel>();
            var articles = _articlesRepo.GetLatestArticles(take);
            foreach (var item in articles)
                vm.Add(new LatestArticlesViewModel(item));

            var latestArticleTitle = _staticContentRepo.GetStaticContentDetail((int)StaticContents.LatestArticleSection);

            ViewBag.Title = latestArticleTitle.Title;

            ViewBag.ShortDescription = latestArticleTitle.ShortDescription;

            ViewBag.Description = latestArticleTitle.Description;

            return PartialView(vm);
        }


        public ActionResult DiscountSection()
        {
            var discountItems = _discountRepo.GetProductsWithDiscount();
            var products = new List<DiscountProductViewModel>();
            foreach (var item in discountItems)
            {
                var product = new DiscountProductViewModel();
                product.Price = _productService.GetProductPrice(item.Product);
                product.PriceAfterDiscount = _productService.GetProductPriceAfterDiscount(item.Product);
                product.ProductId = item.ProductId.Value;
                product.Image = item.Product.Image;
                product.DiscountType = item.DiscountType;
                product.Amount = item.Amount;
                product.Title = item.Title;
                product.ShortTitle = item.Product.ShortTitle;
                product.DeadLine = item.DeadLine;

                products.Add(product);
            }

            return PartialView(products);
        }

        [Route("Faq")]
        public ActionResult Faq()
        {
            var model = _faqGroupsRepo.GetAllFaqGroupsWithFaqs();

            ViewBag.BanerTitle = _staticContentRepo.GetStaticContentDetail(3).Title;
            ViewBag.ShortDescription = _staticContentRepo.GetStaticContentDetail(3).ShortDescription;
            ViewBag.Link = _staticContentRepo.GetStaticContentDetail(3).Link;
            ViewBag.Image = _staticContentRepo.GetStaticContentDetail(3).Image;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(model);
        }

        [Route("About")]
        public ActionResult About()
        {
            //var about = _staticContentRepo.GetStaticContentDetail((int)StaticContents.AboutDescription);

            //var model = new AboutViewModel()
            //{
            //    Title = about.Title,
            //    AboutDescription = about.ShortDescription,
            //    SignatureImage = about.Image,
            //};

            var model = new AboutViewModel();

            model.Image = _staticContentRepo.GetStaticContentDetail((int)StaticContents.firstImageAboutPage).Image;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(model);
        }

        public ActionResult AboutOurPropertiesSection()
        {
            var model = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.AboutProperties);

            return PartialView(model);
        }


        public ActionResult OurTeamsSection()
        {
            var ourTeam = _ourTeamRepo.GetAll();

            return PartialView(ourTeam);
        }

        public ActionResult PartnersSection()
        {
            var partners = _partnersRepo.GetAll();
            return PartialView(partners);
        }

        [Route("ContactUs")]
        public ActionResult Contact()
        {
            var map = _staticContentRepo.GetStaticContentDetail((int)StaticContents.ContactUsMap);
            var phone = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone);
            var email = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Email);
            var address = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Address);
            var vm = new ContactUsViewModel()
            {
                Map = map,
                Phone = phone,
                Email = email,
                Address = address
            };

            //var banner = "";
            //try
            //{
            //    banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ ارتباط با ما").Image;
            //    banner = "/Files/StaticContentImages/Image/" + banner;
            //}
            //catch
            //{

            //}

            //ViewBag.banner = banner;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(vm);
        }

        public ActionResult ContactSocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest);
            model.Youtube = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Youtube);
            model.Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram);

            return PartialView(model);
        }

        public ActionResult ContactUsForm()
        {
            var model = new ContactForm();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ContactUsForm(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                _contactFormRepo.Add(contactForm);
                return RedirectToAction("ContactUsSummary");
            }
            return RedirectToAction("Contact");
        }

        public ActionResult ContactUsSummary()
        {
            return View();
        }

        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string vImagePath = String.Empty;
            string vMessage = String.Empty;
            string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var vFileName = DateTime.Now.ToString("yyyyMMdd-HHMMssff") +
                                    Path.GetExtension(upload.FileName).ToLower();
                    var vFolderPath = Server.MapPath("/Upload/");
                    if (!Directory.Exists(vFolderPath))
                    {
                        Directory.CreateDirectory(vFolderPath);
                    }
                    vFilePath = Path.Combine(vFolderPath, vFileName);
                    upload.SaveAs(vFilePath);
                    vImagePath = Url.Content("/Upload/" + vFileName);
                    vMessage = "Image was saved correctly";
                }
            }
            catch
            {
                vMessage = "There was an issue uploading";
            }
            vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + vImagePath + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput);
        }

        [Route("NotFound")]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmailSubscription(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("Name", "Name is required");
            }
            if (!string.IsNullOrEmpty(Email))
            {
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is required");
            }
            if (ModelState.IsValid)
            {
                EmailSubscription emailSubscription = new EmailSubscription();
                emailSubscription.Email = Email;
                emailSubscription.IsDeleted = false;
                emailSubscription.InsertDate = DateTime.Now;

                _emailSubscriptionRepo.Create(emailSubscription);

            }

            //var email = "";
            //var isValid = true;
            //try
            //{
            //    //email = collection["Email"];

            //    email = Email;
            //}
            //catch
            //{

            //}

            //try
            //{
            //    var addr = new System.Net.Mail.MailAddress(email);

            //    isValid = (addr.Address == email) && ();
            //}
            //catch
            //{
            //    isValid = false;
            //}

            //if(isValid)
            //{
            //    EmailSubscription emailSubscription = new EmailSubscription();
            //    emailSubscription.Email = email;
            //    emailSubscription.IsDeleted = false;
            //    emailSubscription.InsertDate = DateTime.Now;

            //    _emailSubscriptionRepo.Create(emailSubscription);
            //}

            ViewBag.Added = ModelState.IsValid;

            return View();
        }

        [Route("Certificates")]
        public ActionResult Certificates()
        {
            var certificates = _certificatesRepo.GetAll();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(certificates);
        }

        [Route("Gallery")]
        public ActionResult Gallery()
        {
            var model = _productsRepo.GetAllProductsWithGalleries();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(model);
        }

        public ActionResult AboutRegisterSteps()
        {
            var model = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.AboutRegisterSteps);

            return PartialView(model);
        }
    }
}