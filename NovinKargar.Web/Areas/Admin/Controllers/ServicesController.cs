using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NovinKargar.Core.Models;
using NovinKargar.Infrastructure;
using NovinKargar.Infrastructure.Helpers;
using NovinKargar.Infrastructure.Repositories;
using NovinKargar.Web.ViewModels;

namespace NovinKargar.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly ServicesRepository _servicesRepo;
        private readonly ServiceCategoriesRepository _serviceCategoriesRepo;

        public ServicesController(ServicesRepository servicesRepo, ServiceCategoriesRepository serviceCategoriesRepo )
        {
            _servicesRepo = servicesRepo;
            this._serviceCategoriesRepo = serviceCategoriesRepo;
        }
        // GET: Admin/Services
        public ActionResult Index()
        {
            var services = _servicesRepo.GetServices();
            var servicesListVm = new List<ServiceInfoViewModel>();
            foreach (var service in services)
            {
                var serviceVm = new ServiceInfoViewModel(service);
                servicesListVm.Add(serviceVm);
            }
            return View(servicesListVm);
        }
        // GET: Admin/Services/Create
        public ActionResult Create()
        {
            ViewBag.ServiceCategoryId = new SelectList(_serviceCategoriesRepo.GetServiceCategories(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service, HttpPostedFileBase ServiceImage, string Tags)
        {
            if (ModelState.IsValid)
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    ViewBag.Message = "کاربر وارد کننده پیدا نشد.";
                    return View(service);
                }


                #region Upload Image
                if (ServiceImage != null)
                {
                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(ServiceImage.FileName);
                    ServiceImage.SaveAs(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName));

                    // Resize Image
                    ImageResizer image = new ImageResizer(1200, 1200,true);
                    image.Resize(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName),
                        Server.MapPath("/Files/ServiceImages/Image/" + newFileName));

                    ImageResizer thumb = new ImageResizer(300,300,true);
                    thumb.Resize(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName),
                        Server.MapPath("/Files/ServiceImages/Thumb/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName));

                    service.Image = newFileName;
                }
                #endregion

                _servicesRepo.AddService(service);

                //if (!string.IsNullOrEmpty(Tags))
                //    _servicesRepo.AddServiceTags(service.Id, Tags);

                return RedirectToAction("Index");
            }
            ViewBag.Tags = Tags;
            ViewBag.ServiceCategoryId = new SelectList(_serviceCategoriesRepo.GetServiceCategories(), "Id", "Title", service.ServiceCategoryId);
            return View(service);
        }

        // GET: Admin/Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = _servicesRepo.GetService(id.Value);
            if (service == null)
            {
                return HttpNotFound();
            }

            //ViewBag.Tags = _servicesRepo.GetServiceTagsStr(id.Value);

            ViewBag.ServiceCategoryId = new SelectList(_serviceCategoriesRepo.GetServiceCategories(), "Id", "Title", service.ServiceCategoryId);
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service, HttpPostedFileBase ServiceImage, string Tags)
        {
            if (ModelState.IsValid)
            {
                #region Upload Image
                if (ServiceImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Files/ServiceImages/Image/" + service.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/ServiceImages/Image/" + service.Image));

                    if (System.IO.File.Exists(Server.MapPath("/Files/ServiceImages/Thumb/" + service.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/ServiceImages/Thumb/" + service.Image));

                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(ServiceImage.FileName);
                    ServiceImage.SaveAs(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName));

                    // Resize Image
                    ImageResizer image = new ImageResizer(1200, 1200,true);
                    image.Resize(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName),
                        Server.MapPath("/Files/ServiceImages/Image/" + newFileName));

                    ImageResizer thumb = new ImageResizer(300, 300, true);
                    thumb.Resize(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName),
                        Server.MapPath("/Files/ServiceImages/Thumb/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/ServiceImages/Temp/" + newFileName));

                    service.Image = newFileName;
                }
                #endregion

                _servicesRepo.Update(service);

                //if (!string.IsNullOrEmpty(Tags))
                //    _servicesRepo.AddServiceTags(service.Id, Tags);

                return RedirectToAction("Index");
            }
            ViewBag.Tags = Tags;
            ViewBag.ServiceCategoryId = new SelectList(_serviceCategoriesRepo.GetServiceCategories(), "Id", "Title", service.ServiceCategoryId);
            return View(service);
        }

        [HttpPost]
        public ActionResult FileUpload()
        {
            var files = HttpContext.Request.Files;
            foreach (var fileName in files)
            {
                HttpPostedFileBase file = Request.Files[fileName.ToString()];
                var newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("/Files/ServiceImages/" + newFileName));
                TempData["UploadedFile"] = newFileName;
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        // GET: Admin/Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Service service = _servicesRepo.GetService(id.Value);

            if (service == null)
            {
                return HttpNotFound();
            }
            return PartialView(service);
        }

        // POST: Admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var service = _servicesRepo.Get(id);

            //#region Delete Service Image
            //if (service.Image != null)
            //{
            //    if (System.IO.File.Exists(Server.MapPath("/Files/ServiceImages/Image/" + service.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/ServiceImages/Image/" + service.Image));

            //    if (System.IO.File.Exists(Server.MapPath("/Files/ServiceImages/Thumb/" + service.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/ServiceImages/Thumb/" + service.Image));
            //}
            //#endregion

            _servicesRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
