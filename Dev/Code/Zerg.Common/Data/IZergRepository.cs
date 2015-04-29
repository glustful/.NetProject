using YooPoon.Core.Data;

namespace Zerg.Common.Data
{
    public interface ICMSRepository<T> : IRepository<T> where T : class,IBaseEntity
    {
         
    }

    public interface ICRMRepository<T> : IRepository<T> where T : class,IBaseEntity
    {

    }

    public interface ITradingRepository<T> : IRepository<T> where T : class,IBaseEntity
    {

    }
}