using YooPoon.Core.Data;
using YooPoon.Data.EntityFramework;

namespace Zerg.Common.Data
{
    public class CMSRepository<T> :EfRepository<T>,ICMSRepository<T> where T : class,IBaseEntity
    {
        public CMSRepository(IDbContext context):base(context)
        {
            
        } 
    }

    public class CRMRepository<T> : EfRepository<T>, ICRMRepository<T> where T : class,IBaseEntity
    {
        public CRMRepository(IDbContext context)
            : base(context)
        {

        }
    }

    public class TradingRepository<T> : EfRepository<T>, ITradingRepository<T> where T : class,IBaseEntity
    {
        public TradingRepository(IDbContext context)
            : base(context)
        {

        }
    }

    /// <summary>
    /// Event活动数据仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventRepository<T> : EfRepository<T>, IEventRepository<T> where T : class,IBaseEntity
    {
        public EventRepository(IDbContext context)
            : base(context)
        {

        }
    }
}