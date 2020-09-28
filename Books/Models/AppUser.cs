using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class AppUser : IdentityUser
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = "Please specify first name")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Alphanumeric character(s) only")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Please specify middle name")] Only few people have middle names
        [StringLength(1, MinimumLength = 1, 
            ErrorMessage = "Should contain 1 character")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please specify last name")] // Last names can contain spaces
        [RegularExpression("^[a-zA-Z\x20]*$", 
            ErrorMessage = "Alphanumeric character(s) only")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please specify an address")]
        [StringLength(100, MinimumLength = 10, 
            ErrorMessage = "Minimum of 10 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please specify contact number")]
        [RegularExpression("^\\d{10}$", 
            ErrorMessage = "Invalid contact number format")]
        public string ContactNumber { get; set; }

        public string UserType { get; set; }

        [Required(ErrorMessage = "Please specify password")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Please specify email address")]
        
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",           
            ErrorMessage = "Invalid email address format")]
        public string EmailAddress { get; set; }

        public bool IsVerified { get; set; }

        public string VerificationCode { get; set; }

        //public string AppUserFK { get; set; }

        public void UpdateFrom(AppUser appUser)
        {
            this.FirstName = appUser.FirstName;
            this.MiddleName = appUser.MiddleName;
            this.LastName = appUser.LastName;
            this.Address = appUser.Address;
            this.ContactNumber = appUser.ContactNumber;
            this.UserType = appUser.UserType;
            this.Password = appUser.Password;
            this.EmailAddress = appUser.EmailAddress;
            this.IsVerified = appUser.IsVerified;
            this.VerificationCode = appUser.VerificationCode;
        }
    }
}
