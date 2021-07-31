using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using NovinKargar.Core.Models;
using NovinKargar.Infrastructure.Repositories;

namespace NovinKargar.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ArticleCategoriesController : Controller
    {
        private readonly ArticleCategoriesRepository _repo;
        public ArticleCategoriesController(ArticleCategoriesRepository repo)
        {
            _repo = repo;
        }
        // GET: Admin/ArticleCategories
        public ActionResult Index(int? parentId)
        {
            //return View(_repo.GetAll());

            List<ArticleCategory> productGroups;

            if (parentId == null)
                productGroups = _repo.GetArticleCategoryTable();
            else
            {
                productGroups = _repo.GetArticleCategoryTable(parentId.Value);
                var parent = _repo.Get(parentId.Value);
                ViewBag.PrevParent = parent.ParentId;
                ViewBag.ParentId = parentId;
                ViewBag.ParentName = parent.Title;
            }


            return View(productGroups);
        }

        // GET: Admin/ArticleCategories/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(_repo.GetMainArticleCategories(), "Id", "Title");
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title, ParentId")] ArticleCategory articleCategory)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(articleCategory);
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(_repo.GetMainArticleCategories(), "Id", "Title", articleCategory.Id);

            return View(articleCategory);
        }

        // GET: Admin/ArticleCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleCategory articleCategory = _repo.Get(id.Value);
            if (articleCategory == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentId = new SelectList(_repo.GetMainArticleCategories(), "Id", "Title", articleCategory.Id);

            return PartialView(articleCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title, ParentId")] ArticleCategory articleCategory)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(articleCategory);
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(_repo.GetMainArticleCategories(), "Id", "Title", articleCategory.Id);

            return View(articleCategory);
        }

        // GET: Admin/ArticleCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleCategory articleCategory = _repo.Get(id.Value);
            if (articleCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(articleCategory);
        }

        // POST: Admin/ArticleCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
