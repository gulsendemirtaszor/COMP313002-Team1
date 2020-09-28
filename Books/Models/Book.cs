using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please specify a name")]
        public String Title { get; set; }

        [Required(ErrorMessage = "Please specify the author")]
        public String Author { get; set; }

        [Required(ErrorMessage = "Please specify the price")]
        public decimal Price { get; set; }

        //[Required(ErrorMessage = "Please specify the publication date")]
        public String PublicationDate { get; set; }

        //[Required(ErrorMessage = "Please specify the edition")]
        public String Edition { get; set; }

        //[Required(ErrorMessage = "Please specify the isbn")]
        public String Isbn { get; set; }

        [Required(ErrorMessage = "Please specify the category type")]
        public int Category { get; set; }

        [Required(ErrorMessage = "Please specify book conditions")]
        public int Conditions { get; set; }

        [Required(ErrorMessage = "Please specify a text for the post")]
        public String FullDescription { get; set; }

        public String Preview { get; set; }

        public string UserName { get; set; }

        public DateTime CurrentDateTime { get; set; }

        public void UpdateFrom(Book book)
        {
            //Id = book.Id;
            Title = book.Title;
            Price = book.Price;
            Author = book.Author;
            Edition = book.Edition;
            PublicationDate = book.PublicationDate;
            Isbn = book.Isbn;
            Category = book.Category;
            Conditions = book.Conditions;
            FullDescription = book.FullDescription;
            Preview = book.Preview;
        }      

    }
}
