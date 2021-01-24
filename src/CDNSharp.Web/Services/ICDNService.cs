using CDNSharp.Web.Models;
using LiteDB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNSharp.Web.Services
{
    public interface ICDNService : IDisposable
    {
        public Task<CDNFileInfo<string>> UploadAsync(IFormFile file, string fileName, string version);
        public Task<bool> AnyAsync(ObjectId id);
        public Task<bool> AnyAsync(string fileName);
        public Task DeleteAsync(string fileName);
        public Task DeleteAsync(ObjectId id);
        public Task<LiteFileInfo<string>> DownloadAsync(string fileName);
        public Task<LiteFileInfo<string>> DownloadAsync(ObjectId id);
        public IEnumerable<CDNFileInfo<string>> GetAllFilesByContentType(string contentType, int skip, int take);
        public IEnumerable<CDNFileInfo<string>> GetAllFiles(int skip, int take);
    }
}
