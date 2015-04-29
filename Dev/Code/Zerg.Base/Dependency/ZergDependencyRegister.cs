using Autofac;
using Autofac.Core;
using YooPoon.Core.Autofac;
using YooPoon.Core.Data;
using YooPoon.Data.EntityFramework;
using Zerg.Common.Data;

namespace Zerg.Base.Dependency
{
    public class ZergDependencyRegister : IDependencyRegister
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var zergDataProvider = new ZergDataProvider();
            zergDataProvider.InitDatabase();

            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();

            //不同数据库对应不同的仓库
            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                builder.Register<IDbContext>(c => new CMSDbContext(dataProviderSettings.RawDataSettings["CMSConnection"])).Named<IDbContext>("CMS").InstancePerRequest();
                builder.Register<IDbContext>(c => new CRMDbContext(dataProviderSettings.RawDataSettings["CRMConnection"])).Named<IDbContext>("CRM").InstancePerRequest();
                builder.Register<IDbContext>(c => new TradingDbContext(dataProviderSettings.RawDataSettings["TradingConnection"])).Named<IDbContext>("Trading").InstancePerRequest();
            }

            #region 不同的数据库对应不同仓库
            var cmsParameter = new ResolvedParameter((pi, ctx) => pi.Name == "context",
                                            (pi, ctx) => ctx.ResolveNamed<IDbContext>("CMS"));
            var crmParameter = new ResolvedParameter((pi, ctx) => pi.Name == "context",
                                            (pi, ctx) => ctx.ResolveNamed<IDbContext>("CRM"));
            var tradingParameter = new ResolvedParameter((pi, ctx) => pi.Name == "context",
                                         (pi, ctx) => ctx.ResolveNamed<IDbContext>("Trading"));

            builder.RegisterGeneric(typeof(CMSRepository<>))
                .WithParameter(cmsParameter)
                .As(typeof(ICMSRepository<>))
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(CRMRepository<>))
                .WithParameter(crmParameter)
                .As(typeof(ICRMRepository<>))
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(TradingRepository<>))
                .WithParameter(tradingParameter)
                .As(typeof(ITradingRepository<>))
                .InstancePerRequest();
            #endregion
            //            builder.RegisterAssemblyTypes(typeFinder.GetAssemblies().ToArray())
            //                .Where(t =>!String.IsNullOrEmpty(t.Namespace)&& t.Namespace.StartsWith("CMS") && t.Name.Contains("Service"))
            //                .WithParameter(repositeryParameter);

        }

        public int Order { get { return 1; } }
    }
}