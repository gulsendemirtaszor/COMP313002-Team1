using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Books.Models;
using Books.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace Books.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
        private ICategoryRepository categoryRepository;
        private IFileRepository _fileRepository;
        private UserManager<AppUser> _userManager;
        private IConditionRepository _conditionRepository;
        private IAdvertisementRepository _advertisementRepository;

        public BookController(IBookRepository repo, ICategoryRepository categoryRepo,
            IFileRepository fileRepo, UserManager<AppUser> userManagerRepo,
            IConditionRepository conditionRepo, IAdvertisementRepository advertisementRepo)
        {
            repository = repo;
            categoryRepository = categoryRepo;
            this._fileRepository = fileRepo;
            this._userManager = userManagerRepo;
            this._conditionRepository = conditionRepo;
            this._advertisementRepository = advertisementRepo;
        }

        public ViewResult Index()
        {
            ViewBag.Title = "Home";

            //BookInfoListViewModel model = new BookInfoListViewModel();
            //model.Categories = categoryRepository.Categories;
            //model.MyBookImages = this._fileRepository.MyBookImages
            //  .OrderBy(f => f.BookImageId);

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();            
            _model.bookInfoListViewModel.Categories = categoryRepository.Categories;
            _model.bookInfoListViewModel.MyBookImages = this._fileRepository.MyBookImages
                .OrderBy(f => f.BookImageId);
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;
            _model.booksListViewModel = new BooksListViewModel();
            _model.booksListViewModel.Books = this.repository.Books;

            _model.advertisementsListViewModel = new AdvertisementsListViewModel();
            _model.advertisementsListViewModel.Advertisements = this._advertisementRepository.Advertisements
                .OrderBy(ads => ads.RequestId);

            return View(_model);
        }

        public ViewResult ListBooks(int categoryId, string userName)
        {
            ViewBag.Title = "List of Books";
            TempData["loadMessage"] = "Books list has been loaded.";

            SearchListViewModel _model;
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            if (categoryId == 0)
            {
                _model = new SearchListViewModel();
                _model.booksListViewModel = new BooksListViewModel();
                _model.booksListViewModel.Books = isAdmin ? this.repository.Books : this.repository.Books
                    .Where(u => u.UserName == userName);

                int _bookCount = _model.booksListViewModel.Books.Count();
                if (_bookCount == 0)
                {
                    TempData["errorMessage"] = "No record(s) found";
                }


                _model.booksListViewModel.MyBookImages = this._fileRepository.MyBookImages;
                _model.bookInfoListViewModel = new BookInfoListViewModel();
                _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
                _model.appUserListViewModel = new AppUserListViewModel();
                _model.appUserListViewModel.appUsers = this._userManager.Users
                    .OrderBy(u => u.Id);
                _model.conditionsListViewModel = new ConditionsListViewModel();
                _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

                return View(_model);
            }
            else
            {
                _model = new SearchListViewModel();
                _model.booksListViewModel = new BooksListViewModel();
                _model.booksListViewModel.Books = isAdmin ? this.repository.Books.Where(r => r.Category == categoryId) :
                    this.repository.Books.Where(r => r.Category == categoryId && r.UserName == userName);                
                _model.booksListViewModel.MyBookImages = this._fileRepository.MyBookImages;
                _model.bookInfoListViewModel = new BookInfoListViewModel();
                _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
                _model.conditionsListViewModel = new ConditionsListViewModel();
                _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;
                return View(_model);

            }
        }

        [Authorize]
        [HttpGet]
        public ViewResult EditBook(int? id)
        {
            ViewBag.Title = "Edit Book";

            Book book = repository.Books.Where(r => r.Id == id).FirstOrDefault();
            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
            _model.bookInfoListViewModel.book = book;
            _model.bookInfoListViewModel.MyBookImages = this._fileRepository.MyBookImages
                .Where(i => i.Id == id);
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            //BookInfoListViewModel model = new BookInfoListViewModel();
            //model.Categories = categoryRepository.Categories;
            //model.book = book;
            //model.MyBookImages = this._fileRepository.MyBookImages
            //    .Where(i => i.Id == id);

            TempData["processId"] = id;

            return View(_model);
        }

        [Authorize]
        [HttpGet]
        public ViewResult DeleteBook(int id)
        {
            ViewBag.Title = "Delete Book";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
            _model.bookInfoListViewModel.book = repository.DeleteBook(id);
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            return View(_model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditBook(SearchListViewModel _model, IFormFile thumbnail,
            List<IFormFile> relatedImages)
        {
            ViewBag.Title = "Edit Book";
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            if (ModelState.IsValid)
            {
                if (_model.bookInfoListViewModel.book.Id > 0)
                {
                    repository.SaveBook(_model.bookInfoListViewModel.book);

                    _model.bookInfoListViewModel.MyBookImages = this._fileRepository.MyBookImages
                        .Where(i => i.Id == _model.bookInfoListViewModel.book.Id);

                    foreach (BookImage bi in _model.bookInfoListViewModel.MyBookImages)
                    {
                        this._fileRepository.RelatedImagesBook(relatedImages, bi.BookImageCode);
                    }

                    TempData["returnMessage"] = "View Book Details";
                    TempData["successMessage"] = $"Successfully edited book: {_model.bookInfoListViewModel.book.Title}";
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
            return View("ViewBook", _model);
        }

        [Authorize]
        [HttpGet]
        public ViewResult AddBook()
        {
            ViewBag.Title = "Add Book Posting";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            //BookInfoListViewModel model = new BookInfoListViewModel();
            //model.Categories = categoryRepository.Categories;

            return View(_model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddBook(SearchListViewModel _model, BookImage bookImage,
            IFormFile thumbnail, List<IFormFile> relatedImages)
        {
            ViewBag.Title = "Add Book";

            if (ModelState.IsValid)
            {
                repository.SaveBook(_model.bookInfoListViewModel.book);

                this._fileRepository.SaveBookImages(bookImage, thumbnail, _model.bookInfoListViewModel.book.Id);
                this._fileRepository.ThumbnailBook(thumbnail);
                this._fileRepository.RelatedImagesBook(relatedImages, bookImage.BookImageCode);

                TempData["returnMessage"] = "View Book Details";
                TempData["successMessage"] = $"Successfully added new book: {_model.bookInfoListViewModel.book.Title}";
            }
            else
            {
                TempData["returnMessage"] = null;
                return View();
            }

            _model.bookInfoListViewModel.Categories = categoryRepository.Categories;
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            _model.bookInfoListViewModel.MyBookImages = this._fileRepository.MyBookImages
                .Where(i => i.Id == _model.bookInfoListViewModel.book.Id);

            return View("ViewBook", _model);
        }

        [HttpGet]
        public ViewResult ViewBook(int? id)
        {
            ViewBag.Title = "View Book Details";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
            _model.bookInfoListViewModel.book = repository.Books.Where(r => r.Id == id).First();
            _model.bookInfoListViewModel.MyBookImages = this._fileRepository.MyBookImages.OrderBy(f => f.Id == id);
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            //BookInfoListViewModel model = new BookInfoListViewModel();
            //model.Categories = categoryRepository.Categories;
            //model.book = repository.Books.Where(r => r.Id == id).First();
            //model.MyBookImages = this._fileRepository.MyBookImages.OrderBy(f => f.Id == id);

            return View(_model);
        }

        //[HttpPost]
        public ViewResult SearchResult(SearchListViewModel _model)
        {
            ViewBag.Title = "Search Result";

            //_model.bookInfoListViewModel.Categories = new List<Category>();
            _model.bookInfoListViewModel.Categories = categoryRepository.Categories;

            string _item = _model.bookInfoListViewModel.bookSearch.Item;
            decimal _priceFrom = _model.bookInfoListViewModel.bookSearch.PriceFrom;
            decimal _priceTo = _model.bookInfoListViewModel.bookSearch.PriceTo;
            int _category = _model.bookInfoListViewModel.bookSearch.Category;

            //_model.bookInfoListViewModel.MyBooks = repository.Books
            //    .Where(r => r.Category == _category ||
            //    r.Title.Contains(_item) ||
            //    (r.Price >= _priceFrom && r.Price <= _priceTo));

            _model.bookInfoListViewModel.MyBooks = repository.Books
                .Where(r => (r.Category == _category || _category == 0 ) &&
                            (r.Title.Contains(_item) ||  string.IsNullOrEmpty(_item)) && 
                            ((r.Price >= _priceFrom || r.Price== _priceFrom) && (r.Price <= _priceTo || _priceTo==0))
                            );
            _model.bookInfoListViewModel.MyBookImages = this._fileRepository.MyBookImages
                .OrderBy(f => f.BookImageId);

            if (_model.bookInfoListViewModel.MyBooks == null)
            {
                TempData["searchMessage"] = "No record(s) found.";
            }
            else
            {
                TempData["searchMessage"] = null;
            }

            _model.appUserListViewModel = new AppUserListViewModel();
            _model.appUserListViewModel.appUsers = this._userManager.Users
                .OrderBy(u => u.Id);
            _model.conditionsListViewModel = new ConditionsListViewModel();
            _model.conditionsListViewModel.Conditions = this._conditionRepository.Conditions;

            return View(_model);
        }

        public ViewResult BuyingTips()
        {
            return View("BuyingTips");
        }

        [Authorize]
        [HttpGet]
        public ViewResult RequestAdvertisement()
        {
            ViewBag.Title = "Send Request for Book Advertisement";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
            return View(_model);
        }

        [Authorize]
        [HttpPost]
        public ViewResult RequestAdvertisement(SearchListViewModel _model)
        {
            ViewBag.Title = "Send Request for Book Advertisement";

            if (ModelState.IsValid)
            {
                this._advertisementRepository.SaveAdvertisement(_model.advertisementsListViewModel.advertisement);
                TempData["returnMessage"] = "View Request Advertisement Details";
                TempData["successMessage"] = $"Successfully added new advertisement: {_model.advertisementsListViewModel.advertisement.Title}";
            }
            else
            {
                TempData["returnMessage"] = null;
                TempData["successMessage"] = null;
            }

            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;

            return View("ViewRequestAdvertisements", _model);
        }

        [HttpGet]
        public ViewResult ViewRequestAdvertisements(int? requestId)
        {
            ViewBag.Title = "View Request Advertisement Details";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this.categoryRepository.Categories;
            
            _model.advertisementsListViewModel = new AdvertisementsListViewModel();
            _model.advertisementsListViewModel.advertisement = this._advertisementRepository.Advertisements
                .Where(ads => ads.RequestId == requestId).First();

            return View(_model);
        }


    }
}
