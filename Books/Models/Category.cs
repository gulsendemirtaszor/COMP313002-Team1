using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please specify a name for the category")]
        public string Name { get; set; }
        public string Description { get; set; }        
        public string Notes { get; set; }

        public void UpdateFrom(Category category)
        {
            //Id = category.Id;
            Name = category.Name;
            Description = category.Description;        
            Notes = category.Notes;            
        }
    }
}
