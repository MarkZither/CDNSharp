using LiteDB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCDN.Web.Services
{
    public interface ICDNService : IDisposable
    {
        Task<LiteFileInfo<string>> UploadAsync(IFormFile file);
        Task<bool> AnyAsync(ObjectId id);
        Task<bool> AnyAsync(string fileName);
        Task DeleteAsync(string fileName);
        Task DeleteAsync(ObjectId id);
        Task<LiteFileInfo<string>> DownloadAsync(string fileName);
        Task<LiteFileInfo<string>> DownloadAsync(ObjectId id);
        object GetAllFilesByContentType(string contentType, int skip, int
        take);
        object GetAllFiles(int skip, int take);
    }
}
