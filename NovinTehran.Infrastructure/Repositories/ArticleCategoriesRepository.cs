using NovinTehran.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovinTehran.Infrastructure.Repositories
{
    public class ArticleCategoriesRepository : BaseRepository<ArticleCategory, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ArticleCategoriesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<ArticleCategory> GetArticleCategoryTable()
        {
            var allCategories = _context.ArticleCategories.Where(ac => ac.ParentId == null && ac.IsDeleted == false).Include(p => p.Children).ToList();

            foreach (var category in allCategories)
            {
                category.Children = _context.ArticleCategories.Where(ac => ac.ParentId == category.Id && ac.IsDeleted == false).ToList();
            }

            return allCategories;
        }

        public List<ArticleCategory> GetArticleCategoryTable(int id)
        {
            var allCategories = _context.ArticleCategories.Where(ac => ac.ParentId == id && ac.IsDeleted == false).Include(ac => ac.Children).ToList();

            foreach (var category in allCategories)
            {
                category.Children = _context.ArticleCategories.Where(ac => ac.ParentId == category.Id && ac.IsDeleted == false).ToList();
            }

            return allCategories;
        }

        public ArticleCategory GetArticleCategory(int id)
        {
            var pg = _context.ArticleCategories.Include(ac => ac.Children)
                .Include(ac => ac.Parent).FirstOrDefault(ac => ac.Id == id);
            return pg;
        }

        public List<Feature> GetFeatures()
        {
            return _context.Features.Where(f => f.IsDeleted == false).ToList();
        }

        public List<ArticleCategory> GetArticleCategories()
        {
            return _context.ArticleCategories.Where(f => f.IsDeleted == false).Include(p => p.Children).OrderByDescending(p => p.InsertDate).ToList();
        }

        public ArticleCategory AddNewArticleCategory(int parentId, string title)
        {
            var ArticleCategory = new ArticleCategory();

            var user = GetCurrentUser();
            ArticleCategory.InsertDate = DateTime.Now;
            ArticleCategory.InsertUser = user.UserName;

            #region Adding Article Group
            ArticleCategory.Title = title;
            if (parentId != 0)
                ArticleCategory.ParentId = parentId;
            _context.ArticleCategories.Add(ArticleCategory);
            _context.SaveChanges();
            _logger.LogEvent(ArticleCategory.GetType().Name, ArticleCategory.Id, "Add");
            #endregion

            _context.SaveChanges();

            return ArticleCategory;
        }

        public ArticleCategory UpdateArticleCategory(int parentId, int ArticleCategoryId, string title)
        {
            var ArticleCategory = Get(ArticleCategoryId);
            var user = GetCurrentUser();
            ArticleCategory.UpdateDate = DateTime.Now;
            ArticleCategory.UpdateUser = user.UserName;

            #region Adding Article Group
            ArticleCategory.Title = title;
            if (parentId != 0)
                ArticleCategory.ParentId = parentId;
            else
                ArticleCategory.ParentId = null;
            Update(ArticleCategory);
            _logger.LogEvent(ArticleCategory.GetType().Name, ArticleCategory.Id, "Update");
            #endregion

            return ArticleCategory;
        }

        public List<ArticleCategory> GetChildrenArticleCategories(int? parentId = null)
        {
            var groups = new List<ArticleCategory>();
            if (parentId == null)
                groups = _context.ArticleCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();
            else
                groups = _context.ArticleCategories.Where(p => p.IsDeleted == false && p.ParentId == parentId).Include(p => p.Children).ToList();
            return groups;
        }

        public List<ArticleCategory> GetMainArticleCategories()
        {
            var groups = new List<ArticleCategory>();

            groups = _context.ArticleCategories.Where(p => p.IsDeleted == false && p.ParentId == null).Include(p => p.Children).ToList();

            return groups;
        }

        public ArticleCategory GetCategoryByArticleId(int articleId)
        {
            var articleCategoryId = _context.ArticleCategories.Where(ac => ac.IsDeleted == false && ac.Id == articleId).Select(ac => ac.Id).FirstOrDefault();

            return GetArticleCategory(articleCategoryId);
        }
    }
}
