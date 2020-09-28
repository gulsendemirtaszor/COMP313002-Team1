using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public interface IAdvertisementRepository
    {
        IQueryable<Advertisement> Advertisements { get; }

        void SaveAdvertisement(Advertisement advertisement);

        Advertisement DeleteAdvertisement(int requestID);
    }
}
