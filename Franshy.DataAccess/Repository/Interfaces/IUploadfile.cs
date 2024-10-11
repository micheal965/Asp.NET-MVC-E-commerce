using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface IUploadfile<T> where T : class
    {
        Task<string> upload(T entity, IFormFile file, string foldername);
    }
}
