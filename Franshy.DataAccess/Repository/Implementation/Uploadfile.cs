using Franshy.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class Uploadfile<T> : IUploadfile<T> where T : class
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public Uploadfile(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> upload(T entity, IFormFile file, string foldername)
        {
            var filename = Guid.NewGuid().ToString() + file.FileName;
            var folderpath = Path.Combine(webHostEnvironment.WebRootPath, "img", foldername);
            Directory.CreateDirectory(folderpath);
            using (var stream = new FileStream(Path.Combine(folderpath, filename), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/img/" + foldername + "/" + filename;
        }
    }
}
