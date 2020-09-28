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
    public class ConditionController : Controller
    {
        private IConditionRepository _conditionRepository;
        private ICategoryRepository _categoryRepository;

        public ConditionController(IConditionRepository conditionRepo, 
            ICategoryRepository categoryRepository)
        {
            this._conditionRepository = conditionRepo;
            this._categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        public ActionResult ListCondition()
        {                       

            ViewBag.Title = "List of Conditions";
            TempData["loadMessage"] = "Conditions list has been loaded.";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            return View(_model);
        }

        [HttpGet]
        public ActionResult EditCondition(int? id)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Edit Condition";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.condition = this._conditionRepository.Conditions
                .Where(r => r.Id == id).FirstOrDefault();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions
                .Where(r => r.Id == id);
            return View(_model);
        }

        [HttpPost]
        public ActionResult EditCondition(SearchListViewModel _model)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Edit Condition";
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            if (ModelState.IsValid)
            {
                if (_model.conditionsListViewModel.condition.Id > 0)
                {
                    this._conditionRepository.SaveCondition(_model.conditionsListViewModel.condition);
                    TempData["returnMessage"] = "View Category Details";
                    TempData["successMessage"] = $"Successfully edited condition: {_model.conditionsListViewModel.condition.Description}";
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
            return View("ViewCondition", _model);
        }

        [HttpGet]
        public ActionResult DeleteCondition(int id)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Delete Condition";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.condition = this._conditionRepository.DeleteCondition(id);
            return View(_model);
        }

        [HttpGet]
        public ActionResult AddCondition()
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Add Condition";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;
            return View(_model);

            //return View();
        }

        [HttpPost]
        public ActionResult AddCondition(SearchListViewModel _model)
        {
            if (HttpContext.Session.GetString("auth") != "completed") return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Add Condition";

            if (ModelState.IsValid)
            {
                this._conditionRepository.SaveCondition(_model.conditionsListViewModel.condition);
                TempData["returnMessage"] = "View Category Details";
                TempData["successMessage"] = $"Successfully added new condition: {_model.conditionsListViewModel.condition.Description}";
            }
            else
            {
                TempData["returnMessage"] = null;
                TempData["successMessage"] = null;
            }

            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            return View("ViewCondition", _model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult ViewCondition(int? id)
        {
            ViewBag.Title = "View Condition Detail";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.condition = this._conditionRepository.Conditions
                .Where(r => r.Id == id).First();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;
            return View(_model);
        }

    }
}
