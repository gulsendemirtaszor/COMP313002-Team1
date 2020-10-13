using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Books.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        //public DbSet<AppUser> AppUsers { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<BookImage> MyBookImages { get; set; }

        public DbSet<Condition> Conditions { get; set; }

        public DbSet<Advertisement> Advertisements { get; set; }

    }
}