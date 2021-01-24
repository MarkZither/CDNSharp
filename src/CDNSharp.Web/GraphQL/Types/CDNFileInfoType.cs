using GraphQL.Types;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNSharp.Web.Models
{
    public class CDNFileInfoType : ObjectGraphType<CDNFileInfo<string>>
    {
        public CDNFileInfoType()
        {
            Name = "CDNFileInfo";
            Field(b => b.Filename, type: typeof(StringGraphType)).Description("The file name");
        }
    }
}
