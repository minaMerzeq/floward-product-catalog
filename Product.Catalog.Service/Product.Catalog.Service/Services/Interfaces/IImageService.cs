using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Catalog.Service.Services.Interfaces
{
    public interface IImageService
    {
        string UploadImage(IFormFile image);
    }
}
