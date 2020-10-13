using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class Condition
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please specify a description for a condition")]
        public string Description { get; set; }

        public void UpdateFrom(Condition condition)
        {
            this.Description = condition.Description;
        }
    }
}
