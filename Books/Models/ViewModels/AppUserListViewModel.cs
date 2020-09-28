using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models.ViewModels
{
    public class AppUserListViewModel
    {
        public AppUser appUser { get; set; }

        public IEnumerable<AppUser> appUsers { get; set; }

    }
}
