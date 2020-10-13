using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class EFAdvertisementRepository : IAdvertisementRepository
    {
        private ApplicationDbContext _context;

        public EFAdvertisementRepository(ApplicationDbContext ctx)
        {
            this._context = ctx;
        }

        public IQueryable<Advertisement> Advertisements => this._context.Advertisements;

        
        public void SaveAdvertisement(Advertisement advertisement)
        {
            if (advertisement.RequestId == 0)
            {
                this._context.Advertisements.Add(advertisement);
            }
            else
            {
                Advertisement _advertisementEntry = this._context.Advertisements
                    .FirstOrDefault(a => a.RequestId == advertisement.RequestId);

                if (_advertisementEntry != null)
                {
                    _advertisementEntry.UpdateFrom(advertisement);
                }
            }

            this._context.SaveChanges();
        }

        public Advertisement DeleteAdvertisement(int requestID)
        {
            Advertisement _advertisementEntry = this._context.Advertisements
                .FirstOrDefault(a => a.RequestId == requestID);

            if (_advertisementEntry != null)
            {
                this._context.Advertisements.Remove(_advertisementEntry);
                this._context.SaveChanges();
            }

            return _advertisementEntry;
        }
    }
}
