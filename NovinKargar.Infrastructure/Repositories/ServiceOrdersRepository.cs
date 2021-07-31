using NovinKargar.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovinKargar.Infrastructure.Repositories
{
    public class ServiceOrdersRepository : BaseRepository<ServiceOrder, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ServiceOrdersRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<ServiceOrder> GetServiceOrders(int serviceId)
        {
            return _context.ServiceOrders.Where(h => h.ServiceId == serviceId & h.IsDeleted == false).ToList();
        }

        public string GetServiceName(int serviceId)
        {
            return _context.Services.Find(serviceId).Title;
        }

        public List<ServiceOrder> GetServiceOrders()
        {
            return _context.ServiceOrders.Where(o => o.IsDeleted == false).Include(o => o.Customer).Include(o => o.Service).OrderByDescending(o => o.Id).ToList();
        }

        public ServiceOrder GetServiceOrder(int serviceOrderId)
        {
            return _context.ServiceOrders.Include(o => o.Customer.User).Include(o => o.Service).FirstOrDefault(o => o.Id == serviceOrderId && o.IsDeleted == false);
        }

        //public ServiceOrder GetServiceOrder(string serviceOrderNumber)
        //{
        //    return _context.ServiceOrders.Include(o => o.Customer.User).Include(o => o.Service).Include(o => o.Customer).FirstOrDefault(o => o.ServiceOrderNumber == serviceOrderNumber);
        //}

        //public ServiceOrder GetServiceOrder(string serviceOrderNumber, int customerId)
        //{
        //    return _context.ServiceOrders.Include(o => o.Customer.User).Include(o => o.Service).Include(o => o.DiscountCode).FirstOrDefault(o => o.ServiceOrderNumber == serviceOrderNumber && i.CustomerId == customerId);
        //}

        //public ServiceOrder GetServiceOrderByPaymentId(int paymentId)
        //{
        //    var payment = _context.EPayments.FirstOrDefault(p => p.Id == paymentId);
        //    return _context.ServiceOrders.Include(o => o.Customer.User).Include(o => o.Service).Include(o => o.DiscountCode).FirstOrDefault(o => o.Id == payment.ServiceOrderId);
        //}

        public ServiceOrder GetLatestServiceOrder(int customerId)
        {
            ServiceOrder serviceOrder = null;
            try
            {
                serviceOrder = _context.ServiceOrders.Include(o => o.Customer.User).Include(o => o.Service).Include(o => o.Customer).OrderByDescending(o => o.AddedDate).Where(o => o.CustomerId == customerId).ToList()[0];
            }
            catch
            {

            }
            return serviceOrder;
        }

        //public string GetServiceOrderbyServiceId(int serviceId)
        //{
        //    var serviceOrder = _context.ServiceOrder.Find(serviceOrderItemId);
        //    var mainFeature = _context.ProductMainFeatures.Include(m => m.SubFeature).FirstOrDefault(m => m.Id == serviceOrderItem.MainFeatureId);
        //    return mainFeature.SubFeature.Value;
        //}

        //public List<Product> GetTopSoldProducts(int take)
        //{
        //    List<Product> products = new List<Product>();
        //    var productIds = _context.ServiceOrderItems.GroupBy(o => o.ProductId)
        //        .OrderByDescending(pi => pi.Count())
        //        .Select(g => g.Key).ToList();
        //    foreach (var id in productIds)
        //    {
        //        if (products.Count < take)
        //        {
        //            var product = _context.Products.FirstOrDefault(p => p.Id == id);
        //            if (product != null && product.IsDeleted == false)
        //            {
        //                products.Add(product);
        //            }
        //        }
        //    }

        //    return products;
        //}

        public List<ServiceOrder> GetCustomerServiceOrders(int customerId)
        {
            return _context.ServiceOrders.Where(o => o.IsDeleted == false && o.CustomerId == customerId).ToList();
        }
    }
}
