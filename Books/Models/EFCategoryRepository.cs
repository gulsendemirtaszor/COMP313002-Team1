using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//update-database -Context ApplicationDbContext
//update-database -Context AppIdentityDbContext

namespace Books.Models
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext context;

        public EFCategoryRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Category> Categories => context.Categories;


        public void SaveCategory(Category category)
        {
            if (category.Id == 0)            
                context.Categories.Add(category);           
            else
            {
                Category categoryEntry = context.Categories
                    .FirstOrDefault(r => r.Id == category.Id);

                if (categoryEntry != null)                
                    categoryEntry.UpdateFrom(category);                                    
            }
            context.SaveChanges();
        }

        public Category DeleteCategory(int categoryID)
        {
            Category categoryEntry = context.Categories
                .FirstOrDefault(r => r.Id == categoryID);

            if (categoryEntry != null)
            {
                context.Categories.Remove(categoryEntry);
                context.SaveChanges();
            }
            return categoryEntry;
        }
    }
}
