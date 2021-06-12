using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NovinTehran.Infrastructure.Repositories;
using NovinTehran.Core.Models;
using System.Net;

namespace NovinTehran.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ServiceHeadLinesController : Controller
    {
        private readonly ServiceHeadLinesRepository _repo;
        public ServiceHeadLinesController(ServiceHeadLinesRepository repo)
        {
            _repo = repo;
        }
        public ActionResult Index(int serviceId)
        {
            ViewBag.ServiceName = _repo.GetServiceName(serviceId);
            ViewBag.ServiceId = serviceId;
            return View(_repo.GetServiceHeadLines(serviceId));
        }

        public ActionResult Create(int serviceId)
        {
            ViewBag.ServiceId = serviceId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceHeadLine headLine)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(headLine);
                return RedirectToAction("Index",new { serviceId = headLine.ServiceId });
            }
            ViewBag.ServiceId = headLine.ServiceId;
            return View(headLine);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceHeadLine headLine = _repo.Get(id.Value);
            if (headLine == null)
            {
                return HttpNotFound();
            }
            return View(headLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceHeadLine headLine)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(headLine);
                return RedirectToAction("Index", new { serviceId = headLine.ServiceId});
            }
            return View(headLine);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceHeadLine headLine = _repo.Get(id.Value);
            if (headLine == null)
            {
                return HttpNotFound();
            }
            return PartialView(headLine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var serviceId = _repo.Get(id).ServiceId;
            _repo.Delete(id);
            return RedirectToAction("Index",new { serviceId });
        }
    }
}