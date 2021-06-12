using NovinTehran.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovinTehran.Infrastructure.Repositories
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
    }
}
