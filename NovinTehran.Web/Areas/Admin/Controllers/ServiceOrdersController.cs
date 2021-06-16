using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NovinTehran.Core.Models;
using NovinTehran.Core.Utility;
using NovinTehran.Infrastructure.Repositories;
using NovinTehran.Web.ViewModels;

namespace NovinTehran.Web.Areas.Admin.Controllers
{
    public class ServiceOrdersController : Controller
    {
        private readonly ServiceOrdersRepository _serviceOrdersRepo;
        private readonly ServicesRepository _servicesRepo;
        private readonly GeoDivisionsRepository _GeoRepo;

        public ServiceOrdersController(ServiceOrdersRepository serviceOrdersRepo, ServicesRepository servicesRepo, GeoDivisionsRepository geoRepo)
        {
            _serviceOrdersRepo = serviceOrdersRepo;
            this._servicesRepo = servicesRepo;
            _GeoRepo = geoRepo;
        }

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var serviceOrders = _serviceOrdersRepo.GetServiceOrders();
            var vm = new List<ServiceOrderTableViewModel>();
            foreach (var serviceOrder in serviceOrders)
            {
               vm.Add(new ServiceOrderTableViewModel(serviceOrder)); 
            }
            return View(vm);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrder serviceOrder = _serviceOrdersRepo.GetServiceOrder(id.Value);
            if (serviceOrder == null)
            {
                return HttpNotFound();
            }
            //ViewBag.GeoDivisionId = new SelectList(_GeoRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", serviceOrder.GeoDivisionId);
            ViewBag.ServiceId = new SelectList(_servicesRepo.GetServices(), "Id", "Title", serviceOrder.ServiceId);
            return View(serviceOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceOrder serviceOrder)
        {
            if (ModelState.IsValid)
            {

                _serviceOrdersRepo.Update(serviceOrder);
                return RedirectToAction("Index");
            }
            //ViewBag.GeoDivisionId = new SelectList(_GeoRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", serviceOrder.GeoDivisionId);
            return View(serviceOrder);
        }

        public ActionResult ViewServiceOrder(int serviceOrderId)
        {
            var vm = new ViewServiceOrderViewModel();
            var serviceOrder = _serviceOrdersRepo.GetServiceOrder(serviceOrderId);
            vm.ServiceOrder = serviceOrder;
            vm.PersianDate = new PersianDateTime(serviceOrder.AddedDate).ToString();

            //vm.OrderItems = new List<OrderItemWithMainFeatureViewModel>();

            // Getting Order Item SubFeatures
            //foreach (var serviceOrderItem in serviceOrder.OrderItems)
            //{
            //    var serviceOrderItemWithMainFeature = new OrderItemWithMainFeatureViewModel
            //    {
            //        OrderItem = serviceOrderItem, MainFeature = _serviceOrderRepo.GetOrderItemsMainFeature(serviceOrderItem.Id)
            //    };
            //    vm.OrderItems.Add(serviceOrderItemWithMainFeature);

            //}

            return View(vm);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrder serviceOrder = _serviceOrdersRepo.GetServiceOrder(id.Value);
            if (serviceOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersianDate = new PersianDateTime(serviceOrder.AddedDate).ToString();
            return PartialView(serviceOrder);
        }

        // POST: Admin/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var serviceOrder = _serviceOrdersRepo.Get(id);
            _serviceOrdersRepo.Delete(id);
            return RedirectToAction("Index");
        }

    }
}