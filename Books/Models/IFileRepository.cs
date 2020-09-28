using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Books.Models
{
    public interface IFileRepository
    {
        IQueryable<BookImage> MyBookImages { get; }

        void SaveBookImages(BookImage bookImage, IFormFile thumbnail, int bookId);

        void ThumbnailBook(IFormFile thumbnail);

        void RelatedImagesBook(List<IFormFile> relatedImages, string imageCode);
    }
}
