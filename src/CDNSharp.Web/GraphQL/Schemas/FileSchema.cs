using CDNSharp.Web.GraphQL.Types;
using CDNSharp.Web.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNSharp.Web.GraphQL.Schemas
{
    public class FileSchema : Schema
    {
        public FileSchema(ICDNService repo, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = new FileQuery(repo);
        }
    }
}
