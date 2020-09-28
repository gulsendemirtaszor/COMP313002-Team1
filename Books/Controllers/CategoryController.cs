using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Books.Models;
using Books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Books.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;

        public CategoryController(ICategoryRepository repo) {
            repository = repo;
        }       

        [AllowAnonymous]
        public ActionResult ListCategory()
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "List of Categories";
            TempData["loadMessage"] = "Categories list has been loaded.";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.repository.Categories;
            _model.categoriesListViewModel = new CategoriesListViewModel();
            _model.categoriesListViewModel.Categories = this.repository.Categories;

            return View(_model);
        }

        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Edit Category";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.repository.Categories;
            _model.categoriesListViewModel = new CategoriesListViewModel();
            _model.categoriesListViewModel.category = this.repository.Categories
                .Where(r => r.Id == id).FirstOrDefault();
            _model.categoriesListViewModel.Categories = this.repository.Categories
                .Where(r => r.Id == id);
            return View(_model);
        }

        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Delete Category";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.repository.Categories;
            _model.categoriesListViewModel = new CategoriesListViewModel();
            _model.categoriesListViewModel.category = this.repository.DeleteCategory(id);
            return View(_model);
        }

        [HttpPost]
        public ActionResult EditCategory(SearchListViewModel _model)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Edit Category";
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.repository.Categories;

            if (ModelState.IsValid)
            {
                if (_model.categoriesListViewModel.category.Id > 0)
                {
                    repository.SaveCategory(_model.categoriesListViewModel.category);
                    TempData["returnMessage"] = "View Category Details";
                    TempData["successMessage"] = $"Successfully edited category: {_model.categoriesListViewModel.category.Name}";
                }
                else
                {
                    TempData["returnMessage"] = null;
                    TempData["successMessage"] = null;
                }
            }
            else
            {
                TempData["returnMessage"] = null;
                TempData["successMessage"] = null;
            }
            return View("ViewCategory", _model);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Add Category";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.repository.Categories;
            return View(_model);
        }

        [HttpPost]
        public ActionResult AddCategory(SearchListViewModel _model)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Add Category";

            if (ModelState.IsValid)
            {
                repository.SaveCategory(_model.categoriesListViewModel.category);
                TempData["returnMessage"] = "View Category Details";
                TempData["successMessage"] = $"Successfully added new category: {_model.categoriesListViewModel.category.Name}";
            }
            else
            {
                TempData["returnMessage"] = null;
                TempData["successMessage"] = null;
            }
          
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.repository.Categories;

            _model.categoriesListViewModel.Categories = this.repository.Categories;

            return View("ViewCategory", _model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult ViewCategory(int? id)
        {
            ViewBag.Title = "View Category Details";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.repository.Categories;
            _model.categoriesListViewModel = new CategoriesListViewModel();
            _model.categoriesListViewModel.category = this.repository.Categories
                .Where(r => r.Id == id).First();
            _model.categoriesListViewModel.Categories = this.repository.Categories;
            return View(_model);
        }

    }
}
