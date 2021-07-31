using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using NovinKargar.Core.Models;
using NovinKargar.Infrastructure.Repositories;

namespace NovinKargar.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ServiceCategoriesController : Controller
    {
        private readonly ServiceCategoriesRepository _repo;
        public ServiceCategoriesController(ServiceCategoriesRepository repo)
        {
            _repo = repo;
        }
        // GET: Admin/ServiceCategories
        public ActionResult Index(int? parentId)
        {
            //return View(_repo.GetAll());

            List<ServiceCategory> productGroups;

            if (parentId == null)
                productGroups = _repo.GetServiceCategoryTable();
            else
            {
                productGroups = _repo.GetServiceCategoryTable(parentId.Value);
                //var parent = _repo.Get(parentId.Value);
                //ViewBag.PrevParent = parent.ParentId;
                //ViewBag.ParentId = parentId;
                //ViewBag.ParentName = parent.Title;
            }


            return View(productGroups);
        }

        // GET: Admin/ServiceCategories/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(_repo.GetServiceCategories(), "Id", "Title");
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title, ParentId")] ServiceCategory serviceCategory)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(serviceCategory);
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(_repo.GetServiceCategories(), "Id", "Title", serviceCategory.Id);

            return View(serviceCategory);
        }

        // GET: Admin/ServiceCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceCategory serviceCategory = _repo.Get(id.Value);
            if (serviceCategory == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentId = new SelectList(_repo.GetServiceCategories(), "Id", "Title", serviceCategory.Id);

            return PartialView(serviceCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title, ParentId")] ServiceCategory serviceCategory)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(serviceCategory);
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(_repo.GetServiceCategories(), "Id", "Title", serviceCategory.Id);

            return View(serviceCategory);
        }

        // GET: Admin/ServiceCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceCategory serviceCategory = _repo.Get(id.Value);
            if (serviceCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(serviceCategory);
        }

        // POST: Admin/ServiceCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
