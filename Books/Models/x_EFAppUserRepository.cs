using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class EFAppUserRepository : IAppUserRepository
    {
        private AppIdentityDbContext context;
        public EncryptDecryptData _encryptDecryptData;

        public EFAppUserRepository(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<AppUser> AppUsers => context.AppUsers;

        public void SaveAppUser(AppUser appUser)
        {
            if (appUser.Id == string.Empty)
            {
                appUser.UserName =
                    appUser.FirstName.Substring(0, 1).ToUpper() +
                    appUser.LastName;

                this._encryptDecryptData = new EncryptDecryptData();
                appUser.Password = this._encryptDecryptData.EncryptData(appUser.Password);
                appUser.PasswordHash = this._encryptDecryptData.EncryptData(appUser.Password);

                appUser.UserType = "New";
                appUser.Email = appUser.EmailAddress;
                appUser.PhoneNumber = appUser.ContactNumber;
                appUser.HideMap = appUser.HideMap;
                context.AppUsers.Add(appUser);
            }
            else
            {
                AppUser appUserEntry = context.AppUsers
                    .FirstOrDefault(r => r.Id == appUser.Id);

                if (appUserEntry != null)
                {
                    appUserEntry.UpdateFrom(appUser);
                }
            }

            context.SaveChanges();
        }

        public void VerifyAppUser(AppUser appUser)
        {
            if (appUser.Id != string.Empty)
            {
                AppUser appUserEntry = context.AppUsers
                    .FirstOrDefault(r => r.Id == appUser.Id);

                if (appUserEntry != null)
                {
                    appUserEntry.IsVerified = true;
                    appUserEntry.VerificationCode = "Expired";
                    appUserEntry.UpdateFrom(appUser);
                }

                context.SaveChanges();
            }
        }

    }
}
