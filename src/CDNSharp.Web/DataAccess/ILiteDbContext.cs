using LiteDB;

namespace CDNSharp.Web.DataAccess
{
    public interface ILiteDbContext
    {
        public LiteDatabase Database { get; }
    }
}
