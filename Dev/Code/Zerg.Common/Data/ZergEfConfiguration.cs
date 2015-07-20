using System.Data.Entity.Migrations;
using YooPoon.Data.EntityFramework;

namespace Zerg.Common.Data
{
    public class CMSConfiguration : DbMigrationsConfiguration<CMSDbContext>
    {
        public CMSConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;  //如果数据迁移时会发生数据丢失，false则抛出异常，true不抛出异常
        } 
    }

    public class CRMConfiguration : DbMigrationsConfiguration<CRMDbContext>
    {
        public CRMConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;  //如果数据迁移时会发生数据丢失，false则抛出异常，true不抛出异常
        }
    }

    public class TradingConfiguration : DbMigrationsConfiguration<TradingDbContext>
    {
        public TradingConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;  //如果数据迁移时会发生数据丢失，false则抛出异常，true不抛出异常
        }
    }

    public class EventConfiguration : DbMigrationsConfiguration<EventDbContext>
    {
        public EventConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;  //如果数据迁移时会发生数据丢失，false则抛出异常，true不抛出异常
        }
    }
}