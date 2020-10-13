using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public interface ICategoryRepository
    {        
        IQueryable<Category> Categories { get; }
        void SaveCategory(Category category);
        Category DeleteCategory(int categoryID);
    }
}
