using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models.ViewModels
{
    public class AdvertisementsListViewModel
    {
        public Advertisement advertisement { get; set; }

        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}
