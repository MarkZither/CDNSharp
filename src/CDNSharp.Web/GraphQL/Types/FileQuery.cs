using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDNSharp.Web.Models;
using CDNSharp.Web.Services;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;

namespace CDNSharp.Web.GraphQL.Types
{
    public class FileQuery : ObjectGraphType
    {
        public FileQuery (ICDNService CDNService)
        {
            Field<ListGraphType<CDNFileInfoType>>(
                "files",
                resolve: context => CDNService.GetAllFiles(0, 100000)
                );
        }
    }
}
