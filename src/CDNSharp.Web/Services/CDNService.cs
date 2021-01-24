using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CDNSharp.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CDNSharp.Web.Models;

namespace CDNSharp.Web.Services
{
    public class CDNService : ICDNService
    {
        private LiteDatabase _liteDb;
        private readonly ILogger<CDNService> _logger;
        public CDNService(ILiteDbContext liteDbContext, ILogger<CDNService> logger)
        {
            _liteDb = liteDbContext.Database;
            _logger = logger;
        }

        Task<bool> ICDNService.AnyAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICDNService.AnyAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        Task ICDNService.DeleteAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        Task ICDNService.DeleteAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        Task<LiteFileInfo<string>> ICDNService.DownloadAsync(string id)
        {
            // Get a collection (or create, if doesn't exist)
            var col = _liteDb.FileStorage.FindAll().ToList();
            Stream s = new MemoryStream();
            var liteFileInfo = _liteDb.FileStorage.Download(id, s);
            return Task.FromResult(liteFileInfo);
        }

        Task<LiteFileInfo<string>> ICDNService.DownloadAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<CDNFileInfo<string>> ICDNService.GetAllFiles(int skip, int take)
        {
            var files = _liteDb.FileStorage.FindAll();
            var simpleFiles = from file in files
                              select new CDNFileInfo<string> { Filename = file.Filename, Id = file.Id, MimeType = file.MimeType };
            
            return simpleFiles;
        }

        IEnumerable<CDNFileInfo<string>> ICDNService.GetAllFilesByContentType(string contentType, int skip, int take)
        {
            throw new NotImplementedException();
        }

        Task<CDNFileInfo<string>> ICDNService.UploadAsync(IFormFile file, string fileName, string version)
        {
            var bson = new BsonDocument();
            bson["Name"] = "Mark Burton";
            bson["CreateDate"] = DateTime.Now;
            // Upload a file from a Stream
            var liteFileInfo = _liteDb.FileStorage.Upload($"{fileName}@{version}"
                , file.FileName, file.OpenReadStream(), bson);
            var cdnFileInfo = new CDNFileInfo<string> { Filename = liteFileInfo.Filename, Id = liteFileInfo.Id, MimeType = liteFileInfo.MimeType };
            return Task.FromResult(cdnFileInfo);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CDNService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
