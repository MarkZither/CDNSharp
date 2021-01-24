using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNSharp.Web.Models
{
    public record CDNFileInfo<TFileId>
    {
        public TFileId Id { get; init; }
        [BsonField("filename")]
        public string Filename { get; init; }
        [BsonField("mimeType")]
        public string MimeType { get; init; }
    }
}
