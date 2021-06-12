using NovinTehran.Core.Models;
using NovinTehran.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace NovinTehran.Infrastructure.Repositories
{
    public class ServicesRepository : BaseRepository<Service, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ServicesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public Service GetService(int id)
        {
            return _context.Services.Include(a=>a.User).Include(a=>a.ServiceCategory).Include(a=>a.ServiceHeadLines).FirstOrDefault(a=>a.Id == id);
        }
        public List<Service> GetServices()
        {
            return _context.Services.Where(a=>a.IsDeleted == false).Include(a => a.User).Include(a=>a.ServiceCategory).OrderBy(a=>a.InsertDate).ToList();
        }

        public void AddService(Service service)
        {
            var user = GetCurrentUser();
            service.InsertDate = DateTime.Now;
            service.InsertUser = user.UserName;
            service.AddedDate = DateTime.Now;
            service.UserId = user.Id;
            _context.Services.Add(service);
            _context.SaveChanges();
            _logger.LogEvent(service.GetType().Name, service.Id, "Add");
        }

        //public string GetServiceTagsStr(int serviceId)
        //{
        //    var serviceTags = _context.ServiceTags.Where(t => t.ServiceId == serviceId && t.IsDeleted == false).Select(t=>t.Title).ToList();
        //    var tagsStr = string.Join("-", serviceTags.ToList());
        //    return tagsStr;
        //}

        //public void AddServiceTags(int serviceId, string serviceTags)
        //{
        //    if (string.IsNullOrEmpty(serviceTags))
        //        return;
        //    var oldTags = _context.ServiceTags.Where(t => t.ServiceId == serviceId).ToList();
        //    foreach (var tag in oldTags)
        //    {
        //        _context.ServiceTags.Remove(tag);
        //        _context.SaveChanges();
        //    }

        //    string[] tagsArr = serviceTags.Trim().Split('-');
        //    foreach (var tag in tagsArr)
        //    {
        //        var tagObj = new ServiceTag();
        //        tagObj.ServiceId = serviceId;
        //        tagObj.Title = tag.Trim();

        //        _context.ServiceTags.Add(tagObj);
        //        _context.SaveChanges();
        //    }
        //}

        public void AddServiceHeadLine(ServiceHeadLine headLine)
        {
            var user = GetCurrentUser();
            headLine.InsertDate = DateTime.Now;
            headLine.InsertUser = user.UserName;
            _context.ServiceHeadLines.Add(headLine);
            _context.SaveChanges();
            _logger.LogEvent(headLine.GetType().Name, headLine.Id, "Add");
        }

        public List<Service> GetLatestServices(int take)
        {
            return _context.Services.Where(a => a.IsDeleted == false).Include(a => a.User).OrderByDescending(a => a.Id).Take(take).ToList();
        }
        public List<Service> GetTopServices(int take)
        {
            return _context.Services.Where(a => a.IsDeleted == false).Include(a=>a.User)
                .OrderByDescending(a => a.ViewCount).Take(take).ToList();
        }
        #region Get Services List
        public List<Service> GetServicesList(int skip, int take)
        {
            return _context.Services.Where(a => a.IsDeleted == false).Include(a=>a.User).OrderByDescending(a=>a.AddedDate).Skip(skip).Take(take).ToList();
        }
        public List<Service> GetServicesList(int skip, int take, int categoryId)
        {
            return _context.Services.Where(a => a.IsDeleted == false && a.ServiceCategoryId == categoryId).Include(a => a.User).OrderByDescending(a => a.AddedDate).Skip(skip).Take(take).ToList();
        }

        public List<Service> GetServicesList(int skip, int take, string searchString)
        {
            var searchedServices = new List<Service>();

            var trimedSearchString = searchString.Trim().ToLower();

            var services = _context.Services
                    .Where(a => a.IsDeleted == false && (
                           a.Title.Trim().ToLower().Contains(trimedSearchString) 
                        || a.ShortDescription != null && a.ShortDescription.Trim().ToLower().Contains(trimedSearchString) 
                        || a.Description != null && a.Description.Trim().ToLower().Contains(trimedSearchString)
                     ))
                .Include(a => a.User).OrderByDescending(a => a.AddedDate).Skip(skip).Take(take).ToList();

            //var tags = _context.ServiceTags
            //        .Where(t => t.IsDeleted == false && (
            //               t.Title != null && t.Title.ToLower().Trim().Contains(trimedSearchString)
            //       ))
            //       .OrderByDescending(t => t.InsertDate).Skip(skip).Take(take).ToList();

            //foreach (var tag in tags)
            //{
            //    searchedServices.Add(GetService(tag.ServiceId));
            //}

            foreach (var service in services)
            {
                if (!searchedServices.Contains(service))
                {
                    searchedServices.Add(service);
                }
            }

            //|| a.ServiceTags != null && a.ServiceTags.Any(t => t.Title != null && t.Title.ToLower().Trim().Contains(trimedSearchString)

            return searchedServices;
        }
        #endregion
        #region Get Count
        public int GetServicesCount()
        {
            return _context.Services.Where(a => a.IsDeleted == false).Count();
        }
        public int GetServicesCount(int categoryId)
        {
            return _context.Services.Where(a => a.IsDeleted == false && a.ServiceCategoryId == categoryId).Count();
        }
        public int GetServicesCount(string searchString)
        {
            return _context.Services
                .Where(a => a.IsDeleted == false && (a.Title.Trim().ToLower().Contains(searchString.Trim().ToLower()) || a.ShortDescription.Trim().ToLower().Contains(searchString.Trim().ToLower())))
                .Count();
        }

        #endregion

        //public List<ServiceComment> GetServiceComments(int serviceId)
        //{
        //    return _context.ServiceComments.Where(c => c.IsDeleted == false && c.ServiceId == serviceId).ToList();
        //}

        //public List<ServiceTag> GetServiceTags(int serviceId)
        //{
        //    return _context.ServiceTags.Where(c => c.IsDeleted == false && c.ServiceId == serviceId).ToList();
        //}

        public List<ServiceHeadLine> GetServiceHeadlines(int serviceId)
        {
            return _context.ServiceHeadLines.Where(c => c.IsDeleted == false && c.ServiceId == serviceId).ToList();
        }

        public List<Service> GetServicesByCategoryId(int categoryId)
        {
            return _context.Services.Where(a => a.ServiceCategoryId == categoryId && a.IsDeleted == false).Include(a => a.User).Include(a => a.ServiceCategory).OrderBy(a => a.InsertDate).ToList();
        }

        public void UpdateServiceViewCount(int serviceId)
        {
            var service = _context.Services.Find(serviceId);
            service.ViewCount++;
            _context.Entry(service).State = EntityState.Modified;
            _context.SaveChanges();
        }

        //public void AddComment(ServiceComment comment)
        //{
        //    _context.ServiceComments.Add(comment);
        //    _context.SaveChanges();
        //}


        //public Service DeleteService(int serviceId)
        //{
        //    var service = _context.Services.Find(serviceId);
        //    if (service == null)
        //        return null;
        //    var serviceTags = _context.ServiceTags.Where(t => t.ServiceId == serviceId).ToList();
        //    var articeheadLines = _context.
        //}
    }
}
