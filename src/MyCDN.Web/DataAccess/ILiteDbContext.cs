using LiteDB;

namespace MyCDN.Web.DataAccess
{
    public interface ILiteDbContext
    {
        public LiteDatabase Database { get; }
    }
}
