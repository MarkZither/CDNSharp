using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CDNSharp.Web.Models
{
    public class CDNFileInfoString
    {
        public string Id { get; set; }
        public string Filename { get; set; }
        public string MimeType { get; set; }
    }
}
