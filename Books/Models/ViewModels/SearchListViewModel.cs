using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models.ViewModels
{
    public class SearchListViewModel
    {
        public BookInfoListViewModel bookInfoListViewModel { get; set; }

        public BooksListViewModel booksListViewModel { get; set; }

        public AppUser appUser { get; set; }

        public LoginModel loginModel { get; set; }

        public CategoriesListViewModel categoriesListViewModel { get; set; }

        public AppUserListViewModel appUserListViewModel { get; set; }

        public ConditionsListViewModel conditionsListViewModel { get; set; }

        public AdvertisementsListViewModel advertisementsListViewModel { get; set; }

    }
}
