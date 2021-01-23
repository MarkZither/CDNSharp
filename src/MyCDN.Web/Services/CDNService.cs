using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyCDN.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyCDN.Web.Services
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

        Task<LiteFileInfo<string>> ICDNService.DownloadAsync(string fileName)
        {
            Stream s = new MemoryStream();
            var fs = Task.Run(() => _liteDb.FileStorage.Download("$/photos/2014/picture-01.jpg", s));
            return fs;
        }

        Task<LiteFileInfo<string>> ICDNService.DownloadAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        object ICDNService.GetAllFiles(int skip, int take)
        {
            throw new NotImplementedException();
        }

        object ICDNService.GetAllFilesByContentType(string contentType, int skip, int take)
        {
            throw new NotImplementedException();
        }

        Task<LiteFileInfo<string>> ICDNService.UploadAsync(IFormFile file)
        {
            // Upload a file from a Stream
            var liteFileInfo = Task.Run(() => _liteDb.FileStorage.Upload("$/photos/2014/picture-01.jpg", file.FileName, file.OpenReadStream()));
            return liteFileInfo;
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
