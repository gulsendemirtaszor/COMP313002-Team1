using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public interface IAppUserRepository
    {
        IQueryable<AppUser> AppUsers { get; }

        void SaveAppUser(AppUser appUser);

        void VerifyAppUser(AppUser appUser);
    }
}
