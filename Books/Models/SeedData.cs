using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//here
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
// from the project folder: dotnet ef migrations Add Initial
namespace Books.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            // ** Category            
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Business",
                        Description = "Business",
                    },
                    new Category
                    {
                        Name = "Communication",
                        Description = "Communication",
                    },
                    new Category
                    {
                        Name = "Computer Science",
                        Description = "Computer Science",
                    },
                    new Category
                    {
                        Name = "Economics",
                        Description = "Economics",
                    },
                    new Category
                    {
                        Name = "Education",
                        Description = "Education",
                    },
                    new Category
                    {
                        Name = "Engineering",
                        Description = "Engineering",
                    },
                    new Category
                    {
                        Name = "Humanities",
                        Description = "Humanities",
                    },
                    new Category
                    {
                        Name = "Journalism",
                        Description = "Journalism",
                    },
                    new Category
                    {
                        Name = "Law",
                        Description = "Law",
                    },
                    new Category
                    {
                        Name = "Mathematics",
                        Description = "Mathematics",
                    },
                     new Category
                     {
                         Name = "Media Studies",
                         Description = "Media Studies",
                     },
                    new Category
                    {
                        Name = "Medicine",
                        Description = "Medicine",
                    },
                    new Category
                    {
                        Name = "Natural Sciences",
                        Description = "Natural Sciences",
                    },
                    new Category
                    {
                        Name = "Sociology",
                        Description = "Sociology",
                    },
                    new Category
                    {
                        Name = "Philosophy",
                        Description = "Philosophy",
                    },
                    new Category
                    {
                        Name = "Physics",
                        Description = "Physics",
                    },
                    new Category
                    {
                        Name = "Psychology",
                        Description = "Psychology",
                    }
                );

                context.SaveChanges();
            }

            // ** Book
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Title = "First Book",
                        Price = 3,
                        Author = "John",
                        Edition = "1st",
                        PublicationDate = "2020-01-01",
                        FullDescription = "..."
                    } ,
                     new Book
                     {
                         Title = "Hacking for Dummies",
                         Price = 100,
                         Author = "Kevin",
                         Edition = "2st",
                         PublicationDate = "2020-01-01",
                         FullDescription = "..."
                     }
                );
                context.SaveChanges();
            }

            // ** Condition
            if (!context.Conditions.Any())
            {
                context.Conditions.AddRange(
                    new Condition
                    {
                        Description = "BrandNew"
                    },
                    new Condition
                    {
                        Description = "Used - mint conditions"
                    }, 
                    new Condition
                    {
                        Description = "Used - slightly deteriorated"
                    }, 
                    new Condition
                    {
                        Description = "Used - deteriorated"
                    }, 
                    new Condition
                    {
                        Description = "Almost unusable"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
