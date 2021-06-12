using NovinTehran.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovinTehran.Infrastructure.Repositories
{
    public class ServiceHeadLinesRepository : BaseRepository<ServiceHeadLine, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ServiceHeadLinesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<ServiceHeadLine> GetServiceHeadLines(int serviceId)
        {
            return _context.ServiceHeadLines.Where(h => h.ServiceId == serviceId & h.IsDeleted == false).ToList();
        }
        public string GetServiceName(int serviceId)
        {
            return _context.Services.Find(serviceId).Title;
        }
    }
}
