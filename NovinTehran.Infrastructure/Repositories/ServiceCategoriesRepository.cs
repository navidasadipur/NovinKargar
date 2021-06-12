using NovinTehran.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovinTehran.Infrastructure.Repositories
{
    public class ServiceCategoriesRepository : BaseRepository<ServiceCategory, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ServiceCategoriesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<ServiceCategory> GetServiceCategoryTable()
        {
            var allCategories = _context.ServiceCategories.Where(ac => ac.IsDeleted == false).ToList();

            return allCategories;
        }

        public List<ServiceCategory> GetServiceCategoryTable(int id)
        {
            var allCategories = _context.ServiceCategories.Where(ac => ac.IsDeleted == false).ToList();

            return allCategories;
        }

        public ServiceCategory GetServiceCategory(int id)
        {
            var pg = _context.ServiceCategories
                .FirstOrDefault(ac => ac.Id == id);
            return pg;
        }

        public List<Feature> GetFeatures()
        {
            return _context.Features.Where(f => f.IsDeleted == false).ToList();
        }

        public List<ServiceCategory> GetServiceCategories()
        {
            return _context.ServiceCategories.Where(f => f.IsDeleted == false).OrderByDescending(p => p.InsertDate).ToList();
        }

        public ServiceCategory AddNewServiceCategory(int parentId, string title)
        {
            var ServiceCategory = new ServiceCategory();

            var user = GetCurrentUser();
            ServiceCategory.InsertDate = DateTime.Now;
            ServiceCategory.InsertUser = user.UserName;

            #region Adding Service Group
            ServiceCategory.Title = title;

            //if (parentId != 0)
            //    ServiceCategory.ParentId = parentId;


            _context.ServiceCategories.Add(ServiceCategory);
            _context.SaveChanges();
            _logger.LogEvent(ServiceCategory.GetType().Name, ServiceCategory.Id, "Add");
            #endregion

            _context.SaveChanges();

            return ServiceCategory;
        }

        public ServiceCategory UpdateServiceCategory(int parentId, int ServiceCategoryId, string title)
        {
            var ServiceCategory = Get(ServiceCategoryId);
            var user = GetCurrentUser();
            ServiceCategory.UpdateDate = DateTime.Now;
            ServiceCategory.UpdateUser = user.UserName;

            #region Adding Service Group
            ServiceCategory.Title = title;

            //if (parentId != 0)
            //    ServiceCategory.ParentId = parentId;
            //else
            //    ServiceCategory.ParentId = null;

            Update(ServiceCategory);
            _logger.LogEvent(ServiceCategory.GetType().Name, ServiceCategory.Id, "Update");
            #endregion

            return ServiceCategory;
        }

        //public List<ServiceCategory> GetChildrenServiceCategories(int? parentId = null)
        //{
        //    var groups = new List<ServiceCategory>();
        //    if (parentId == null)
        //        groups = _context.ServiceCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();
        //    else
        //        groups = _context.ServiceCategories.Where(p => p.IsDeleted == false && p.ParentId == parentId).Include(p => p.Children).ToList();
        //    return groups;
        //}

        //public List<ServiceCategory> GetMainServiceCategories()
        //{
        //    var groups = new List<ServiceCategory>();

        //    groups = _context.ServiceCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();

        //    return groups;
        //}

        public ServiceCategory GetCategoryByServiceId(int serviceId)
        {
            var serviceCategoryId = _context.ServiceCategories.Where(ac => ac.IsDeleted == false && ac.Id == serviceId).Select(ac => ac.Id).FirstOrDefault();

            return GetServiceCategory(serviceCategoryId);
        }
    }
}
