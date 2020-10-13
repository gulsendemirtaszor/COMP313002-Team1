using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class Advertisement
    {
        [Key]
        public int RequestId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string RequestedBy { get; set; }

        public DateTime DateRequested { get; set; }

        public void UpdateFrom (Advertisement advertisement)
        {
            this.Status = advertisement.Status;
        }
    }
}
