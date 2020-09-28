using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Books.Models
{
    public class EFFileRepository : IFileRepository
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _host;

        public EFFileRepository(ApplicationDbContext ctx, IHostingEnvironment host)
        {
            this._context = ctx;
            this._host = host;
        }

        public IQueryable<BookImage> MyBookImages => this._context.MyBookImages;

        public void SaveBookImages(BookImage bookImage, IFormFile thumbnail, 
            int bookId)
        {
            if (bookImage.BookImageId == 0)
            {
                if (thumbnail == null)
                {
                    bookImage.BookImageThumbnail = "plain_image.png";
                }
                else
                {
                    bookImage.BookImageThumbnail = thumbnail.FileName;
                }
                
                bookImage.Id = bookId; // ** Foreign Key
                this._context.MyBookImages.Add(bookImage);
            }

            this._context.SaveChanges();
        }

        public void ThumbnailBook(IFormFile thumbnail)
        {
            if (thumbnail != null)
            {
                if (thumbnail.Length > 0)
                {
                    var _filePath = this._host.WebRootPath + "\\uploads\\" +
                        thumbnail.FileName;
                    using (var stream = File.Create(_filePath))
                    {
                        thumbnail.CopyTo(stream);
                    }
                }
            }
        }

        public void RelatedImagesBook(List<IFormFile> relatedImages, string imageCode)
        {
            foreach (var formFile in relatedImages)
            {
                if (formFile.Length > 0)
                {
                    var _filePath = this._host.WebRootPath + $"\\uploads\\{imageCode}_" +
                        formFile.FileName;
                    using (var stream = File.Create(_filePath))
                    {
                        formFile.CopyTo(stream);
                    }
                }
            }
        }

    }
}
