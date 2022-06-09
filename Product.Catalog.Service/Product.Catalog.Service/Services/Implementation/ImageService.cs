using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Product.Catalog.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Catalog.Service.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string UploadImage(IFormFile image)
        {
            var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
            var imagePath = Path.Combine(directoryPath, image.FileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return "/Images/" + image.FileName;
        }
    }
}
