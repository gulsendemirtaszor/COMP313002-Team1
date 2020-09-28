using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class BookImage
    {
        [Key]
        public int BookImageId { get; set; }

        // ** For Thumbnail Image
        //[Required(ErrorMessage = "Please select thumbnail image.")]
        public string BookImageThumbnail { get; set; }

        // ** For Multiple Image(s)
        //[Required(ErrorMessage = "Please enter related image(s).")]
        public string BookImageCode { get; set; }

        // ** Referenced from Book Model (FK)
        public int Id { get; set; }
    }
}
