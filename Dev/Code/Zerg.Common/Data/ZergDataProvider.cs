using System;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using YooPoon.Core.Data;
using YooPoon.Data.EntityFramework;
using YooPoon.Data.EntityFramework.Migrations;

namespace Zerg.Common.Data
{
    public class ZergDataProvider:SqlServerDataProvider
    {
        public override void InitDatabase()
        {
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            //依据model更新数据库
            Database.SetInitializer(new EfInitializer<CMSDbContext, CMSConfiguration>(dataProviderSettings.RawDataSettings["CMSConnection"]));
            Database.SetInitializer(new EfInitializer<CRMDbContext, CRMConfiguration>(dataProviderSettings.RawDataSettings["CRMConnection"]));
            Database.SetInitializer(new EfInitializer<TradingDbContext, TradingConfiguration>(dataProviderSettings.RawDataSettings["TradingConnection"]));

        }
    }
}
